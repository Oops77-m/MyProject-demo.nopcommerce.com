// CI/CD для тестового проекта (NUnit + Selenium + Allure).
// Требования к Jenkins-агенту: Windows, .NET SDK 10, Google Chrome,
// плагин Allure Jenkins (https://plugins.jenkins.io/allure-jenkins-plugin/).
pipeline {
    agent any

    environment {
        // запуск Chrome без окна (BaseTest.cs реагирует на эту переменную)
        HEADLESS = 'true'
        DOTNET_CLI_TELEMETRY_OPTOUT = '1'
        DOTNET_NOLOGO = 'true'
    }

    options {
        // не держать устаревшие сборки
        buildDiscarder(logRotator(daysToKeepStr: '30', numToKeepStr: '20'))
        // отмена устаревших запусков той же ветки
        disableConcurrentBuilds()
        timestamps()
    }

    stages {
        stage('Restore and Build') {
            steps {
                bat 'dotnet restore MyProject-demo.nopcommerce.com.sln'
                bat 'dotnet build MyProject-demo.nopcommerce.com.sln --no-restore -v q'
            }
        }

        stage('Run Tests') {
            steps {
                bat 'dotnet test MyProject-demo.nopcommerce.com.sln --no-build --logger "trx;LogFileName=results.trx"'
            }
        }
    }

    post {
        always {
            // собрать allure-results из папки тестового проекта на уровень workspace
            powershell '''
                $src = "MyProject-demo.nopcommerce.com\\bin\\Debug\\net10.0\\allure-results"
                if (Test-Path $src) {
                    if (Test-Path "allure-results") { Remove-Item "allure-results" -Recurse -Force }
                    Copy-Item $src "allure-results" -Recurse
                    Write-Host "Copied: $((Get-ChildItem allure-results -File).Count) files"
                } else {
                    Write-Host "allure-results not found at $src"
                }
            '''
            // опубликовать Allure-отчёт (ссылка на отчёт — на странице сборки)
            allure results: [[path: 'allure-results']], reportBuildPolicy: 'ALWAYS'
            // сохранить сырые результаты (TRX + allure-results)
            archiveArtifacts artifacts: 'MyProject-demo.nopcommerce.com/TestResults/**, allure-results/**',
                allowEmptyArchive: true
            // TRX можно посмотреть отдельным шагом, если добавить плагин:
            // junit — только JUnit XML, поэтому TRX в базовой поставке Jenkins не парсится;
            // табличное отображение авто-тестов даёт Allure-отчёт выше.
        }
    }
}

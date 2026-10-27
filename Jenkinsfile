// CI/CD для тестового проекта (NUnit + Selenium + Allure).
// Jenkins работает в Linux-контейнере — все шаги через sh.
// На агенте могут отсутствовать .NET SDK и Chrome — первый этап это проверяет.
pipeline {
    agent any

    environment {
        // запуск Chrome без окна (BaseTest.cs реагирует на эту переменную)
        HEADLESS = 'true'
        // путь к бинарнику браузера в контейнере (BaseTest.cs использует как BinaryLocation)
        CHROME_BINARY_PATH = '/usr/bin/chromium'
        DOTNET_CLI_TELEMETRY_OPTOUT = '1'
        DOTNET_NOLOGO = 'true'
    }

    options {
        buildDiscarder(logRotator(daysToKeepStr: '30', numToKeepStr: '20'))
        disableConcurrentBuilds()
        timestamps()
    }

    stages {
        stage('Environment check') {
            steps {
                sh '''
                  echo "--- dotnet ---"
                  which dotnet && dotnet --version || echo "NO DOTNET"
                  echo "--- chrome/chromium ---"
                  which google-chrome google-chrome-stable chromium chromium-browser || echo "NO CHROME"
                  echo "--- OS ---"
                  cat /etc/os-release 2>/dev/null | head -2
                '''
            }
        }

        stage('Restore and Build') {
            steps {
                sh 'dotnet restore MyProject-demo.nopcommerce.com.sln'
                sh 'dotnet build MyProject-demo.nopcommerce.com.sln --no-restore -v q'
            }
        }

        stage('Run Tests') {
            steps {
                sh 'dotnet test MyProject-demo.nopcommerce.com.sln --no-build --logger "trx;LogFileName=results.trx"'
            }
        }
    }

    post {
        always {
            // собрать allure-results из папки тестового проекта на уровень workspace
            sh '''
              if [ -d "MyProject-demo.nopcommerce.com/bin/Debug/net10.0/allure-results" ]; then
                  rm -rf allure-results
                  cp -r "MyProject-demo.nopcommerce.com/bin/Debug/net10.0/allure-results" allure-results
                  echo "Copied: $(ls allure-results | wc -l) files"
              else
                  echo "allure-results not found"
              fi
            '''
            // опубликовать Allure-отчёт (ссылка на отчёт — на странице сборки)
            allure results: [[path: 'allure-results']], reportBuildPolicy: 'ALWAYS'
            // сохранить сырые результаты
            archiveArtifacts artifacts: 'MyProject-demo.nopcommerce.com/TestResults/**, allure-results/**, MyProject-demo.nopcommerce.com/bin/Debug/net10.0/chromedriver.log',
                allowEmptyArchive: true
        }
    }
}

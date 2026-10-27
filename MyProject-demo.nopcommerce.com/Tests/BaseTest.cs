using Allure.NUnit;
using Allure.NUnit.Attributes;
using MyProject_demo.nopcommerce.com.PageObject;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace MyProject_demo.nopcommerce.com;

// Allure: включает формирование отчёта для всех тестов
[AllureNUnit]
[AllureEpic("Automation Test Store")]
public class BaseTest
{
    protected IWebDriver driver;
    protected BasePage basePage;
    
    
    [SetUp]
    [AllureBefore("Открыть браузер и сайт")]
    public void Setup()
    {
        var options = new ChromeOptions();

        // в CI можно указать путь к бинарнику браузера (например, /usr/bin/chromium)
        var chromeBinary = Environment.GetEnvironmentVariable("CHROME_BINARY_PATH");
        if (!string.IsNullOrEmpty(chromeBinary))
            options.BinaryLocation = chromeBinary;

        // в CI (переменная HEADLESS=true) браузер запускается без окна
        if (Environment.GetEnvironmentVariable("HEADLESS") == "true")
        {
            options.AddArgument("--headless=new");
            options.AddArgument("--window-size=1920,1080");
            options.AddArgument("--disable-gpu");
        }

        // обязательные флаги для запуска Chrome/Chromium в Docker
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");

        // verbose-лог chromedriver — помогает диагностировать, почему браузер не стартует
        var service = ChromeDriverService.CreateDefaultService();
        service.EnableVerboseLogging = true;
        service.LogPath = Path.Combine(AppContext.BaseDirectory, "chromedriver.log");

        driver = new ChromeDriver(service, options);
        basePage = new BasePage(driver);
        basePage.OpenAutomationteststore();
        
    }
    
    [TearDown]
    [AllureAfter("Закрыть браузер")]
    public void TearDown()
    {
        driver?.Quit();
        driver?.Dispose();
    }
}
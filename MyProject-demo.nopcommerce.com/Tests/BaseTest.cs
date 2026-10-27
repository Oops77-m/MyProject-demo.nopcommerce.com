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
        // в CI (переменная HEADLESS=true) браузер запускается без окна
        var options = new ChromeOptions();
        if (Environment.GetEnvironmentVariable("HEADLESS") == "true")
        {
            options.AddArgument("--headless=new");
            options.AddArgument("--window-size=1920,1080");
            options.AddArgument("--disable-gpu");
        }
        driver = new ChromeDriver(options);
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
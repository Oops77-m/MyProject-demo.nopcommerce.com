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
        driver = new ChromeDriver();
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
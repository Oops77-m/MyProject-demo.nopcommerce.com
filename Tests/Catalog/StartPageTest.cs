using Allure.Net.Commons;
using Allure.NUnit.Attributes;
using OpenQA.Selenium;

namespace MyProject_demo.nopcommerce.com.Tests;

[AllureFeature("Главная страница")]
public class StartPageTest : BaseTest
{ private readonly By _title = By.CssSelector("img[alt='Automation Test Store']");
    [AllureStory("Главная страница")]
    [AllureDescription("Проверить, что главная страница открылась и логотип отображается")]
    [AllureSeverity(SeverityLevel.normal)]
    [Test]
    public void StartPage_IsOpened()
    {
        Assert.That(basePage.GetUrl(), Is.EqualTo("https://automationteststore.com/"),
            "URL главной страницы неверный");
        Assert.That(driver.FindElement(_title).Displayed, Is.True,
            "Логотип не отображается");
    }
}
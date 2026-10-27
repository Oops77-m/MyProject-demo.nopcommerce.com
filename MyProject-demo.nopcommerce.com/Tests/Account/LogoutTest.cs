using Allure.Net.Commons;
using Allure.NUnit.Attributes;
using MyProject_demo.nopcommerce.com.PageObject;

namespace MyProject_demo.nopcommerce.com.Tests;

[AllureFeature("Аккаунт")]
public class LogoutTest : BaseTest
{
    private AccountPage accountPage;
    private LoginPage loginPage;

    // перед тестом создаём объекты страниц (браузер и открытие сайта делает BaseTest)
    [SetUp]
    public void CreatePages()
    {
        accountPage = new AccountPage(driver);
        loginPage = new LoginPage(driver);
    }

    // выход: регистрация (авто-вход) → страница аккаунта → Logout → страница логина
    [AllureStory("Выход из аккаунта")]
    [AllureDescription("Проверить, что выход со страницы аккаунта возвращает на страницу логина")]
    [AllureSeverity(SeverityLevel.normal)]
    [Test]
    public void Logout_FromAccountPage_ReturnsToLogin()
    {
        // 1. Генерируем уникальные данные
        var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        var email = $"testuser_{timestamp}@test.com";
        var loginName = $"testuser_{timestamp}";

        // 2. Регистрируем пользователя (после регистрации он автоматически вошёл)
        var registerPage = new RegisterPage(driver);
        registerPage.OpenRegisterPage();
        registerPage.FillPersonalDetails("Test", "User", email);
        registerPage.FillAddress("1 Test Street", "London", "SW1A 1AA");
        registerPage.FillLoginDetails(loginName, "TestPass123!");
        registerPage.CheckPrivacyPolicy();
        registerPage.Submit();

        // 3. Проверяем, что мы на странице аккаунта (то есть залогинены)
        Assert.That(accountPage.IsAccountPageOpened(), Is.True,
            "Страница аккаунта не открыта");

        // 4. Выходим
        accountPage.ClickLogout();

        // 5. Проверяем, что вернулись на страницу логина
        Assert.That(loginPage.IsLoginPageOpened(), Is.True,
            "После выхода не открылась страница логина");
    }
}

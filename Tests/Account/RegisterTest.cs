using Allure.Net.Commons;
using Allure.NUnit.Attributes;
using MyProject_demo.nopcommerce.com.PageObject;

namespace MyProject_demo.nopcommerce.com.Tests;

[AllureFeature("Аккаунт")]
public class RegisterTest : BaseTest
{
    private LoginPage loginPage;
    private RegisterPage registerPage;

    // перед каждым тестом создаём объекты страниц (браузер и открытие сайта делает BaseTest)
    [SetUp]
    public void CreatePages()
    {
        loginPage = new LoginPage(driver);
        registerPage = new RegisterPage(driver);
    }

    // регистрация с верными данными: аккаунт создан, пользователь на странице аккаунта
    [AllureStory("Регистрация")]
    [AllureDescription("Проверить, что при регистрации с верными данными создаётся аккаунт")]
    [AllureSeverity(SeverityLevel.critical)]
    [Test]
    public void Register_WithValidData_CreatesAccount()
    {
        // 1. Генерируем уникальные данные, чтобы повторный запуск не падал
        var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        var email = $"testuser_{timestamp}@test.com";
        var loginName = $"testuser_{timestamp}";

        // 2. Открываем страницу логина и переходим на регистрацию
        loginPage.OpenLoginPage();
        registerPage = loginPage.GoToRegisterPage();
        Assert.That(registerPage.IsRegisterPageOpened(), Is.True,
            "Страница регистрации не открылась");

        // 3. Заполняем форму
        registerPage.FillPersonalDetails("Test", "User", email);
        registerPage.FillAddress("1 Test Street", "London", "SW1A 1AA");
        registerPage.FillLoginDetails(loginName, "TestPass123!");
        registerPage.CheckPrivacyPolicy();

        // 4. Отправляем форму
        registerPage.Submit();

        // 5. Проверяем, что пользователь на странице аккаунта
        Assert.That(driver.Url, Does.Contain("rt=account/success"),
            "После регистрации не открылась страница аккаунта");
        Assert.That(registerPage.IsSuccessMessageDisplayed(), Is.True,
            "Success-сообщение не отображается");
    }
}

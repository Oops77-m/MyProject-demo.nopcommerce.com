using Allure.Net.Commons;
using Allure.NUnit.Attributes;
using MyProject_demo.nopcommerce.com.PageObject;
using OpenQA.Selenium.Support.UI;

namespace MyProject_demo.nopcommerce.com.Tests;

[AllureFeature("Аккаунт")]
public class LoginTest : BaseTest
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

    // вход по верным данным ранее зарегистрированного пользователя
    [AllureStory("Вход пользователя")]
    [AllureDescription("Проверить, что вход проходит по верным данным ранее зарегистрированного пользователя")]
    [AllureSeverity(SeverityLevel.critical)]
    [Test]
    public void Login_WithValidData_LogsIn()
    {
        // 1. Генерируем уникальные данные
        var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        var email = $"testuser_{timestamp}@test.com";
        var loginName = $"testuser_{timestamp}";
        var password = "TestPass123!";

        // 2. Сначала регистрируем пользователя
        registerPage.OpenRegisterPage();
        registerPage.FillPersonalDetails("Test", "User", email);
        registerPage.FillAddress("1 Test Street", "London", "SW1A 1AA");
        registerPage.FillLoginDetails(loginName, password);
        registerPage.CheckPrivacyPolicy();
        registerPage.Submit();
        Assert.That(driver.Url, Does.Contain("rt=account/success"),
            "Регистрация не прошла");

        // 3. Сбрасываем сессию (как будто закрыли браузер) и возвращаемся на главную
        driver.Manage().Cookies.DeleteAllCookies();
        basePage.OpenAutomationteststore();

        // 4. Открываем страницу логина и входим с данными из регистрации
        loginPage.OpenLoginPage();
        loginPage.Login(loginName, password);

        // 5. Проверяем, что пользователь на странице аккаунта
        // после входа сайт может вести на account/account (или на account/success)
        Assert.That(driver.Url, Does.Contain("rt=account/"),
            "Вход не прошёл");
    }

    // вход с некорректными данными: остаёмся на странице логина, появляется ошибка
    [AllureStory("Вход пользователя")]
    [AllureDescription("Проверить, что при входе с некорректными данными показывается ошибка")]
    [AllureSeverity(SeverityLevel.normal)]
    [Test]
    public void Login_WithInvalidData_ShowsError()
    {
        // 1. Открываем страницу логина
        loginPage.OpenLoginPage();

        // 2. Вводим несуществующий логин и пароль
        loginPage.Login("wronguser_xyz", "WrongPass123!");

        // 3. Ждём появления сообщения об ошибке
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
        wait.Until(_ => loginPage.IsErrorMessageDisplayed());

        // 4. Проверяем, что мы всё ещё на странице логина
        Assert.That(loginPage.IsLoginPageOpened(), Is.True,
            "После неверного входа не остаёмся на странице логина");
    }
}

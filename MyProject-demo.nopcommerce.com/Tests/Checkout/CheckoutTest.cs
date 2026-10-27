using Allure.Net.Commons;
using Allure.NUnit.Attributes;
using MyProject_demo.nopcommerce.com.PageObject;
using MyProject_demo.nopcommerce.com.PageObject.Category;

namespace MyProject_demo.nopcommerce.com.Tests;

[AllureFeature("Оформление заказа")]
public class CheckoutTest : BaseTest
{
    // товар для заказа: Delicate Oil-Free Powder Blush, $29.50
    private const string ProductName = "Delicate Oil-Free Powder Blush";

    private CartPage cartPage;

    // перед тестом создаём объект корзины (браузер и открытие сайта делает BaseTest)
    [SetUp]
    public void CreatePages()
    {
        cartPage = new CartPage(driver);
    }

    // открыть карточку товара через категории: Makeup → Face → товар
    [AllureStep("Открыть карточку товара через категории")]
    private OpenProductCardPage OpenProductCard()
    {
        var categoryPage = new ProductCategoryPage(driver);
        categoryPage.ClickMakeupFaceSubcategory();

        var facePage = new FacePage(driver);
        return facePage.ClickDelicateBlush();
    }

    // полный сценарий заказа гостем: корзина → доставка → оплата → подтверждение
    [AllureStory("Заказ гостем")]
    [AllureDescription("Проверить полный сценарий заказа гостем: корзина → доставка → оплата → подтверждение")]
    [AllureSeverity(SeverityLevel.critical)]
    [Test]
    public void Checkout_FullOrderAsGuest()
    {
        // 1. Добавляем товар в корзину
        var card = OpenProductCard();
        cartPage = card.ClickAddToCart();
        Assert.That(cartPage.IsProductInCart(ProductName), Is.True,
            "Товар не добавлен в корзину");

        // 2. Переходим к оформлению заказа: для нового гостя сайт показывает Checkout Options
        var optionsPage = cartPage.ClickCheckout();
        Assert.That(optionsPage.IsCheckoutOptionsPageOpened(), Is.True,
            "Страница Checkout Options не открылась");

        // 3. Выбираем Guest Checkout и открываем страницу доставки
        var checkoutPage = optionsPage.ContinueAsGuest();
        Assert.That(checkoutPage.IsShippingPageOpened(), Is.True,
            "Страница доставки не открылась");

        // 4. Заполняем данные доставки (гость, уникальный email)
        var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        checkoutPage.FillShippingDetails("Test", "User",
            $"testuser_{timestamp}@test.com",
            "1 Test Street", "London", "SW1A 1AA");

        // 5. Переходим к подтверждению заказа
        var confirmPage = checkoutPage.ContinueToConfirmation();
        Assert.That(confirmPage.IsConfirmPageOpened(), Is.True,
            "Страница подтверждения не открылась");

        // 6. Подтверждаем и проверяем, что заказ оформлен
        var orderPage = confirmPage.ConfirmOrder();
        Assert.That(orderPage.IsOrderPlaced(), Is.True,
            "Заказ не оформлен");
        Assert.That(orderPage.IsThankYouMessageDisplayed(), Is.True,
            "Сообщение Thank you не отображается");
    }

    // полный цикл: регистрация → вход → корзина → checkout → подтверждение
    [AllureStory("Заказ из аккаунта")]
    [AllureDescription("Проверить полный цикл: регистрация → вход → корзина → заказ → подтверждение")]
    [AllureSeverity(SeverityLevel.critical)]
    [Test]
    public void Checkout_LoggedIn_FullOrder()
    {
        // 1. Генерируем уникальные данные
        var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        var email = $"testuser_{timestamp}@test.com";
        var loginName = $"testuser_{timestamp}";
        var password = "TestPass123!";

        // 2. Регистрируем пользователя
        var registerPage = new RegisterPage(driver);
        registerPage.OpenRegisterPage();
        registerPage.FillPersonalDetails("Test", "User", email);
        registerPage.FillAddress("1 Test Street", "London", "SW1A 1AA");
        registerPage.FillLoginDetails(loginName, password);
        registerPage.CheckPrivacyPolicy();
        registerPage.Submit();
        Assert.That(driver.Url, Does.Contain("rt=account/success"),
            "Регистрация не прошла");

        // 3. Сбрасываем сессию (как будто закрыли браузер)
        driver.Manage().Cookies.DeleteAllCookies();
        basePage.OpenAutomationteststore();

        // 4. Входим с данными из регистрации
        var loginPage = new LoginPage(driver);
        loginPage.OpenLoginPage();
        loginPage.Login(loginName, password);
        // после входа сайт может вести на account/account (или на account/success)
        Assert.That(driver.Url, Does.Contain("rt=account/"),
            "Вход не прошёл");

        // 5. Добавляем товар в корзину
        var card = OpenProductCard();
        cartPage = card.ClickAddToCart();
        Assert.That(cartPage.IsProductInCart(ProductName), Is.True,
            "Товар не добавлен в корзину");

        // 6. Переходим к оформлению заказа: вошедший пользователь идёт сразу на подтверждение
        var checkoutPage = cartPage.ClickCheckout();
        Assert.That(checkoutPage.IsConfirmationPageOpened(), Is.True,
            "Страница подтверждения не открылась");

        // 7. Проверяем, что имя и фамилия подставились из аккаунта
        Assert.That(checkoutPage.GetPageText(), Does.Contain("Test User"),
            "Имя и фамилия не подставились из аккаунта");

        // 8. Подтверждаем и проверяем, что заказ оформлен
        var orderPage = checkoutPage.ConfirmOrder();
        Assert.That(orderPage.IsOrderPlaced(), Is.True,
            "Заказ не оформлен");
        Assert.That(orderPage.IsThankYouMessageDisplayed(), Is.True,
            "Сообщение Thank you не отображается");
    }
}

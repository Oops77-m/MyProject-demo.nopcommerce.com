using Allure.Net.Commons;
using Allure.NUnit.Attributes;
using MyProject_demo.nopcommerce.com.PageObject;
using MyProject_demo.nopcommerce.com.PageObject.Category;

namespace MyProject_demo.nopcommerce.com.Tests;

[AllureFeature("Корзина")]
public class CartTest : BaseTest
{
    // товар для тестов: Delicate Oil-Free Powder Blush, $29.50
    private const string ProductName = "Delicate Oil-Free Powder Blush";

    private OpenProductCardPage productCardPage;
    private CartPage cartPage;

    // перед каждым тестом создаём объекты страниц (браузер и открытие сайта делает BaseTest)
    [SetUp]
    public void CreatePages()
    {
        productCardPage = new OpenProductCardPage(driver);
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

    // добавить товар в корзину: карточка → Add to Cart
    [AllureStep("Добавить товар в корзину")]
    private CartPage AddProductToCart()
    {
        var card = OpenProductCard();
        return card.ClickAddToCart();
    }

    // добавить товар в корзину, проверить имя и количество
    [AllureStory("Добавление товара в корзину")]
    [AllureDescription("Проверить, что товар добавлен в корзину и отображаются имя и количество 1")]
    [AllureSeverity(SeverityLevel.critical)]
    [Test]
    public void AddToCart_ProductAppearsInCart()
    {
        cartPage = AddProductToCart();

        // 1. открылась страница корзины
        Assert.That(cartPage.IsCartPageOpened(), Is.True,
            "Страница корзины не открылась");

        // 2. товар в корзине, количество = 1
        Assert.That(cartPage.IsProductInCart(ProductName), Is.True,
            "Товар не найден в корзине");
        Assert.That(cartPage.GetQuantity(), Is.EqualTo(1),
            "Количество не равно 1");
    }

    // изменить количество товара, сумма пересчитывается
    [AllureStory("Изменение количества в корзине")]
    [AllureDescription("Проверить, что при изменении количества сумма пересчитывается")]
    [AllureSeverity(SeverityLevel.normal)]
    [Test]
    public void ChangeQuantity_TotalUpdates()
    {
        cartPage = AddProductToCart();

        // 1. меняем количество на 3
        cartPage.SetQuantity(3);

        // 2. проверяем количество и сумму (29.50 * 3 = 88.50)
        Assert.That(cartPage.GetQuantity(), Is.EqualTo(3),
            "Количество не изменилось на 3");
        Assert.That(cartPage.GetTotal(), Does.Contain("$88.50"),
            "Сумма не пересчиталась");
    }

    // удалить товар из корзины, корзина становится пустой
    [AllureStory("Удаление товара из корзины")]
    [AllureDescription("Проверить, что при удалении товара корзина становится пустой")]
    [AllureSeverity(SeverityLevel.normal)]
    [Test]
    public void RemoveProduct_CartBecomesEmpty()
    {
        cartPage = AddProductToCart();

        // 1. удаляем товар
        cartPage.RemoveProduct();

        // 2. проверяем, что корзина пуста
        Assert.That(cartPage.IsCartEmpty(), Is.True,
            "Корзина не пуста");
        Assert.That(cartPage.GetProductCount(), Is.Zero,
            "В корзине остались товары");
    }
}

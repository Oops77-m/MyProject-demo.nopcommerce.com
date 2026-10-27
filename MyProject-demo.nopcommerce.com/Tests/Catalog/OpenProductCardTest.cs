using Allure.Net.Commons;
using Allure.NUnit.Attributes;
using MyProject_demo.nopcommerce.com.PageObject;
using MyProject_demo.nopcommerce.com.PageObject.Category;

namespace MyProject_demo.nopcommerce.com.Tests;

[TestFixture]
[AllureFeature("Карточка товара")]
public class OpenProductCardTest : BaseTest
{
    private OpenProductCardPage productCardPage;

    [SetUp]
    public void CreatePage()
    {
        productCardPage = new OpenProductCardPage(driver);
    }

    [AllureStory("Карточка товара")]
    [AllureDescription("Проверить карточку товара: имя, цена, модель, кнопка Add to Cart")]
    [AllureSeverity(SeverityLevel.critical)]
    [Test]
    public void OpenProductCard_FromFaceCategory()
    {
        // 1. Переходим в Makeup → Face
        var categoryPage = new ProductCategoryPage(driver);
        categoryPage.ClickMakeupFaceSubcategory();

        // 2. Проверяем, что открылась страница Face
        Assert.That(driver.Url, Does.Contain("path=36_38"));
        // 3. Кликаем 
        FacePage facePage = new FacePage(driver);
        var productPage = facePage.ClickDelicateBlush();
        

        // 4. Проверяем, что открылась карточка товара
       
        Assert.That(productPage.GetUrl(),
            Is.EqualTo("https://automationteststore.com/index.php?rt=product/product&path=36_38&product_id=57"));
        Assert.Multiple(() =>
        {
            Assert.That(productPage.IsProductNameDisplayed(), Is.True);
            Assert.That(productPage.GetProductName(),
                Is.EqualTo("Delicate Oil-Free Powder Blush"),
                "Имя товара не совпадает");

            Assert.That(productPage.IsProductPriceDisplayed(), Is.True,
                "Цена не отображается");
            Assert.That(productPage.GetProductPrice(),
                Is.EqualTo("$29.50"),
                "Цена не совпадает");

            Assert.That(productPage.IsModelRowDisplayed(), Is.True,
                "Строка Model не отображается");
            Assert.That(productPage.GetModelNumber(),
                Is.EqualTo("117148"),
                "Номер модели не совпадает");

            Assert.That(productPage.IsAddToCartButtonDisplayed(), Is.True,
                "Кнопка Add to Cart не отображается");
        });
    }
    
}
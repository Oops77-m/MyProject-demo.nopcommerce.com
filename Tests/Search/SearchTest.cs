using Allure.Net.Commons;
using Allure.NUnit.Attributes;
using MyProject_demo.nopcommerce.com.PageObject;

namespace MyProject_demo.nopcommerce.com.Tests;

[AllureFeature("Поиск")]
public class SearchTest : BaseTest
{
    private SearchPage searchPage;

    // перед каждым тестом создаём объект страницы поиска (браузер и открытие сайта делает BaseTest)
    [SetUp]
    public void CreatePage()
    {
        searchPage = new SearchPage(driver);
    }

    // поиск по части имени: "cream" должен вернуть 4 товара
    [AllureStory("Поиск товара")]
    [AllureDescription("Проверить, что поиск по части имени товара возвращает 4 товара")]
    [AllureSeverity(SeverityLevel.normal)]
    [Test]
    public void Search_Cream_ReturnsFourProducts()
    {
        searchPage.SearchByKeyword("cream");

        Assert.That(searchPage.IsSearchPageOpened(), Is.True,
            "Страница поиска не открылась");
        Assert.That(searchPage.GetSearchResultsCount(), Is.EqualTo(4),
            "Количество результатов не равно 4");

        // ожидаемые имена товаров
        var expected = new[]
        {
            "Skin Minerals For Men Cleansing Cream",
            "Body Cream by Bulgari",
            "Shaving cream",
            "Total Moisture Facial Cream"
        };
        var actual = searchPage.GetSearchResultNames();
        Assert.Multiple(() =>
        {
            // каждый ожидаемый товар должен быть в результатах (сайт отдаёт названия в верхнем регистре)
            foreach (var name in expected)
                Assert.That(actual.Contains(name, StringComparer.OrdinalIgnoreCase), Is.True,
                    $"Результат '{name}' не найден");
        });
    }

    // поиск по точному имени товара: сайт редиректит сразу на карточку
    [AllureStory("Поиск товара")]
    [AllureDescription("Проверить, что поиск по точному имени товара ведёт на карточку товара")]
    [AllureSeverity(SeverityLevel.normal)]
    [Test]
    public void Search_ExactProductName_RedirectsToProductCard()
    {
        searchPage.SearchByKeyword("Shaving cream");

        Assert.That(driver.Url, Does.Contain("product_id=98"),
            "Редирект на карточку товара не произошёл");

        var card = new OpenProductCardPage(driver);
        Assert.Multiple(() =>
        {
            Assert.That(card.GetProductName(), Is.EqualTo("Shaving cream"),
                "Имя товара не совпадает");
            Assert.That(card.GetProductPrice(), Is.EqualTo("$98.00"),
                "Цена не совпадает");
        });
    }

    // поиск несуществующего товара: должно быть сообщение "не найдено" и 0 результатов
    [AllureStory("Поиск товара")]
    [AllureDescription("Проверить, что поиск несуществующего товара показывает сообщение о пустом результате")]
    [AllureSeverity(SeverityLevel.normal)]
    [Test]
    public void Search_NoResults_ShowsMessage()
    {
        searchPage.SearchByKeyword("xyznonexistent123");

        Assert.That(searchPage.IsSearchPageOpened(), Is.True,
            "Страница поиска не открылась");
        Assert.That(searchPage.IsNoResultsMessageDisplayed(), Is.True,
            "Сообщение о пустом результате не отображается");
        Assert.That(searchPage.GetSearchResultsCount(), Is.Zero,
            "Результаты должны быть пустыми");
    }

    // клик по лупе с пустым полем: тоже должно быть сообщение "не найдено"
    [AllureStory("Поиск товара")]
    [AllureDescription("Проверить, что пустой поиск показывает сообщение о пустом результате")]
    [AllureSeverity(SeverityLevel.minor)]
    [Test]
    public void Search_EmptyQuery_ShowsNoResults()
    {
        searchPage.SearchByKeyword("");

        Assert.That(searchPage.IsNoResultsMessageDisplayed(), Is.True,
            "Сообщение о пустом результате не отображается");
    }

    // клик по результату из списка открывает карточку товара
    [AllureStory("Поиск товара")]
    [AllureDescription("Проверить, что клик по результату поиска открывает карточку товара")]
    [AllureSeverity(SeverityLevel.normal)]
    [Test]
    public void Search_ClickResult_OpensProductCard()
    {
        searchPage.SearchByKeyword("cream");
        var card = searchPage.ClickResultByName("Body Cream by Bulgari");

        Assert.That(card.GetUrl(), Does.Contain("product_id=92"),
            "Карточка товара не открылась");
        Assert.That(card.GetProductName(), Is.EqualTo("Body Cream by Bulgari"),
            "Имя товара не совпадает");
    }
}

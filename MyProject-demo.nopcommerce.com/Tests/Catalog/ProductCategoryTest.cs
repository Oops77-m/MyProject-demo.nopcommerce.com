using Allure.Net.Commons;
using Allure.NUnit.Attributes;
using MyProject_demo.nopcommerce.com.PageObject;
using OpenQA.Selenium;

namespace MyProject_demo.nopcommerce.com.Tests;


[AllureFeature("Категории")]
public class ProductCategoryTest : BaseTest
{
    private ProductCategoryPage categoryPage;

    [SetUp]
    public void CreatePage()
    {
        categoryPage = new ProductCategoryPage(driver);
    }
    [AllureStory("Категории в меню")]
    [AllureDescription("Проверить, что в меню отображаются все 6 категорий верхнего уровня")]
    [AllureSeverity(SeverityLevel.normal)]
    [Test]
    
    public void IsCategoryDisplayed()
    {
        Assert.Multiple(() =>
        {
            Assert.That(categoryPage.IsApparelAndAccesCategoryDisplayed(), Is.True, "Apparel & accessories");
            Assert.That(categoryPage.IsMakeupCategoryDisplayed(),           Is.True, "Makeup");
            Assert.That(categoryPage.IsSkincareCategoryDisplayed(),         Is.True, "Skincare");
            Assert.That(categoryPage.IsFragranceCategoryDisplayed(),        Is.True, "Fragrance");
            Assert.That(categoryPage.IsMenCategoryDisplayed(),              Is.True, "Men");
            Assert.That(categoryPage.IsBooksCategoryDisplayed(),            Is.True, "Books");
        });
    }

    [AllureStory("Категории в меню")]
    [AllureDescription("Проверить, что клик по категории Makeup открывает страницу категории")]
    [AllureSeverity(SeverityLevel.normal)]
    [Test]
    public void ChoiceCategoryMakeup()
    {
        categoryPage.ClickMakeupCategory();

        Assert.That(driver.Url, Does.Contain("path=36"),
            "Переход в категорию Makeup не произошёл");
    }
    
    
}
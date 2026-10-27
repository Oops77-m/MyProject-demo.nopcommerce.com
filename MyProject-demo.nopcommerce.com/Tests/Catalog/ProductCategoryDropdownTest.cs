using Allure.Net.Commons;
using Allure.NUnit.Attributes;
using MyProject_demo.nopcommerce.com.PageObject;
using NUnit.Framework;
using OpenQA.Selenium;

namespace MyProject_demo.nopcommerce.com.Tests;

[AllureFeature("Категории")]
public class ProductCategoryDropdownTest : BaseTest
{
    private ProductCategoryPage categoryPage;

    [SetUp]
    public void CreatePage()
    {
        categoryPage = new ProductCategoryPage(driver);
    }

    private void OpenMakeupDropdown()
    {
        categoryPage.HoverMakeupCategory();
        Thread.Sleep(500); // даём CSS-переходу примениться
    }

    [AllureStory("Dropdown подкатегорий Makeup")]
    [AllureDescription("Проверить, что подкатегории скрыты до наведения на категорию")]
    [AllureSeverity(SeverityLevel.minor)]
    [Test]
    public void MakeupDropdown_IsHiddenBeforeHover()
    {
        Assert.That(categoryPage.IsMakeupSubcategoriesContainerDisplayed(), Is.False); //подкатегории Makeup скрыты до наведения
    }

    [AllureStory("Dropdown подкатегорий Makeup")]
    [AllureDescription("Проверить, что список подкатегорий появляется при наведении на категорию")]
    [AllureSeverity(SeverityLevel.normal)]
    [Test]
    public void MakeupDropdown_AppearsOnHover()
    {
        OpenMakeupDropdown();

        Assert.That(categoryPage.IsMakeupSubcategoriesContainerDisplayed(), Is.True); //подкатегории Makeup не появились после наведения
    }

    [TestCase("Cheeks")]
    [TestCase("Eyes")]
    [TestCase("Face")]
    [TestCase("Lips")]
    [TestCase("Nails")]
    [TestCase("Value Sets")]
    [AllureStory("Dropdown подкатегорий Makeup")]
    [AllureDescription("Проверить, что подкатегория отображается в dropdown")]
    [AllureSeverity(SeverityLevel.normal)]
    public void MakeupDropdown_SubcategoryIsDisplayed(string name)
    {
        OpenMakeupDropdown(); // ❗ без hover подкатегория скрыта

        Assert.That(categoryPage.IsMakeupSubcategoryDisplayed(name), Is.True,
            $"{name} не отображается");
    }

    [AllureStory("Dropdown подкатегорий Makeup")]
    [AllureDescription("Проверить, что у категории Makeup 6 подкатегорий")]
    [AllureSeverity(SeverityLevel.normal)]
    [Test]
    public void MakeupDropdown_HasSixSubcategories()
    {
        Assert.That(categoryPage.MakeupSubcategoriesCount(), Is.EqualTo(6)); //Makeup должно быть 6 подкатегорий);
    }

    [AllureStory("Dropdown подкатегорий Makeup")]
    [AllureDescription("Проверить, что клик по подкатегории Face открывает страницу подкатегории")]
    [AllureSeverity(SeverityLevel.normal)]
    [Test]
    public void MakeupDropdown_FaceOpen()
    {
        categoryPage.ClickMakeupFaceSubcategory();

        Assert.That(driver.Url, Does.Contain("path=36_38"),
            "Переход на подкатегорию Face не произошёл");
    }
   
    
}
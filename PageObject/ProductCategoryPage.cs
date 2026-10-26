using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace MyProject_demo.nopcommerce.com.PageObject;

public class ProductCategoryPage : BasePage
{
    public ProductCategoryPage(IWebDriver driver) : base(driver)
    {
    }

    // === Категории верхнего уровня ===
    private readonly By _makeupCategory =
        By.XPath("//ul[contains(@class,'categorymenu')]//a[contains(@href, 'path=36') and not(contains(@href, 'path=36_'))]");

    private readonly By _apparelAddAccesCategory =
        By.XPath("//ul[contains(@class,'categorymenu')]//a[contains(@href, 'path=68') and not(contains(@href, 'path=68_'))]");

    private readonly By _skincareCategory =
        By.XPath("//ul[contains(@class,'categorymenu')]//a[contains(@href, 'path=43') and not(contains(@href, 'path=43_'))]");

    private readonly By _fragranceCategory =
        By.XPath("//ul[contains(@class,'categorymenu')]//a[contains(@href, 'path=49') and not(contains(@href, 'path=49_'))]");

    private readonly By _menCategory =
        By.XPath("//ul[contains(@class,'categorymenu')]//a[contains(@href, 'path=58') and not(contains(@href, 'path=58_'))]");

    private readonly By _hairCareCategory =
        By.XPath("//ul[contains(@class,'categorymenu')]//a[contains(@href, 'path=52') and not(contains(@href, 'path=52_'))]");

    private readonly By _booksCategory =
        By.XPath("//ul[contains(@class,'categorymenu')]//a[contains(@href, 'path=65') and not(contains(@href, 'path=65_'))]");

    // === Dropdown Makeup ===
    private readonly By _makeupSubcategoriesContainer =
        By.XPath("//ul[contains(@class,'categorymenu')]//a[contains(@href, 'path=36') and not(contains(@href, 'path=36_'))]/following-sibling::div[contains(@class,'subcategories')]");

    private readonly By _title = By.CssSelector(".page-title h1, h1");
    
    private static readonly Dictionary<string, By> MakeupSubcategories = new()
    {
        ["Cheeks"]     = By.XPath("//ul[contains(@class,'categorymenu')]//a[contains(@href, 'path=36_40')]"),
        ["Eyes"]       = By.XPath("//ul[contains(@class,'categorymenu')]//a[contains(@href, 'path=36_39')]"),
        ["Face"]       = By.XPath("//ul[contains(@class,'categorymenu')]//a[contains(@href, 'path=36_38')]"),
        ["Lips"]       = By.XPath("//ul[contains(@class,'categorymenu')]//a[contains(@href, 'path=36_41')]"),
        ["Nails"]      = By.XPath("//ul[contains(@class,'categorymenu')]//a[contains(@href, 'path=36_42')]"),
        ["Value Sets"] = By.XPath("//ul[contains(@class,'categorymenu')]//a[contains(@href, 'path=36_37')]"),
    };

    // === Состояния категорий ===

    public bool IsApparelAndAccesCategoryDisplayed()
    {
        try { return _driver.FindElement(_apparelAddAccesCategory).Displayed; }
        catch (NoSuchElementException) { return false; }
    }

    public bool IsMakeupCategoryDisplayed()
    {
        try { return _driver.FindElement(_makeupCategory).Displayed; }
        catch (NoSuchElementException) { return false; }
    }

    public bool IsSkincareCategoryDisplayed()
    {
        try { return _driver.FindElement(_skincareCategory).Displayed; }
        catch (NoSuchElementException) { return false; }
    }

    public bool IsFragranceCategoryDisplayed()
    {
        try { return _driver.FindElement(_fragranceCategory).Displayed; }
        catch (NoSuchElementException) { return false; }
    }

    public bool IsMenCategoryDisplayed()
    {
        try { return _driver.FindElement(_menCategory).Displayed; }
        catch (NoSuchElementException) { return false; }
    }

    public bool IsBooksCategoryDisplayed()
    {
        try { return _driver.FindElement(_booksCategory).Displayed; }
        catch (NoSuchElementException) { return false; }
    }

    // === Действия ===

    public void ClickMakeupCategory()
    {
        _driver.FindElement(_makeupCategory).Click();
    }

    public void HoverMakeupCategory()
    {
        var element = _driver.FindElement(_makeupCategory);
        new Actions(_driver).MoveToElement(element).Perform();
    }

    // === Состояния dropdown Makeup ===

    public bool IsMakeupSubcategoriesContainerDisplayed()
    {
        try { return _driver.FindElement(_makeupSubcategoriesContainer).Displayed; }
        catch (NoSuchElementException) { return false; }
    }

    public bool IsMakeupSubcategoryDisplayed(string name)
    {
        try { return _driver.FindElement(MakeupSubcategories[name]).Displayed; }
        catch (NoSuchElementException) { return false; }
    }

    public int MakeupSubcategoriesCount()
    {
        return _driver.FindElements(
            By.XPath("//ul[contains(@class,'categorymenu')]//a[contains(@href, 'path=36_')]")).Count;
    }
    public bool IsPageOpened()
    {
        try
        {
            return _driver.FindElement(_title).Displayed
                   && _driver.Url.Contains("path=36_38");
        }
        catch (NoSuchElementException)
        {
            return false;
        }
    }
    public void ClickMakeupFaceSubcategory()
    {
        HoverMakeupCategory();
        Thread.Sleep(500);

        var element = _driver.FindElement(MakeupSubcategories["Face"]);
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", element);
    }
    
}
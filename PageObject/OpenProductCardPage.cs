using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace MyProject_demo.nopcommerce.com.PageObject;

public class OpenProductCardPage : BasePage
{
    public OpenProductCardPage(IWebDriver driver) : base(driver)
    {
    }
    // ЗАМЕНИТЕ на реальные селекторы из DevTools
    private readonly By _productPrice = By.CssSelector(".productfilneprice");
    private readonly By _productName = By.CssSelector("span.bgnone");
    private readonly By _modelRow =
        By.XPath("//li[span[contains(@class,'productinfoleft') and contains(., 'Model')]]");
    private readonly By _addToCartButton = By.CssSelector("a.cart, button.cart");
    
    public bool IsProductPriceDisplayed()
    {
        try { return _driver.FindElement(_productPrice).Displayed; }
        catch (NoSuchElementException) { return false; }
    }
    

    public bool IsProductNameDisplayed()
    {
        try { return _driver.FindElement(_productName).Displayed; }
        catch (NoSuchElementException) { return false; }
    }

    public bool IsModelRowDisplayed()
    {
        try { return _driver.FindElement(_modelRow).Displayed; }
        catch (NoSuchElementException) { return false; }
    }

    public bool IsAddToCartButtonDisplayed()
    {
        try { return _driver.FindElement(_addToCartButton).Displayed; }
        catch (NoSuchElementException) { return false; }
    }
    public string GetProductPrice()
    {
        try { return _driver.FindElement(_productPrice).Text.Trim(); }
        catch (NoSuchElementException) { return string.Empty; }
    }
   
    public string GetProductName()
    {
        try { return _driver.FindElement(_productName).Text.Trim(); }
        catch (NoSuchElementException) { return string.Empty; }
    }
    public string GetModelNumber()
    {
        try
        {
            var row = _driver.FindElement(_modelRow);
            return row.Text.Replace("Model:", "").Trim();
        }
        catch (NoSuchElementException) { return string.Empty; }
    }

    // кликнуть Add to Cart, вернёт страницу корзины
    public CartPage ClickAddToCart()
    {
        _driver.FindElement(_addToCartButton).Click();
        return new CartPage(_driver);
    }

}
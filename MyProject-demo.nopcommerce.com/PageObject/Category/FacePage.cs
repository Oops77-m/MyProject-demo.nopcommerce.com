using OpenQA.Selenium;

namespace MyProject_demo.nopcommerce.com.PageObject.Category;

public class FacePage : BasePage
{
    public FacePage(IWebDriver driver) : base(driver) { }

    private readonly By _title = By.CssSelector(".page-title h1, h1");
    private readonly By _delicateBlushLink =
        By.XPath("//a[contains(@class,'prdocutname') and contains(@href, 'product_id=57')]");

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
    public bool IsDelicateBlushDisplayed()
    {
        try { return _driver.FindElement(_delicateBlushLink).Displayed; }
        catch (NoSuchElementException) { return false; }
    }
    public OpenProductCardPage ClickDelicateBlush()
    {
        _driver.FindElement(_delicateBlushLink).Click();
        return new OpenProductCardPage(_driver);
    }
}
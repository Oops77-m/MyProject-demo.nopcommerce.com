using OpenQA.Selenium;

namespace MyProject_demo.nopcommerce.com.PageObject;

public class BasePage
{
     
    protected IWebDriver _driver;
    public  BasePage(IWebDriver driver)
    {
        _driver = driver;
    }
    public void OpenAutomationteststore()
    {
        _driver.Navigate().GoToUrl("https://automationteststore.com/");
        _driver.Manage().Window.Maximize(); 
    }
    public string GetUrl()
    {
        return _driver.Url;
    }

    public string GetPageTitle()
    {
        return _driver.Title;
    }
}
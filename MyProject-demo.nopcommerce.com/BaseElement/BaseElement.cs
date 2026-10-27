using OpenQA.Selenium;

namespace MyProject_demo.nopcommerce.com.BaseElement;

public class BaseElement
{
    private readonly IWebDriver _driver;
    private readonly By _locator;

    public BaseElement(IWebDriver driver, By locator)
    {
        _driver = driver;
        _locator = locator;
    }

    private IWebElement Element => _driver.FindElement(_locator);

    public void Click() //нажать
    {
        Element.Click();
    }

    public void SendKeys(string text) //ввести текст
    {
        Element.SendKeys(text);
    }

    public string GetText() //получить текст
    {
        return Element.Text;
    }

    public bool IsDisplayed() //проверить отображение
    {
        return Element.Displayed;
    }
    public int Count() //посчитать элементы
    {
        return _driver.FindElements(_locator).Count;
    }
}
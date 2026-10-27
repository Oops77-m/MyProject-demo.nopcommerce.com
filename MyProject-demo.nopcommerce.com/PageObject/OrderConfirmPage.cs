using OpenQA.Selenium;

namespace MyProject_demo.nopcommerce.com.PageObject;

// Страница Checkout Confirmation (guest_step_3 / checkout/confirm)
// и после Confirm Order — страница успеха (checkout/success)
public class OrderConfirmPage : BasePage
{
    // сообщение об успешном заказе
    private readonly By _successMessage = By.XPath("//p[contains(., 'Your order has been successfully processed')]");
    // сообщение "Thank you"
    private readonly By _thankYouMessage = By.XPath("//p[contains(., 'Thank you')]");

    public OrderConfirmPage(IWebDriver driver) : base(driver)
    {
    }

    // открыта ли страница подтверждения заказа
    public bool IsConfirmPageOpened()
    {
        var url = _driver.Url;
        return url.Contains("guest_step_3") || url.Contains("checkout/confirm");
    }

    // подтвердить заказ (переход на rt=checkout/success)
    public OrderConfirmPage ConfirmOrder()
    {
        _driver.FindElement(By.Id("checkout_btn")).Click();
        // страница success грузится после подтверждения
        var url = _driver.Url;
        var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(_driver, TimeSpan.FromSeconds(15));
        wait.Until(d => d.Url.Contains("checkout/success"));
        return new OrderConfirmPage(_driver);
    }

    // оформлен ли заказ (страница success)
    public bool IsOrderPlaced()
    {
        return _driver.Url.Contains("rt=checkout/success");
    }

    // текст основного блока страницы (проверка подстановки данных аккаунта)
    public string GetPageText()
    {
        return _driver.FindElement(By.CssSelector(".contentpanel")).Text;
    }

    // видно ли сообщение о успешной обработке заказа
    public bool IsOrderSuccessMessageDisplayed()
    {
        try { return _driver.FindElement(_successMessage).Displayed; }
        catch (NoSuchElementException) { return false; }
    }

    // видно ли сообщение Thank you
    public bool IsThankYouMessageDisplayed()
    {
        try { return _driver.FindElement(_thankYouMessage).Displayed; }
        catch (NoSuchElementException) { return false; }
    }
}

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace MyProject_demo.nopcommerce.com.PageObject;

// Обёртка над страницами оформления заказа:
// 1) Checkout Options (rt=account/login, выбор Guest Checkout)
// 2) Гость: ввод адреса доставки (rt=checkout/guest_step_1)
public class CheckoutPage : BasePage
{
    public CheckoutPage(IWebDriver driver) : base(driver)
    {
    }

    // переключатель Guest Checkout на странице Checkout Options
    private readonly By _guestCheckoutRadio = By.Id("accountFrm_accountguest");
    // кнопка Continue на странице Checkout Options
    private readonly By _continueButton = By.CssSelector("button[title='Continue']");
    // === Поля формы гостевого заказа / доставки (те же имена, что в форме регистрации) ===
    private readonly By _firstName = By.CssSelector("input[name='firstname']");
    private readonly By _lastName = By.CssSelector("input[name='lastname']");
    private readonly By _email = By.CssSelector("input[name='email']");
    private readonly By _address1 = By.CssSelector("input[name='address_1']");
    private readonly By _city = By.CssSelector("input[name='city']");
    private readonly By _postcode = By.CssSelector("input[name='postcode']");
    private readonly By _zone = By.CssSelector("select[name='zone_id']");

    // открыта ли страница Checkout Options (выбор способа продолжения для нового гостя)
    public bool IsCheckoutOptionsPageOpened()
    {
        return _driver.Url.Contains("rt=account/login");
    }

    // выбрать Guest Checkout и перейти к вводу адреса (rt=checkout/guest_step_1)
    public CheckoutPage ContinueAsGuest()
    {
        _driver.FindElement(_guestCheckoutRadio).Click();
        // Continue находится в той же форме, что и переключатель
        _driver.FindElement(_guestCheckoutRadio)
            .FindElement(By.XPath("ancestor::form[1]//button[@title='Continue']"))
            .Click();
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
        wait.Until(d => d.Url.Contains("guest_step_1"));
        return new CheckoutPage(_driver);
    }

    // открыта ли страница ввода адреса доставки (гость идёт на guest_step_1)
    public bool IsShippingPageOpened()
    {
        var url = _driver.Url;
        return url.Contains("rt=checkout/shipping") || url.Contains("rt=checkout/guest_step_1");
    }

    // заполнить данные доставки (гостевой заказ)
    public void FillShippingDetails(string firstName, string lastName, string email,
        string address1, string city, string postcode)
    {
        _driver.FindElement(_firstName).SendKeys(firstName);
        _driver.FindElement(_lastName).SendKeys(lastName);
        _driver.FindElement(_email).SendKeys(email);
        _driver.FindElement(_address1).SendKeys(address1);
        _driver.FindElement(_city).SendKeys(city);
        _driver.FindElement(_postcode).SendKeys(postcode);
        SelectFirstZone();
    }

    // дождаться загрузки списка зон (подгружается JS-ом) и выбрать первую
    private void SelectFirstZone()
    {
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
        wait.Until(d => new SelectElement(d.FindElement(_zone)).Options.Count > 1);
        new SelectElement(_driver.FindElement(_zone)).SelectByIndex(1);
    }

    // значение поля First Name (проверка подстановки данных из аккаунта)
    public string GetFirstName()
    {
        return _driver.FindElement(_firstName).GetAttribute("value");
    }

    // перейти к подтверждению заказа: guest_step_1 → guest_step_3 (Checkout Confirmation)
    public OrderConfirmPage ContinueToConfirmation()
    {
        _driver.FindElement(_zone)
            .FindElement(By.XPath("ancestor::form[1]//button[@title='Continue']"))
            .Click();
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
        wait.Until(d => d.Url.Contains("guest_step_3") || d.Url.Contains("checkout/confirm"));
        return new OrderConfirmPage(_driver);
    }

    // открыта ли страница подтверждения заказа
    public bool IsConfirmationPageOpened()
    {
        var url = _driver.Url;
        return url.Contains("guest_step_3") || url.Contains("checkout/confirm");
    }

    // подтвердить заказ, когда мы уже на странице подтверждения (вошедший пользователь попадает сюда сразу)
    public OrderConfirmPage ConfirmOrder()
    {
        _driver.FindElement(By.Id("checkout_btn")).Click();
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
        wait.Until(d => d.Url.Contains("checkout/success"));
        return new OrderConfirmPage(_driver);
    }

    // текст основного блока страницы (проверка подстановки данных аккаунта)
    public string GetPageText()
    {
        return _driver.FindElement(By.CssSelector(".contentpanel")).Text;
    }
}

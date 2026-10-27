using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace MyProject_demo.nopcommerce.com.PageObject;

public class RegisterPage : BasePage
{
    public RegisterPage(IWebDriver driver) : base(driver)
    {
    }

    // === Личные данные ===
    private readonly By _firstName = By.Id("AccountFrm_firstname");
    private readonly By _lastName = By.Id("AccountFrm_lastname");
    private readonly By _email = By.Id("AccountFrm_email");

    // === Адрес ===
    private readonly By _address1 = By.Id("AccountFrm_address_1");
    private readonly By _city = By.Id("AccountFrm_city");
    private readonly By _postcode = By.Id("AccountFrm_postcode");
    private readonly By _zone = By.Id("AccountFrm_zone_id");

    // === Данные для входа ===
    private readonly By _loginName = By.Id("AccountFrm_loginname");
    private readonly By _password = By.Id("AccountFrm_password");
    private readonly By _passwordConfirm = By.Id("AccountFrm_confirm");

    // чекбокс согласия с Privacy Policy
    private readonly By _privacyPolicyCheckbox = By.Id("AccountFrm_agree");
    // кнопка Continue
    private readonly By _continueButton = By.CssSelector("button[title='Continue']");
    // success-сообщение (на странице success нет .alert-success, есть <p> с текстом)
    private readonly By _successMessage = By.XPath("//p[contains(., 'successfully created')]");

    // открыть страницу регистрации
    public void OpenRegisterPage()
    {
        _driver.Url = "https://automationteststore.com/index.php?rt=account/create";
        WaitUntilRegisterPageLoaded();
    }

    // дождаться загрузки страницы регистрации
    private void WaitUntilRegisterPageLoaded()
    {
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
        wait.Until(d => d.FindElements(_firstName).Count > 0);
    }

    // заполнить личные данные
    public void FillPersonalDetails(string firstName, string lastName, string email)
    {
        _driver.FindElement(_firstName).SendKeys(firstName);
        _driver.FindElement(_lastName).SendKeys(lastName);
        _driver.FindElement(_email).SendKeys(email);
    }

    // заполнить адрес (страна остаётся по умолчанию — United Kingdom)
    public void FillAddress(string address1, string city, string postcode)
    {
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

    // заполнить данные для входа (пароль и подтверждение — одинаковые)
    public void FillLoginDetails(string loginName, string password)
    {
        _driver.FindElement(_loginName).SendKeys(loginName);
        _driver.FindElement(_password).SendKeys(password);
        _driver.FindElement(_passwordConfirm).SendKeys(password);
    }

    // поставить галочку согласия с Privacy Policy
    public void CheckPrivacyPolicy()
    {
        var checkbox = _driver.FindElement(_privacyPolicyCheckbox);
        if (!checkbox.Selected) checkbox.Click();
    }

    // нажать Continue
    public void Submit()
    {
        _driver.FindElement(_continueButton).Click();
    }

    // открыта ли страница регистрации
    public bool IsRegisterPageOpened()
    {
        return _driver.Url.Contains("rt=account/create");
    }

    // видно ли success-сообщение
    public bool IsSuccessMessageDisplayed()
    {
        try { return _driver.FindElement(_successMessage).Displayed; }
        catch (NoSuchElementException) { return false; }
    }
}

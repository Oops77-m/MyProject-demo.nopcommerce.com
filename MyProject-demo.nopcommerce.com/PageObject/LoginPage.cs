using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace MyProject_demo.nopcommerce.com.PageObject;

public class LoginPage : BasePage
{
    public LoginPage(IWebDriver driver) : base(driver)
    {
    }

    // ссылка "Login or register" в шапке сайта
    private readonly By _loginOrRegisterLink = By.XPath("//a[contains(text(),'Login or register')]");
    // поле логина
    private readonly By _loginName = By.Id("loginFrm_loginname");
    // поле пароля
    private readonly By _password = By.Id("loginFrm_password");
    // кнопка Login
    private readonly By _loginButton = By.CssSelector("#loginFrm button[type='submit']");
    // кнопка Continue в блоке нового клиента (ведёт на регистрацию)
    private readonly By _newCustomerContinueButton = By.CssSelector("#accountFrm button[type='submit']");
    // сообщение об ошибке (неверный логин/пароль)
    private readonly By _errorMessage = By.CssSelector(".alert");

    // открыть страницу логина кликом по ссылке в шапке
    public void OpenLoginPage()
    {
        _driver.FindElement(_loginOrRegisterLink).Click();
        WaitUntilLoginPageLoaded();
    }

    // дождаться загрузки страницы логина
    private void WaitUntilLoginPageLoaded()
    {
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
        wait.Until(d => d.FindElements(_loginName).Count > 0);
    }

    // ввести логин и пароль и нажать Login
    public void Login(string loginName, string password)
    {
        _driver.FindElement(_loginName).SendKeys(loginName);
        _driver.FindElement(_password).SendKeys(password);
        _driver.FindElement(_loginButton).Click();
    }

    // перейти на страницу регистрации (блок нового клиента)
    public RegisterPage GoToRegisterPage()
    {
        _driver.FindElement(_newCustomerContinueButton).Click();
        return new RegisterPage(_driver);
    }

    // открыта ли страница логина (после Logoff сайт сначала показывает страницу account/logout)
    public bool IsLoginPageOpened()
    {
        var url = _driver.Url;
        return url.Contains("rt=account/login") || url.Contains("rt=account/logout");
    }

    // видно ли сообщение об ошибке
    public bool IsErrorMessageDisplayed()
    {
        try { return _driver.FindElement(_errorMessage).Displayed; }
        catch (NoSuchElementException) { return false; }
    }
}

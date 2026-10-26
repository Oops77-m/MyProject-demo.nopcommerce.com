using OpenQA.Selenium;

namespace MyProject_demo.nopcommerce.com.PageObject;

public class AccountPage : BasePage
{
    public AccountPage(IWebDriver driver) : base(driver)
    {
    }

    // ссылка Logout на странице аккаунта
    private readonly By _logoutLink = By.XPath("//a[contains(@href, 'account/logout')]");

    // открыта ли страница аккаунта (после входа/регистрации сайт ведёт на account/success)
    public bool IsAccountPageOpened()
    {
        var url = _driver.Url;
        return url.Contains("rt=account/success") || url.Contains("rt=account/account");
    }

    // кликнуть Logout: первые совпадения могут быть в скрытом выпадающем меню, нужна видимая ссылка
    public void ClickLogout()
    {
        foreach (var link in _driver.FindElements(_logoutLink))
        {
            if (link.Displayed)
            {
                link.Click();
                return;
            }
        }
        throw new NoSuchElementException("Видимая ссылка Logout не найдена на странице");
    }
}

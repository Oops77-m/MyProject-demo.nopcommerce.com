using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace MyProject_demo.nopcommerce.com.PageObject;

public class SearchPage : BasePage
{
    public SearchPage(IWebDriver driver) : base(driver)
    {
    }

    // поле поиска в шапке сайта
    private readonly By _searchInput = By.Id("filter_keyword");
    // иконка-лупа, клик по ней запускает поиск
    private readonly By _searchButton = By.CssSelector("div.button-in-search");
    // ссылки на товары в сетке результатов (класс prdocutname — опечатка сайта, оставляем как есть)
    private readonly By _resultProductLinks = By.CssSelector("div.thumbnails.grid a.prdocutname");
    // сообщение "товаров не найдено"
    private readonly By _noResultsMessage =
        By.XPath("//div[contains(text(),'There is no product that matches the search criteria.')]");

    // вводит текст в поле поиска в шапке и кликает по лупе
    public void SearchByKeyword(string keyword)
    {
        _driver.FindElement(_searchInput).SendKeys(keyword);
        _driver.FindElement(_searchButton).Click();
        WaitForResultsOrMessage();
    }

    // ждёт, пока появятся результаты поиска, ИЛИ сообщение "не найдено",
    // ИЛИ сайт сам редиректнул на карточку товара (точное совпадение имени)
    public void WaitForResultsOrMessage()
    {
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
        wait.Until(d =>
            d.FindElements(_resultProductLinks).Count > 0 ||
            d.FindElements(_noResultsMessage).Count > 0 ||
            d.Url.Contains("product_id="));
    }

    // проверяем, что открылась именно страница поиска (а не карточка товара)
    public bool IsSearchPageOpened()
    {
        return _driver.Url.Contains("rt=product/search");
    }

    // сколько товаров в результатах поиска
    public int GetSearchResultsCount()
    {
        return _driver.FindElements(_resultProductLinks).Count;
    }

    // список имён товаров из результатов поиска
    public List<string> GetSearchResultNames()
    {
        return _driver.FindElements(_resultProductLinks).Select(e => e.Text.Trim()).ToList();
    }

    // видно ли сообщение "товаров не найдено"
    public bool IsNoResultsMessageDisplayed()
    {
        try { return _driver.FindElement(_noResultsMessage).Displayed; }
        catch (NoSuchElementException) { return false; }
    }

    // кликает по результату с нужным именем и возвращает страницу карточки товара
    public OpenProductCardPage ClickResultByName(string name)
    {
        var link = _driver.FindElements(_resultProductLinks)
            .First(e => e.Text.Trim().Equals(name, StringComparison.OrdinalIgnoreCase));
        link.Click();
        return new OpenProductCardPage(_driver);
    }
}

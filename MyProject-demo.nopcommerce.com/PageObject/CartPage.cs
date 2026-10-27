using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace MyProject_demo.nopcommerce.com.PageObject;

public class CartPage : BasePage
{
    public CartPage(IWebDriver driver) : base(driver)
    {
    }

    // ссылки на товары в корзине (имена)
    private readonly By _productLinks = By.CssSelector("form#cart table a[href*='product_id']");
    // поле количества
    private readonly By _quantityInput = By.CssSelector("input[name^=\"quantity[\"]");
    // кнопка удаления (корзинка)
    private readonly By _removeButton = By.CssSelector("a[href*='checkout/cart&remove=']");
    // кнопка Update Cart
    private readonly By _updateCartButton = By.Id("cart_update");
    // итоговая сумма: колонка Total в последней строке таблицы товаров
    private readonly By _total = By.CssSelector("form#cart table tbody tr:last-child td:nth-last-child(2)");
    // сообщение "корзина пуста"
    private readonly By _emptyCartMessage = By.XPath("//div[contains(., 'Your shopping cart is empty')]");
    // кнопка Checkout на странице корзины
    private readonly By _checkoutButton = By.Id("cart_checkout1");

    // открыть страницу корзины
    public void OpenCartPage()
    {
        _driver.Url = "https://automationteststore.com/index.php?rt=checkout/cart";
    }

    // открыта ли страница корзины
    public bool IsCartPageOpened()
    {
        return _driver.Url.Contains("rt=checkout/cart");
    }

    // список имён товаров в корзине
    public List<string> GetProductNames()
    {
        return _driver.FindElements(_productLinks).Select(e => e.Text.Trim()).ToList();
    }

    // сколько товаров в корзине
    public int GetProductCount()
    {
        return _driver.FindElements(_productLinks).Count;
    }

    // есть ли товар в корзине
    public bool IsProductInCart(string name)
    {
        return GetProductNames().Contains(name);
    }

    // количество товара в корзине
    public int GetQuantity()
    {
        return int.Parse(_driver.FindElement(_quantityInput).GetAttribute("value"));
    }

    // изменить количество и нажать Update Cart
    public void SetQuantity(int quantity)
    {
        // нажимаем Update: страница перезагружается — ждём исчезновения старого элемента
        var oldInput = _driver.FindElement(_quantityInput);
        oldInput.Clear();
        oldInput.SendKeys(quantity.ToString());
        var updateButton = _driver.FindElement(_updateCartButton);
        updateButton.Click();
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
        wait.Until(_ => IsQuantityEqualTo(quantity));
    }

    // ожидание: значение количества на свежей странице равно нужному
    private bool IsQuantityEqualTo(int quantity)
    {
        try
        {
            var input = _driver.FindElement(_quantityInput);
            return !string.IsNullOrWhiteSpace(input.GetAttribute("value"))
                && int.Parse(input.GetAttribute("value")) == quantity;
        }
        catch (NoSuchElementException) { return false; }
    }

    // удалить товар из корзины
    public void RemoveProduct()
    {
        _driver.FindElement(_removeButton).Click();
    }

    // итоговая сумма в корзине
    public string GetTotal()
    {
        return _driver.FindElement(_total).Text;
    }

    // пуста ли корзина
    public bool IsCartEmpty()
    {
        try { return _driver.FindElement(_emptyCartMessage).Displayed; }
        catch (NoSuchElementException) { return false; }
    }

    // перейти к оформлению заказа
    public CheckoutPage ClickCheckout()
    {
        _driver.FindElement(_checkoutButton).Click();
        return new CheckoutPage(_driver);
    }
}

using OpenQA.Selenium;

namespace MyProject_demo.nopcommerce.com.PageObject;

public class PaymentPage : BasePage
{
    public PaymentPage(IWebDriver driver) : base(driver)
    {
    }

    // радио-кнопки способа оплаты
    private readonly By _paymentMethods = By.CssSelector("input[name='payment_method']");
    // кнопка Place Order (оформить заказ)
    private readonly By _placeOrderButton = By.CssSelector("button[title='Place Order']");

    // выбрать первый способ оплаты
    public void SelectFirstPaymentMethod()
    {
        _driver.FindElements(_paymentMethods)[0].Click();
    }

    // оформить заказ
    public OrderConfirmPage PlaceOrder()
    {
        _driver.FindElement(_placeOrderButton).Click();
        return new OrderConfirmPage(_driver);
    }

    // открыта ли страница оплаты
    public bool IsPaymentPageOpened()
    {
        return _driver.Url.Contains("rt=checkout/payment");
    }
}

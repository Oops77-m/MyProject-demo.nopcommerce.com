using System.Threading.Tasks;
using NUnit.Framework;
using RestSharp;

namespace MyProject_demo.nopcommerce.com.Tests.Api
{
    // Smoke-тесты сайта automationteststore.com как HTTP-сервиса:
    // страницы живы, коды ответов корректны, сервер не отдаёт 5xx.
    // Браузер не нужен — работают быстро и стабильно.
    public class SiteApiTests : BaseApiTest
    {
        protected override string BaseUrl => "https://automationteststore.com";

        // ключевые страницы сайта, которые обязаны отвечать
        private static readonly string[] Pages =
        {
            "/",                                                          // стартовая
            "index.php?rt=product/category&path=36",                     // категория Makeup
            "index.php?rt=product/category&path=36_38",                  // подкатегория Face
            "index.php?rt=search",                                       // страница поиска
            "index.php?rt=product/search&search=cream",                  // результаты поиска
            "index.php?rt=checkout/cart",                                // корзина
            "index.php?rt=account/login",                                // страница входа
            "index.php?rt=account/create"                                // страница регистрации
        };

        [Test, Order(1)]
        [Description("Сайт: стартовая страница отвечает 200 и HTML")]
        public async Task HomePage_Returns200_WithHtml()
        {
            var response = await client.GetAsync(new RestRequest("/"));

            Assert.Multiple(() =>
            {
                Assert.That((int)response.StatusCode, Is.EqualTo(200),
                    $"Стартовая страница недоступна: {response.ErrorMessage}");
                Assert.That(response.Content, Does.Contain("<html"));
                Assert.That(response.ContentType, Does.Contain("text/html"));
            });
        }

        [TestCaseSource(nameof(Pages))]
        [Description("Сайт: страница отвечает не ошибкой сервера ({0})")]
        public async Task Page_DoesNotReturnServerError(string path)
        {
            var response = await client.GetAsync(new RestRequest(path));

            Assert.That((int)response.StatusCode, Is.LessThan(500),
                $"Сервер отдал ошибку на {path}: {response.Content}");
        }

        [Test, Order(2)]
        [Description("Сайт: несуществующая страница не отдаёт 5xx")]
        public async Task UnknownPage_DoesNotReturnServerError()
        {
            var response = await client.GetAsync(new RestRequest("index.php?rt=nonexistent/page"));

            // сайт возвращает страницу с сообщением об ошибке, но не должен падать 5xx
            Assert.That((int)response.StatusCode, Is.LessThan(500),
                $"На несуществующей странице сервер отдал {(int)response.StatusCode}");
        }

        [Test, Order(3)]
        [Description("Сайт: поиск отвечает за разумное время (менее 20 секунд)")]
        public async Task SearchPage_RespondsQuickly()
        {
            var response = await client.GetAsync(new RestRequest("index.php?rt=product/search&search=cream"));

            Assert.Multiple(() =>
            {
                Assert.That((int)response.StatusCode, Is.LessThan(500));
                Assert.That(response.Content!, Does.Contain("cream").IgnoreCase);
            });
        }
    }
}

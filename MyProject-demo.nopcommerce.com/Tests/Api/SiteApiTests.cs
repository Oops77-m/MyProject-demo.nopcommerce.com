using System.Threading.Tasks;
using NUnit.Framework;
using RestSharp;

namespace MyProject_demo.nopcommerce.com.Tests.Api
{
    /// <summary>
    /// Smoke-тесты сайта automationteststore.com как HTTP-сервиса.
    ///
    /// Идея: проверить, что сервер живой и ключевые страницы отдают корректные ответы,
    /// БЕЗ запуска браузера. Такие тесты:
    /// - выполняются за секунды (в отличие от UI-тестов);
    /// - работают как «сигнализатор»: монитору ясно, что сломалось —
    ///   если эти тесты зелёные, а UI-тесты красные, значит проблема
    ///   в браузерной части или в самих тестах, а не на сервере сайта;
    /// - не зависят от вёрстки и селекторов.
    /// </summary>
    public class SiteApiTests : BaseApiTest
    {
        // тестируем реальный сайт, тот же, на который ходят UI-тесты
        protected override string BaseUrl => "https://automationteststore.com";

        /// <summary>
        /// Список страниц сайта, которые обязаны существовать и отвечать без ошибок сервера.
        /// Сюда входят все основные маршруты, которыми пользуются UI-тесты:
        /// "/" — главная, категории (path=36 Makeup, path=36_38 Face),
        /// поиск, корзина, вход и регистрация.
        /// Один и тот же набор прогоняется на каждой странице (TestCaseSource).
        /// </summary>
        private static readonly string[] Pages =
        {
            "/",                                          // стартовая
            "index.php?rt=product/category&path=36",      // категория Makeup
            "index.php?rt=product/category&path=36_38",   // подкатегория Face
            "index.php?rt=search",                        // страница поиска
            "index.php?rt=product/search&search=cream",   // результаты поиска
            "index.php?rt=checkout/cart",                 // корзина
            "index.php?rt=account/login",                 // страница входа
            "index.php?rt=account/create"                 // страница регистрации
        };

        /// <summary>
        /// Проверка №1: сам факт доступности сайта.
        /// Открываем главную страницу и проверяем сразу три вещи:
        /// - статус 200 (страница существует, сервер её отдаёт);
        /// - тело содержит тег "html" (отдаётся веб-страница, а не пустота/ошибка);
        /// - Content-Type - text/html (это обычная веб-страница).
        /// Assert.Multiple - если сломаться могли несколько проверок,
        /// покажутся сразу ВСЕ упавшие, а не только первая.
        /// </summary>
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

        /// <summary>
        /// Проверка №2: ни одна из ключевых страниц не «падает» на сервере.
        /// Для каждой страницы из списка Pages отдельно проверяем:
        /// статус-код строго МЕНЬШЕ 500. То есть страница может вернуть
        /// 200 (всё ок) или редирект/400 на порядок доработки, НО не должна
        /// отдавать внутренние ошибки 5xx - это признак сломанного сервера.
        /// TestCaseSource размножает этот один тест на все 8 страниц -
        /// в отчёте Allure будет по строке на каждую страницу.
        /// </summary>
        [TestCaseSource(nameof(Pages))]
        [Description("Сайт: страница отвечает не ошибкой сервера ({0})")]
        public async Task Page_DoesNotReturnServerError(string path)
        {
            var response = await client.GetAsync(new RestRequest(path));

            Assert.That((int)response.StatusCode, Is.LessThan(500),
                $"Сервер отдал ошибку на {path}: {response.Content}");
        }

        /// <summary>
        /// Проверка №3: поведение сайта на несуществующем маршруте.
        /// Мы специально запрашиваем несуществующий раздел (rt=nonexistent/page)
        /// и проверяем, что сервер НЕ отдаёт 5xx. Даже когда пользователь
        /// забрёл «не туда», сайт должен отдать аккуратную страницу-заглушку
        /// или 404, но не внутреннюю ошибку.
        /// </summary>
        [Test, Order(3)]
        [Description("Сайт: несуществующая страница не отдаёт 5xx")]
        public async Task UnknownPage_DoesNotReturnServerError()
        {
            var response = await client.GetAsync(new RestRequest("index.php?rt=nonexistent/page"));

            Assert.That((int)response.StatusCode, Is.LessThan(500),
                $"На несуществующем маршруте сервер отдал {(int)response.StatusCode}");
        }

        /// <summary>
        /// Проверка №4: поиск работает и отвечает быстро.
        /// Запрашиваем страницу результатов поиска по слову "cream" и проверяем:
        /// - статус < 500 (запрос обработан сервером корректно);
        /// - в HTML-ответе присутствует слово "cream" — сервер действительно
        ///   отдаёт страницу с результатами, а не пустую заглушку.
        /// Заодно эта проверка выступает «таймером»: если сервер зависнет,
        /// сработает 30-секундный Timeout из BaseApiTest и тест упадёт.
        /// </summary>
        [Test, Order(4)]
        [Description("Сайт: поиск отвечает и отдаёт результаты")]
        public async Task SearchPage_ReturnsResults()
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

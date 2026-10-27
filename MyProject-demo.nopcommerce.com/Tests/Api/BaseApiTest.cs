using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using RestSharp;

namespace MyProject_demo.nopcommerce.com.Tests.Api
{
    /// <summary>
    /// Базовый класс для всех API-тестов проекта.
    ///
    /// Что делает:
    /// 1. Перед каждым тестом ([SetUp]) создаёт HTTP-клиент RestSharp,
    ///    направленный на базовый адрес (BaseUrl), который задаёт наследник.
    /// 2. После каждого теста ([TearDown]) освобождает клиент
    ///    (закрывает соединения, чтобы не копились открытые сокеты).
    ///
    /// Отличие от UI-тестов (BaseTest):
    /// - не запускает браузер (ни локальный ChromeDriver, ни RemoteWebDriver);
    /// - работает напрямую с HTTP — быстрее и стабильнее;
    /// - поэтому для этих тестов не нужны контейнеры selenium-chrome и флаги headless.
    ///
    /// Атрибут [AllureEpic] группирует эти тесты в отчёте Allure
    /// в отдельную группу «API», чтобы не смешивать с UI-тестами.
    /// </summary>
    [AllureNUnit]
    [AllureEpic("Automation Test Store - API")]
    public abstract class BaseApiTest
    {
        /// <summary>HTTP-клиент, через который тесты отправляют запросы. Создаётся в [SetUp].</summary>
        protected RestClient client = null!;

        /// <summary>
        /// Базовый адрес тестируемого сервиса, например "https://automationteststore.com".
        /// Каждый наследник определяет свою точку входа.
        /// </summary>
        protected abstract string BaseUrl { get; }

        /// <summary>
        /// Заголовки, которые добавляются к КАЖДОМУ запросу (например, API-ключи).
        /// По умолчанию заголовков нет; наследник может переопределить.
        /// </summary>
        protected virtual (string name, string value)[]? DefaultHeaders => null;

        [SetUp]
        [AllureBefore("Создать HTTP-клиент")]
        public void ApiSetup()
        {
            client = CreateClient(BaseUrl, DefaultHeaders);
        }

        [TearDown]
        [AllureAfter("Освободить HTTP-клиент")]
        public void ApiTearDown()
        {
            client?.Dispose();
        }

        /// <summary>
        /// Собирает настроенный RestSharp-клиент:
        /// - ThrowOnAnyError = false — клиент НЕ бросает исключение на коды вроде 404/500,
        ///   вместо этого возвращает ответ как есть. Мы сами решаем в тесте,
        ///   какой код считать правильным (например, 404 это ВАЖНЫЙ и осознанный результат);
        /// - Timeout = 30 c — если сервер не ответил за полминуты, тест упадёт,
        ///   а не повиснет навсегда.
        /// </summary>
        protected static RestClient CreateClient(string baseUrl, (string name, string value)[]? defaultHeaders = null)
        {
            var options = new RestClientOptions(baseUrl)
            {
                ThrowOnAnyError = false,
                Timeout = TimeSpan.FromSeconds(30)
            };
            var rest = new RestClient(options);

            if (defaultHeaders != null)
                foreach (var (name, value) in defaultHeaders)
                    rest.AddDefaultHeader(name, value);

            return rest;
        }
    }
}

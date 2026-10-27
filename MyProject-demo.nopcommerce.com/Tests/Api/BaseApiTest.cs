using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using RestSharp;

namespace MyProject_demo.nopcommerce.com.Tests.Api
{
    // Общая база для API-тестов: создаёт настроенный RestSharp-клиент.
    // API-тесты не используют браузер (selenium не нужен).
    [AllureNUnit]
    [AllureEpic("Automation Test Store - API")]
    public abstract class BaseApiTest
    {
        protected RestClient client = null!;

        [SetUp]
        [AllureBefore("Создать HTTP-клиент")]
        public virtual void ApiSetup()
        {
            client = CreateClient(BaseUrl, DefaultHeaders);
        }

        [TearDown]
        [AllureAfter("Освободить HTTP-клиент")]
        public void ApiTearDown()
        {
            client?.Dispose();
        }

        // базовый адрес переопределяется в наследниках
        protected abstract string BaseUrl { get; }

        // заголовки по умолчанию (нужны не всем API)
        protected virtual (string name, string value)[]? DefaultHeaders => null;

        protected static RestClient CreateClient(string baseUrl, (string name, string value)[]? defaultHeaders = null)
        {
            var options = new RestClientOptions(baseUrl)
            {
                ThrowOnAnyError = false,        // сами проверяем статус-коды в тестах
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

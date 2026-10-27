using System.Threading.Tasks;
using NUnit.Framework;
using RestSharp;

namespace MyProject_demo.nopcommerce.com.Tests.Api
{
    // Тесты учебного публичного API reqres.in:
    // полноценные CRUD-сценарии и валидация кодов ответов.
    // С 2025 года reqres требует бесплатный ключ в заголовке x-api-key.
    public class ReqresApiTests : BaseApiTest
    {
        protected override string BaseUrl => "https://reqres.in";

        protected override (string name, string value)[] DefaultHeaders =>
            new[] { ("x-api-key", "reqres-free-v1") };

        // === GET ===

        [Test, Order(1)]
        [Description("GET /users/2: код 200, получен пользователь с id=2")]
        public async Task GetUser_Returns200_WithCorrectUser()
        {
            var response = await client.GetAsync(new RestRequest("api/users/2"));

            Assert.Multiple(() =>
            {
                Assert.That((int)response.StatusCode, Is.EqualTo(200));
                Assert.That(response.Content!, Does.Contain("\"id\":2"));
                Assert.That(response.Content!, Does.Contain("email"));
            });
        }

        [Test, Order(2)]
        [Description("GET /users?page=2: список содержит несколько пользователей")]
        public async Task GetUsersList_ReturnsMultipleUsers()
        {
            var response = await client.GetAsync(new RestRequest("api/users?page=2"));

            Assert.Multiple(() =>
            {
                Assert.That((int)response.StatusCode, Is.EqualTo(200));
                Assert.That(response.Content!, Does.Contain("\"per_page\""));
                // на странице 2 — id пользователей 7..12
                Assert.That(response.Content!, Does.Contain("\"id\":7"));
            });
        }

        [Test, Order(3)]
        [Description("GET несуществующего пользователя: код 404")]
        public async Task GetUnknownUser_Returns404()
        {
            var response = await client.GetAsync(new RestRequest("api/users/99999"));

            Assert.That((int)response.StatusCode, Is.EqualTo(404));
        }

        // === POST ===

        [Test, Order(4)]
        [Description("POST /users: создание возвращает 201 и поле id")]
        public async Task CreateUser_Returns201_WithId()
        {
            var request = new RestRequest("api/users", Method.Post)
                .AddJsonBody(new { name = "morpheus", job = "leader" });

            var response = await client.ExecuteAsync(request);

            Assert.Multiple(() =>
            {
                Assert.That((int)response.StatusCode, Is.EqualTo(201));
                Assert.That(response.Content!, Does.Contain("\"id\""));
                Assert.That(response.Content!, Does.Contain("createdAt"));
            });
        }

        [Test, Order(5)]
        [Description("POST /register: успешная регистрация возвращает 200 и токен")]
        public async Task RegisterUser_Returns200_WithToken()
        {
            var request = new RestRequest("api/register", Method.Post)
                .AddJsonBody(new { email = "eve.holt@reqres.in", password = "pistol" });

            var response = await client.ExecuteAsync(request);

            Assert.Multiple(() =>
            {
                Assert.That((int)response.StatusCode, Is.EqualTo(200));
                Assert.That(response.Content!, Does.Contain("token"));
            });
        }

        [Test, Order(6)]
        [Description("POST /register без пароля: код 400 с сообщением об ошибке")]
        public async Task RegisterWithoutPassword_Returns400_WithMessage()
        {
            var request = new RestRequest("api/register", Method.Post)
                .AddJsonBody(new { email = "sydney@fife" });

            var response = await client.ExecuteAsync(request);

            Assert.Multiple(() =>
            {
                Assert.That((int)response.StatusCode, Is.EqualTo(400));
                Assert.That(response.Content!, Does.Contain("error"));
            });
        }

        // === PUT ===

        [Test, Order(7)]
        [Description("PUT /users/2: обновление возвращает 200 и updatedAt")]
        public async Task UpdateUser_Returns200_WithUpdatedAt()
        {
            var request = new RestRequest("api/users/2", Method.Put)
                .AddJsonBody(new { name = "morpheus", job = "zion resident" });

            var response = await client.ExecuteAsync(request);

            Assert.Multiple(() =>
            {
                Assert.That((int)response.StatusCode, Is.EqualTo(200));
                Assert.That(response.Content!, Does.Contain("updatedAt"));
            });
        }

        // === DELETE ===

        [Test, Order(8)]
        [Description("DELETE /users/2: код 204 без тела ответа")]
        public async Task DeleteUser_Returns204_NoContent()
        {
            var response = await client.DeleteAsync(new RestRequest("api/users/2"));

            Assert.Multiple(() =>
            {
                Assert.That((int)response.StatusCode, Is.EqualTo(204));
                Assert.That(response.Content, Is.Null.Or.Empty);
            });
        }
    }
}

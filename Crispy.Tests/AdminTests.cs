using Crispy.Tests.Models;
using Crispy.Tests.Utility;
using System.Net;
using System.Text.Json;

namespace Crispy.Tests
{
    internal class AdminTests
    {
        private Data data;
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            data = new();
        }

        [Test]
        public async Task Get_Categories_Success()
        {
            var response = await data.GetClient().GetAsync($"Admin/Category");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
        [Test]
        public async Task Post_CreateCategory_CategoryCreated()
        {
            Random rand = new();
            var categoryName = data.GenerateRandomString();
            var initialCount = data.GetCategoriesCount();
            var displayOrder = rand.Next(99);

            var formData = new Dictionary<string, string>
            {
                { "displayOrder", displayOrder.ToString() },
                { "name",  categoryName },
            };

            var formContent = new FormUrlEncodedContent(formData);

            var response = await data.GetClient().PostAsync($"Admin/Category/Create", formContent);
            var content = await response.Content.ReadAsStringAsync();
            var category = JsonSerializer.Deserialize<CategorySnake>(content);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(data.GetCategoriesCount(), Is.EqualTo(initialCount + 1));
                Assert.That(category.DisplayOrder, Is.EqualTo(displayOrder));
                Assert.That(category.Name, Is.EqualTo(categoryName));
            });
        }

        [Test]
        public async Task Get_Companies_Success()
        {
            var response = await data.GetClient().GetAsync($"Admin/Company");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task Post_CreateCompany_CompanyCreated()
        {
            var companyName = data.GenerateRandomString();
            var initialCount = data.GetCompaniesCount();

            var formData = new Dictionary<string, string>
            {
                { "name",  companyName },
                { "city",  data.GenerateRandomString() },
                { "postalCode",  data.GenerateRandomString() },
                { "streetAdress",  data.GenerateRandomString() },
                { "state",  data.GenerateRandomString() },
                { "phoneNumber",  data.GenerateRandomString() },
            };

            var formContent = new FormUrlEncodedContent(formData);

            var response = await data.GetClient().PostAsync($"Admin/Company/Upsert", formContent);
            var content = await response.Content.ReadAsStringAsync();
            var company = JsonSerializer.Deserialize<CompanySnake>(content);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(data.GetCompaniesCount(), Is.EqualTo(initialCount + 1));
                Assert.That(company.Name, Is.EqualTo(companyName));
            });
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            data.Dispose();
        }
    }
}

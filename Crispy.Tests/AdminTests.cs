using Crispy.Models;
using Crispy.Tests.Models;
using Crispy.Tests.Utility;
using Crispy.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
            var initialCount = data.GetCategoriesCount();
            var categoryName = "AbraKadabra";

            var formData = new Dictionary<string, string>
            {
                { "name",  categoryName }
            };

            var formContent = new FormUrlEncodedContent(formData);

            var response = await data.GetClient().PostAsync($"Admin/Category/Create", formContent);
            var content = await response.Content.ReadAsStringAsync();

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(data.GetCategoriesCount(), Is.EqualTo(initialCount + 1));
            });
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            data.Dispose();
        }
    }
}

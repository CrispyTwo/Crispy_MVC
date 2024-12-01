using Crispy.Models;
using Crispy.Tests.Models;
using Crispy.Tests.Utility;
using System.Net;
using System.Text.Json;

namespace Crispy.Tests
{
    [TestFixture]
    public class ShoppingCartTests
    {
        private Data data;
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            data = new();
        }

        [Test]
        public async Task Get_ProductDetails_IdCorrect()
        {
            await data.CreateShoppingCart(data.GetDefaultProductId());

            var response = await data.GetClient().GetAsync($"Customer/Home/Details/{data.GetDefaultProductId()}");
            var content = await response.Content.ReadAsStringAsync();
            var cart = JsonSerializer.Deserialize<ShoppingCartSnake>(content);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(cart.ProductId, Is.EqualTo(data.GetDefaultProductId()));
            });

            response = await data.GetClient().GetAsync("/Customer/Cart");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task Post_ProductDetails_RecordAdded()
        {
            await data.CreateShoppingCart(data.GetDefaultProductId(), true, 5);
        }

        [Test]
        public async Task Post_PlusCount_CountAdded()
        {
            var cartId = await data.CreateShoppingCart(data.GetDefaultProductId());
            var initialCount = data.GetShoppingCartCount(data.GetDefaultProductId());

            var formData = new Dictionary<string, string>
            {
                { "cartId", cartId.ToString() }
            };

            var formContent = new FormUrlEncodedContent(formData);
            var response = await data.GetClient().PostAsync($"Customer/Cart/Plus", formContent);

            var newCount = data.GetShoppingCartCount(data.GetDefaultProductId(), true);
            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(newCount, Is.EqualTo(initialCount + 1));
            });
        }

        [Test]
        public async Task Post_MinusCount_CountDeducted()
        {
            var cartId = await data.CreateShoppingCart(data.GetDefaultProductId());
            var initialCount = data.GetShoppingCartCount(data.GetDefaultProductId());

            var formData = new Dictionary<string, string>
            {
                { "cartId", cartId.ToString() }
            };
            var formContent = new FormUrlEncodedContent(formData);
            var response = await data.GetClient().PostAsync($"Customer/Cart/Minus", formContent);

            var newCount = data.GetShoppingCartCount(data.GetDefaultProductId(), true);
            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(newCount, Is.EqualTo(initialCount - 1));
            });
        }

        [Test]
        public async Task Post_DeleteProduct_ProductDeleted()
        {
            var cartId = await data.CreateShoppingCart(data.GetDefaultProductId());

            var formData = new Dictionary<string, string>
            {
                { "cartId", cartId.ToString() }
            };
            var formContent = new FormUrlEncodedContent(formData);
            var response = await data.GetClient().PostAsync($"Customer/Cart/Remove", formContent);

            var newCount = data.GetShoppingCartCount(data.GetDefaultProductId());
            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(newCount, Is.EqualTo(0));
            });
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            data.Dispose();
        }
    }
}

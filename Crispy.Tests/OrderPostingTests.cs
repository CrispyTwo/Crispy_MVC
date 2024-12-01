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
    [TestFixture]
    internal class OrderPostingTests
    {
        private Data data;
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            data = new();
        }

        [Test]
        public async Task Get_OrderSummary_CorrectData()
        {
            var cartId = await data.CreateShoppingCart(data.GetDefaultProductId());
            var count = data.GetShoppingCartCount(data.GetDefaultProductId());

            var response = await data.GetClient().GetAsync($"Customer/Cart/Summary");
            var content = await response.Content.ReadAsStringAsync();
            var cartVM = JsonSerializer.Deserialize<ShoppingCartVMSnake>(content);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(cartVM.ShoppingCartList.Last().ProductId, Is.EqualTo(data.GetDefaultProductId()));
                Assert.That(cartVM.ShoppingCartList.Last().Count, Is.EqualTo(count));
                Assert.That(cartVM.OrderHeader.OrderStatus, Is.EqualTo(null));
                Assert.That(cartVM.OrderHeader.PaymentStatus, Is.EqualTo(null));
            });
        }

        [Test]
        public async Task Post_OrderSummary_OrderPosted()
        {
            var cartId = await data.CreateShoppingCart(data.GetDefaultProductId());
            var count = data.GetShoppingCartCount(data.GetDefaultProductId());

            var response = await data.GetClient().GetAsync($"Customer/Cart/Summary");
            var content = await response.Content.ReadAsStringAsync();
            var cartVM = JsonSerializer.Deserialize<ShoppingCartVMSnake>(content);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(cartVM.ShoppingCartList.Last().ProductId, Is.EqualTo(data.GetDefaultProductId()));
                Assert.That(cartVM.ShoppingCartList.Last().Count, Is.EqualTo(count));
                Assert.That(cartVM.OrderHeader.OrderStatus, Is.EqualTo(null));
                Assert.That(cartVM.OrderHeader.PaymentStatus, Is.EqualTo(null));
            });

            var flattenJson = data.FlattenJson(content, string.Empty, "applicationUser");
            var formContent = new FormUrlEncodedContent(flattenJson);
            
            response = await data.GetClient().PostAsync($"Customer/Cart/Summary", formContent);
            var order = await data.GetOrderHeader();

            Assert.Multiple(() =>
            {
                Assert.That(order.OrderStatus, Is.EqualTo(SD.StatusPending));
                Assert.That(order.PaymentStatus, Is.EqualTo(SD.PaymentStatusPending));
            });
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            data.Dispose();
        }
    }
}

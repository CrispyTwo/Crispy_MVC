using Crispy.DataAccess.Data;
using Crispy.Models;
using Crispy.Tests.Models;
using Crispy.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Crispy.Tests.Utility
{
    internal class Data
    {
        private readonly IServiceScope _serviceScope;
        private readonly WebApplicationFactory<Program> _factory;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDBContext _dbContext;
        private HttpClient _client;

        private ApplicationUser User;
        private ApplicationUser Admin;
        public Data()
        {
            _factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(ConfigureTestServices);
                });

            _serviceScope = _factory.Services.CreateScope();
            var scopedServices = _serviceScope.ServiceProvider;

            _dbContext = scopedServices.GetRequiredService<ApplicationDBContext>();
            _userManager = scopedServices.GetRequiredService<UserManager<IdentityUser>>();

            GetClient();
        }

        internal int GetDefaultProductId()
        {
            return 1;
        }
        internal string GetDefaultEmail()
        {
            return "Admin@gmail.com";
        }
        internal int GetShoppingCartCount(int productId, bool reload = false)
        {
            var cart = _dbContext.ShoppingCart.FirstOrDefault(x => x.ApplicationUser.Email == GetDefaultEmail() && x.ProductId == productId);
            if (reload)
                _dbContext.ShoppingCart.Entry(cart).Reload();
            return cart == null ? 0 : cart.Count;
        }
        internal int GetShoppingCartId(int productId, bool reload = false)
        {
            var cart = _dbContext.ShoppingCart.FirstOrDefault(x => x.ApplicationUser.Email == GetDefaultEmail() && x.ProductId == productId);
            if (reload)
                _dbContext.ShoppingCart.Entry(cart).Reload();
            return cart == null ? 0 : cart.Id;
        }
        internal int GetCategoriesCount()
        {
            return _dbContext.Category.ToArrayAsync().GetAwaiter().GetResult().Length;
        }
        internal async Task<int> CreateShoppingCart(int productId, bool forceCreate = false, int newCount = 3)
        {
            var count = GetShoppingCartCount(productId);

            if (forceCreate || count < 1)
            {
                var cart = new ShoppingCart()
                {
                    ProductId = productId,
                    Count = newCount
                };

                var formData = new Dictionary<string, string>
                {
                    { "ProductId", cart.ProductId.ToString() },
                    { "Count", cart.Count.ToString() }
                };

                var formContent = new FormUrlEncodedContent(formData);

                var response = await GetClient().PostAsync($"Customer/Home/Details", formContent);
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ShoppingCartSnake>(content);

                Assert.Multiple(() =>
                {
                    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                    Assert.That(result.ProductId, Is.EqualTo(productId));
                    Assert.That(result.Count, Is.EqualTo(newCount));
                });
            }

            return GetShoppingCartId(productId);
        }
        internal async Task<OrderHeader> GetOrderHeader(bool reload = false)
        {
            var orderHeader = _dbContext.OrderHeader
                            .OrderByDescending(x => x.OrderDate)
                            .FirstOrDefault();

            if (reload)
                _dbContext.OrderHeader.Entry(orderHeader).Reload();
            return orderHeader;
        }
        async internal Task CreateAdmin()
        {
            string email = "testAdmin@example.com", password = "TestPassword123!";

            if (Admin == null)
            {
                Admin = new ApplicationUser { UserName = "Adam", Email = email, EmailConfirmed = true };
                var createUserTask = await _userManager.CreateAsync(Admin, password);

                Assert.That(createUserTask.Succeeded, Is.True);

                Admin = _dbContext.User.FirstOrDefault(x => x.Email == email);
                var assignRoleTask = await _userManager.AddToRoleAsync(Admin, SD.RoleAdmin);

                Assert.That(assignRoleTask.Succeeded, Is.True);
            }
        }
        async internal Task CreateUser()
        {
            string email = "testUser@example.com", password = "TestPassword123!";

            if (User == null)
            {
                User = new ApplicationUser { UserName = "Mark", Email = email, EmailConfirmed = true };
                var createUserTask = await _userManager.CreateAsync(User, password);

                Assert.That(createUserTask.Succeeded, Is.True);

                User = _dbContext.User.FirstOrDefault(x => x.Email == email);
                var assignRoleTask = await _userManager.AddToRoleAsync(User, SD.RoleCustomer);

                Assert.That(assignRoleTask.Succeeded, Is.True);
            }
        }
        public Dictionary<string, string> FlattenJson(string json, string prefix = "", string excludeKey = "")
        {
            var dict = new Dictionary<string, string>();
            var token = JToken.Parse(json);

            void ProcessToken(JToken token, string currentPrefix)
            {
                if (token is JObject obj)
                {
                    foreach (var prop in obj.Properties())
                    {
                        var newPrefix = string.IsNullOrEmpty(currentPrefix) ? prop.Name : $"{currentPrefix}.{prop.Name}";

                        if (prop.Name.StartsWith(excludeKey))
                        {
                            continue;
                        }

                        ProcessToken(prop.Value, newPrefix);
                    }
                }
                else if (token is JArray array)
                {
                    for (int i = 0; i < array.Count; i++)
                    {
                        var newPrefix = $"{currentPrefix}[{i}]";
                        ProcessToken(array[i], newPrefix);
                    }
                }
                else // Handle primitive values
                {
                    dict[currentPrefix] = token.ToString();
                }
            }

            ProcessToken(token, prefix);
            return dict;
        }
        public void ConfigureTestServices(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Fake";
                options.DefaultChallengeScheme = "Fake";
                options.DefaultScheme = "Fake";
            })
            .AddScheme<AuthenticationSchemeOptions, FakeAuthenticationHandler>("Fake", options => { });

        }
        internal HttpClient GetClient()
        {
            if (_client == null)
            {
                _client = _factory.CreateClient();
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Fake");
            }

            return _client;
        }
        async internal void Dispose()
        {
            _client?.Dispose();
            _factory?.Dispose();
            _serviceScope.Dispose();
        }
    }
}

//async internal Task LogInUser()
//{
//    string email = "testUser@example.com", password = "TestPassword123!";

//    if (User == null)
//    {
//        User = new ApplicationUser { UserName = "Adam", Email = email, EmailConfirmed = true };
//        await _userManager.CreateAsync(User, password);

//        ApplicationUser user = _dbContext.User.FirstOrDefault(x => x.Email == email);
//        await _userManager.AddToRoleAsync(user, SD.RoleCustomer);
//    }

//    var get = await _client.GetAsync("/Identity/Account/Login");
//    var cookies = get.Headers.GetValues("Set-Cookie");
//    var csrfToken = cookies.FirstOrDefault(c => c.Contains("AspNetCore.Antiforgery"));

//    var formData = new FormUrlEncodedContent(
//    [
//        new KeyValuePair<string, string>("Input.Email", email),
//                new KeyValuePair<string, string>("Input.Password", password),
//                new KeyValuePair<string, string>("Input.RememberMe", "false"),
//                new KeyValuePair<string, string>("__RequestVerificationToken", csrfToken)
//    ]);
//    _client.DefaultRequestHeaders.Add("Referer", _client.BaseAddress + "/Identity/Account/Login");
//    _client.DefaultRequestHeaders.Add("RequestVerificationToken", csrfToken);

//    var response = await _client.PostAsync("/Identity/Account/Login", formData);

//    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
//    var content = await response.Content.ReadAsStringAsync();
//    Assert.That(content, Does.Not.Contain("Invalid login attempt."));
//}

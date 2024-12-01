using Crispy.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Crispy.Tests.Models
{
    internal class ShoppingCartSnake
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("applicationUserId")]
        public string ApplicationUserId { get; set; }
        [JsonPropertyName("applicationUser")]
        public ApplicationUser ApplicationUser { get; set; }
        [JsonPropertyName("productId")]
        public int ProductId { get; set; }
        [JsonPropertyName("product")]
        public Product Product { get; set; }
        [JsonPropertyName("count")]
        public int Count { get; set; }
        [JsonPropertyName("price")]
        public double Price { get; set; }
    }
}

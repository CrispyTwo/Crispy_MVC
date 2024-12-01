using Crispy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Crispy.Tests.Models
{
    internal class ShoppingCartVMSnake
    {
        [JsonPropertyName("shoppingCartList")]
        public IEnumerable<ShoppingCartSnake> ShoppingCartList { get; set; }
        [JsonPropertyName("orderHeader")]
        public OrderHeaderSnake OrderHeader { get; set; }
    }
}

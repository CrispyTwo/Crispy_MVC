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
    internal class OrderHeaderSnake
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("applicationUserId")]
        public string ApplicationUserId { get; set; }

        [ForeignKey(nameof(ApplicationUserId))]
        [ValidateNever]
        [JsonPropertyName("applicationUser")]
        public ApplicationUser ApplicationUser { get; set; }

        [JsonPropertyName("order_date")]
        public DateTime OrderDate { get; set; }

        [JsonPropertyName("shippingDate")]
        public DateTime ShippingDate { get; set; }

        [JsonPropertyName("orderTotal")]
        public double OrderTotal { get; set; }

        [JsonPropertyName("orderStatus")]
        public string? OrderStatus { get; set; }

        [JsonPropertyName("paymentStatus")]
        public string? PaymentStatus { get; set; }

        [JsonPropertyName("trackingNumber")]
        public string? TrackingNumber { get; set; }

        [JsonPropertyName("carrier")]
        public string? Carrier { get; set; }

        [JsonPropertyName("paymentDate")]
        public DateTime PaymentDate { get; set; }

        [JsonPropertyName("paymentDueDate")]
        public DateTime PaymentDueDate { get; set; }

        [JsonPropertyName("sessionId")]
        public string? SessionId { get; set; }

        [JsonPropertyName("paymentIntendId")]
        public string? PaymentIntendId { get; set; }

        [JsonPropertyName("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonPropertyName("street")]
        public string Street { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("region")]
        public string Region { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}

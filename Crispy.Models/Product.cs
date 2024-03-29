﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Crispy.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        [Display(Name = "List Price")]
        [Range(0, 1000)]
        public int ListPrice { get; set; }
        [Required]
        [Display(Name = "Price 1-10")]
        [Range(0, 1000)]
        public int Price { get; set; }
        [Required]
        [Display(Name = "Price 10-50")]
        [Range(0, 1000)]
        public int Price10 { get; set; }
        [Required]
        [Display(Name = "Price 50+")]
        [Range(0, 1000)]
        public int Price50 { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }
        [ValidateNever]
        public string ImageURL { get; set; }

    }
}

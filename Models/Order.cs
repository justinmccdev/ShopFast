using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopFast.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        [Column("OrderTotal")]
        public decimal OrderTotal { get; set; }

        [Column("Status")]
        public string Status { get; set; }
        //public decimal OrderTotal { get; set; }
        //public string Status { get; set; }
        //public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string StreetAddress { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string Zip { get; set; }

        [Required]
        public string FakeCreditCardNumber { get; set; }    
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ShopFast.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Status { get; set; }

        public IdentityUser User { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
    }
}

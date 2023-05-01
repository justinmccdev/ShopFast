using Microsoft.EntityFrameworkCore;
using ShopFast.Data;
using ShopFast.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ShopFast.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;

        public CartService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ShoppingCart> GetCartAsync(string userId)
        {
            var cart = await _context.ShoppingCarts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId && c.Status == "Active");

            if (cart == null)
            {
                cart = new ShoppingCart
                {
                    UserId = userId,
                    Status = "Active",
                    CartItems = new List<CartItem>() // Add this line to initialize the CartItems property
                };

                _context.ShoppingCarts.Add(cart);
                await _context.SaveChangesAsync();
            }


            return cart;
        }

        public async Task AddToCartAsync(ShoppingCart cart, Product product, int quantity)
        {
            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == product.Id);

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    ShoppingCartId = cart.Id,
                    ProductId = product.Id,
                    Quantity = quantity
                };

                _context.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemoveFromCartAsync(CartItem cartItem)
        {
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task ClearCartAsync(ShoppingCart cart)
        {
            _context.CartItems.RemoveRange(cart.CartItems);
            await _context.SaveChangesAsync();
        }
        public async Task<decimal> GetTotalAsync(ShoppingCart cart)
        {
            return cart.CartItems.Sum(ci => ci.Product.Price * ci.Quantity);
        }

    }
}

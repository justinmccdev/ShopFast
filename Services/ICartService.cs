// Create a new file called ICartService.cs in the Services folder

using ShopFast.Models;
using System.Threading.Tasks;

namespace ShopFast.Services
{
    public interface ICartService
    {
        Task<ShoppingCart> GetCartAsync(string userId);
        Task AddToCartAsync(ShoppingCart cart, Product product, int quantity);
        Task RemoveFromCartAsync(CartItem cartItem);
        Task ClearCartAsync(ShoppingCart cart);
        Task<decimal> GetTotalAsync(ShoppingCart cart);

    }
}

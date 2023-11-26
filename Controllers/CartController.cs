using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopFast.Data;
using ShopFast.Models;
using ShopFast.Services;
using System.Threading.Tasks;

namespace ShopFast.Controllers
{
    //[Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        //private readonly CartService _cartService;

        private readonly ICartService _cartService;

        public CartController(ApplicationDbContext context, UserManager<IdentityUser> userManager, ICartService cartService)
        {
            _context = context;
            _userManager = userManager;
            _cartService = cartService;
        }


        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            if (userId == null)
            {
                TempData["ErrorMessage"] = "Please login in order to check your cart.";
                return RedirectToAction(nameof(Index), "Home"); // Redirect to the homepage
            }
            var cart = await _cartService.GetCartAsync(userId);
            var total = await _cartService.GetTotalAsync(cart);

            ViewData["Total"] = total;
            return View(cart);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Checkout()
        {
            var userId = _userManager.GetUserId(User);
            var order = new Order
            {
                UserId = userId, // Default value for UserID
                OrderTotal = 0, // Default value for OrderTotal
                Status = "Pending" // Default value for Status
            };
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(Order order)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var cart = await _cartService.GetCartAsync(userId);
                var orderTotal = cart.CartItems.Sum(ci => ci.Product.Price * ci.Quantity);

                order.UserId = userId; // Add this line to set the UserId property
                order.OrderTotal = orderTotal;
                order.OrderDate = DateTime.Now;
                order.Status = "Completed";

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                await _cartService.ClearCartAsync(cart);
                await _context.SaveChangesAsync();

                return RedirectToAction("Confirmation", new { orderId = order.Id });
            }

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            if (!User.Identity.IsAuthenticated)
            {
                // Return a view for guests with a message to log in or register
                return View("GuestAddToCart");
            }

            var userId = _userManager.GetUserId(User);
            var product = await _context.Products.FindAsync(productId);

            if (product == null)
            {
                return NotFound();
            }

            var cart = await _cartService.GetCartAsync(userId);
            await _cartService.AddToCartAsync(cart, product, quantity);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int cartItemId)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);

            if (cartItem == null)
            {
                return NotFound();
            }

            await _cartService.RemoveFromCartAsync(cartItem);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ClearCart()
        {
            var userId = _userManager.GetUserId(User);
            var cart = await _cartService.GetCartAsync(userId);
            await _cartService.ClearCartAsync(cart);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Confirmation(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }


    }
}

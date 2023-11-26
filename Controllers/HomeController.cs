using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShopFast.Data;
using ShopFast.Models;
using ShopFast.ViewModels;
using System.Diagnostics;
using System.Linq;

namespace ShopFast.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(UserManager<IdentityUser> userManager, ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _userManager = userManager;
            _logger = logger;
            _context = context;
        }


        public async Task<IActionResult> Index(string query)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                ViewData["Username"] = user.UserName;
            }
            else
            {
                ViewData["Username"] = "No user is logged in.";
            }

            var products = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(query))
            {
                products = products.Where(p => EF.Functions.Like(p.Name, $"%{query}%"));
            }

            var featuredProducts = products
                                    .OrderBy(p => p.Name)
                                    .Take(10)
                                    .ToList();

            var viewModel = new HomePageViewModel
            {
                FeaturedProducts = featuredProducts
            };

            ViewData["ShowProductGrid"] = true;
            ViewBag.Products = featuredProducts;
            return View(viewModel);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

using Microsoft.AspNetCore.Identity;

namespace ShopFast.Middleware
{
    public class GuestLoginMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GuestLoginMiddleware> _logger;

        public GuestLoginMiddleware(RequestDelegate next, ILogger<GuestLoginMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            // Check if user is authenticated
            if (!context.User.Identity.IsAuthenticated)
            {
                _logger.LogInformation("User is not authenticated, logging in guest user...");

                // Log in the guest user
                var guestUser = await userManager.FindByNameAsync("Guest");
                if (guestUser != null)
                {
                    _logger.LogInformation("Guest user found, signing in...");
                    await signInManager.SignInAsync(guestUser, isPersistent: false);
                    _logger.LogInformation("Guest user signed in successfully");
                }
                else
                {
                    _logger.LogWarning("Guest user not found");
                }
            }
            else
            {
                _logger.LogInformation("User is already authenticated");
            }

            // Call the next middleware in the pipeline
            await _next(context);
        }
    }


}

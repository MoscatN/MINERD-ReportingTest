using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SalesReporter.Models;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace SalesReporter.Controllers {
    public class HomeController : Controller {

        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private StoredbContext _context;

        public HomeController(ILogger<HomeController> logger, StoredbContext _context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager) {
            _logger = logger;
            this._context = _context;
            _signInManager = signInManager;
            this._userManager = userManager;
        }

        public IActionResult Index() {
            return RedirectToAction("Register");
        }


        public IActionResult Register() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(User user) 
        {
            if (ModelState.IsValid) 
            {

                var userIden = new IdentityUser 
                {
                    UserName = user.UserName,
                    Email = user.Email,
                };

                var res = await _userManager.CreateAsync(userIden, user.Password);

                if(res.Succeeded) 
                {
                    await _signInManager.SignInAsync(userIden, isPersistent: false);
                    //await _context.Users.AddAsync(user);
                    //await _context.SaveChangesAsync();

                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in res.Errors) 
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Login() 
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string userName, string password)     
        {
            ArgumentNullException.ThrowIfNull(password);

            var res = await _signInManager.PasswordSignInAsync(userName, password, false, false);

            if(res.Succeeded) 
            {
                var user = await _signInManager.UserManager.FindByNameAsync(userName);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                };

                var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                var authProperties = new AuthenticationProperties {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),
                };

                await HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(claimsIdentity), authProperties);

                return RedirectToAction("Products", "Products");   
            }

            ModelState.AddModelError(string.Empty, "Username or password incorrect.");

            return View();
        }

        public async Task<IActionResult> Logout() 
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
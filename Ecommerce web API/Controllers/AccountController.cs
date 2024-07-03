using Ecommerce_web_API.Models;
using Ecommerce_web_API.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Ecommerce_web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ApplicationDbContext context;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager , ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
            this.roleManager = roleManager;
        }

        [HttpPost]
       // [ValidateAntiForgeryToken]
        [Route("register")]
        public async Task<IActionResult> Register (RegisterViewModel newUser)
        {
            if (newUser.Role == "Seller" || newUser.Role == "Buyer")
            {
                if (ModelState.IsValid)
            {
                
                ApplicationUser user = new ApplicationUser();
                user.UserName = newUser.UserName;
                user.PasswordHash = newUser.Password;
                user.Email = newUser.Email;
                user.Address = newUser.Address;


                
                    var result = await userManager.CreateAsync(user, newUser.Password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, newUser.Role);
                        await signInManager.SignInAsync(user, false);
                        

                    }
                    else
                    {
                        foreach (var errorItem in result.Errors)
                        {
                            ModelState.AddModelError("Password", errorItem.Description);
                            return BadRequest();
                        }

                    } 

                return Ok(newUser);
             }
               
            }
            return BadRequest();
            
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginViewModel userVM)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser userModel = await userManager.FindByNameAsync(userVM.UserName);
                if (userModel != null)
                {
                    bool found = await userManager.CheckPasswordAsync(userModel, userVM.Password);
                    if (found)
                    {
                        await signInManager.SignInAsync(userModel, userVM.RememperMe);
                        RedirectToAction("Register");
                        return Ok(userVM);
                    }
                }


                ModelState.AddModelError("", "User Name or Password Wrong");



            }
            return NotFound(userVM);
        }

        [HttpPost]
        [Route("Logout")]
        public IActionResult Logout()
        {
            signInManager.SignOutAsync();
            return Ok();
        }

        



            /*
            private readonly UserManager<ApplicationUser> userManager;
            private readonly SignInManager<ApplicationUser> signInManager;

            public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
            {
                this.userManager = userManager;
                this.signInManager = signInManager;
            }

            [HttpGet]
            public IActionResult Register()
            {
                return View();
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Register(RegisterUserViewModel newUserVM)
            {
                if (ModelState.IsValid)
                {
                    ApplicationUser user = new ApplicationUser();
                    user.UserName = newUserVM.UserName;
                    user.Address = newUserVM.Address;
                    user.PasswordHash = newUserVM.Password;

                    var result = await userManager.CreateAsync(user, newUserVM.Password);
                    if (result.Succeeded)
                    {
                        await signInManager.SignInAsync(user, false);
                        return RedirectToAction("Index", "Employee");
                    }
                    else
                    {
                        foreach (var errorItem in result.Errors)
                        {
                            ModelState.AddModelError("Password", errorItem.Description);
                        }

                    }
                }
                return View(newUserVM);
            }
            [HttpGet]
            public IActionResult Login()
            {
                return View();
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Login(LoginUserViewModel userVM)
            {
                if (ModelState.IsValid)
                {
                    ApplicationUser userModel = await userManager.FindByNameAsync(userVM.UserName);
                    if (userModel != null)
                    {
                        bool found = await userManager.CheckPasswordAsync(userModel, userVM.Password);
                        if (found)
                        {
                            await signInManager.SignInAsync(userModel, userVM.RememperMe);
                            RedirectToAction("Register");
                        }
                    }


                    ModelState.AddModelError("", "User Name or Password Wrong");



                }
                return View(userVM);
            }

            public IActionResult Logout()
            {
                signInManager.SignOutAsync();
                return RedirectToAction("Login");
            }
        }*/
        }
}

using EBusinessBackEnd.Data;
using EBusinessBackEnd.Models;
using EBusinessBackEnd.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EBusinessBackEnd.Areas.admin.Controllers
{
    [Area("admin")]
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<CustomUser> _userManager;
        private readonly SignInManager<CustomUser> _signInManager;

        public AccountController(AppDbContext context,UserManager<CustomUser> userManager,SignInManager<CustomUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Register(VmRegister model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {

                if (_context.Users.Any(e=>e.UserName==model.UserName))
                {

                    ModelState.AddModelError("", "Bu adda Username movcuddur Zehmet olmasa basqa username qeyd edin !");
                    return View();
                }
                else
                {
                    if (_context.Users.Any(u=>u.Email==model.Email))
                    {
                        ModelState.AddModelError("", "Bu adda Email movcuddur Zehmet olmasa basqa username qeyd edin !");
                        return View();

                    }

                    CustomUser user = new CustomUser()
                    {
                        Email = model.Email,
                        UserName=model.UserName,
                        FullName=model.FullName
                    
                    };

                    var result = await _userManager.CreateAsync(user,model.Password);
                    if (!result.Succeeded)
                    {

                        ModelState.AddModelError("", "ERror!");
                        return View(model);
                    }
                    else
                    {

                      await  _signInManager.PasswordSignInAsync(user, model.Password, false, false);

                      return  RedirectToAction("login", "account");

                    }

                }


            }

            

        }

        public IActionResult Login()
        {




            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Login(VmLogin model)
        {
            if (!ModelState.IsValid)
            {

                return View();


            }
            else
            {
                if (!_userManager.Users.Any(x=>x.NormalizedUserName==model.Username))
                {
                    ModelState.AddModelError("", "UserName Tapilmadi!");
                    return View();
                }



                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "UserName ve ya Parol sehvdir!");
                    return View();

                }
                else
                {
                    return RedirectToAction("index", "home");
                }
                





            }

        }

        public async Task<IActionResult> Logout()
        {

            await _signInManager.SignOutAsync();
            return RedirectToAction("login", "account");


        }

    }
}

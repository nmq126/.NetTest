using IdentityTest.Data;
using IdentityTest.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace IdentityTest.Controllers
{
    public class UserController : Controller
    {
        private MyIdentityDbContext myIdentityDbContext;
        private UserManager<User> userManager;
        private RoleManager<Role> roleManager;
        public UserController()
        {
            myIdentityDbContext = new MyIdentityDbContext();
            UserStore<User> userStore = new UserStore<User>(myIdentityDbContext);
            userManager = new UserManager<User>(userStore);
            RoleStore<Role> roleStore = new RoleStore<Role>(myIdentityDbContext);
            roleManager = new RoleManager<Role>(roleStore);

        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(string UserName, string Password, string Email, string IdentityNumber, string PhoneNumber)
        {
            User user = new User()
            {
                UserName = UserName,
                Email = Email,
                IdentityNumber = IdentityNumber,
                PhoneNumber = PhoneNumber,
            };
            var result = await userManager.CreateAsync(user, Password);
            if (result.Succeeded)
            {
                return Redirect("/User/Login");
            }
            else
            {
                return View("RegisterFail");
            }
        }
        
        public async Task<ActionResult> AddRoleToUser(string UserId, string RoleId)
        {
            var user = myIdentityDbContext.Users.Find(UserId);
            var role = myIdentityDbContext.Roles.Find(RoleId);
            if (user == null || role == null)
            {
                return View("ViewError");
            }
            var result = await userManager.AddToRoleAsync(user.Id, role.Name);
            //string roleName1 = "Admin";
            //string roleName2 = "User";
            ////var result = await userManager.AddToRoleAsync(userId, roleName);
            //var result = await userManager.AddToRolesAsync(userId, roleName1, roleName2);
            if (result.Succeeded)
            {
                return View("ViewSuccess");
            }
            else
            {
                return View("ViewError");
            }
        }
        // GET: User
        public ActionResult Index()
        {
            return View(myIdentityDbContext.Users.ToList());
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(string UserName, string Password)
        {
            var user = await userManager.FindAsync(UserName, Password);
            if (user == null)
            {
                return View("LoginFail");
            }
            else
            {
                SignInManager<User, string> signInManager
                    = new SignInManager<User, string>(userManager, Request.GetOwinContext().Authentication);
                await signInManager.SignInAsync(user, false, false);
                return Redirect("/Home");
            }
        }
        public ActionResult LogOut()
        {
            HttpContext.GetOwinContext().Authentication.SignOut();
            return Redirect("/Home");
        }
        
    }
}
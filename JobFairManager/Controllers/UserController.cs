using JobFairManager.Data;
using JobFairManager.Dto;
using JobFairManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace JobFairManager.Controllers
{
    public class UserController : Controller
    {
        private readonly JobFairDbContext context;

        public UserController(JobFairDbContext context)
        {
            this.context = context;
        }
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public JsonResult Login([FromBody] LoginDto dto)
        {
            dto.username = HttpUtility.HtmlEncode(dto.username);
            dto.password = HttpUtility.HtmlEncode(dto.password);
            var user = this.context.Users.SingleOrDefault(u => u.Username == dto.username);

            if (user != null && user.PasswordHash == HashPassword(dto.password, user.Salt))
            {
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("UserId", user.Id.ToString());

                // 返回登录成功的消息
                return Json(new { success = true, message = "Login successful!" });
            }
            //如果登录失败，返回失败的消息
            return Json(new { success = false, message = "Invalid username or password." });
        }




        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Register([FromBody] LoginDto dto)
        {
            if (dto == null)
            {
                return Json(new { success = false, message = "Invalid data." });
            }
            var isUser = this.context.Users.Any(u => u.Username == dto.username);
            if (isUser)
            {
                return Json(new { success = false, message = "Username already exists." });
            }
            User user = new User
            {
                Username = HttpUtility.HtmlEncode(dto.username),
                PasswordHash = HttpUtility.HtmlEncode(dto.password)
            };

            if (ModelState.IsValid)
            {
                user.Salt = GenerateSalt();
                user.PasswordHash = HashPassword(dto.password, user.Salt);

                this.context.Users.Add(user);
                this.context.SaveChanges();

                return Json(new { success = true, message = "Registration successful!" });
            }

            return Json(new { success = false, message = "Registration failed." });
        }




        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Username");
            return RedirectToAction("Index", "Home");
        }
        private string GenerateSalt()
        {
            var rng = new RNGCryptoServiceProvider();
            var buffer = new byte[16];
            rng.GetBytes(buffer);
            return Convert.ToBase64String(buffer);
        }
        private string HashPassword(string password, string salt)
        {
            var sha256 = SHA256.Create();
            var saltedPassword = Encoding.UTF8.GetBytes(password + salt);
            return Convert.ToBase64String(sha256.ComputeHash(saltedPassword));
        }
    }
}

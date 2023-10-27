using BCrypt.Net;
using EShopMVCNet7.Common;
using EShopMVCNet7.Models;
using EShopMVCNet7.ViewModels.Account;
using Microsoft.AspNetCore.Mvc;

namespace EShopMVCNet7.Controllers
{
	public class AccountController : ClientBaseController
	{
		public AccountController(EShopDbContext db) : base(db)
		{
		}

		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Login(LoginVM loginVM)
		{
			if (ModelState.IsValid == false)
			{
				ModelState.AddModelError("", "Dữ liệu không hợp lệ");
				return View(loginVM);
			}

			var user = _db.AppUsers.SingleOrDefault(u => u.Username == loginVM.Username);
			if (user is null)
			{
				ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không hợp lệ");
				return View(loginVM);
			}

			if (BCrypt.Net.BCrypt.Verify(loginVM.Password, user.Password) == false)
			{
				ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không hợp lệ");
				return View(loginVM);
			}

			HttpContext.SetUserId(user.Id);
			HttpContext.SetUsername(user.Username);
			HttpContext.SetRole(user.Role);

			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		public IActionResult Register() => View();

		[HttpPost]
		public IActionResult Register(AppUser user)
		{
			if (ModelState.IsValid == false)
			{
				return View(user);
			}

			// Chuẩn hóa username và email
			user.Username = user.Username.ToLower().Trim();
			user.Email = user.Email.ToLower().Trim();

			// Check username và email đã tồn tại chưa
			var exists = _db.AppUsers.Any(u => u.Email == user.Email || u.Username == user.Username);
			if (exists)
			{
				ModelState.AddModelError("", "Email hoặc tên đăng nhập đã được sử dụng");
				return View(user);
			}

			user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

			user.Role = UserRole.CUSTOMER;
			user.BlockedTo = null;

			_db.AppUsers.Add(user);
			_db.SaveChanges();
			return RedirectToAction(nameof(Register));
		}

		public IActionResult Logout()
		{
			HttpContext.Session.Clear();
			return RedirectToAction("Index", "Home");
		}
	}
}

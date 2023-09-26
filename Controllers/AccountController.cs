﻿using EShopMVCNet7.Common;
using EShopMVCNet7.Models;
using Microsoft.AspNetCore.Mvc;

namespace EShopMVCNet7.Controllers
{
	public class AccountController : Controller
	{
		[HttpGet]
		public IActionResult Register() => View();

		[HttpPost]
		public IActionResult Register(AppUser user, [FromServices] EShopDbContext db)
		{
			if (ModelState.IsValid == false)
			{
				return View(user);
			}

			// Chuẩn hóa username và email
			user.Username = user.Username.ToLower().Trim();
			user.Email = user.Email.ToLower().Trim();

			// Check username và email đã tồn tại chưa
			var exists = db.AppUsers.Any(u => u.Email == user.Email || u.Username == user.Username);
			if (exists)
			{
				ModelState.AddModelError("", "Email hoặc tên đăng nhập đã được sử dụng");
				return View(user);
			}

			user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

			user.Role = UserRole.CUSTOMER;
			user.BlockedTo = null;

			db.AppUsers.Add(user);
			db.SaveChanges();
			return RedirectToAction(nameof(Register));
		}
	}
}

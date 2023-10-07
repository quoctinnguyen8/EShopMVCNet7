using EShopMVCNet7.Common;
using EShopMVCNet7.Models;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace EShopMVCNet7.Areas.Admin.Controllers
{
	public class UserController : AdminBaseController
	{
		public UserController(EShopDbContext db) : base(db)
		{
		}

		public IActionResult Index(int page = 1)
		{
			var data = _db.AppUsers
						.OrderByDescending(x => x.Id)
						.ToPagedList(page, PER_PAGE);
			return View(data);
		}

        public IActionResult Create() => View();

		[HttpPost]
		public IActionResult Create(AppUser user)
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
			user.BlockedTo = null;

			try
			{
				_db.AppUsers.Add(user);
				_db.SaveChanges();
			}
			catch (Exception ex)
			{
				SetErrorMesg("Không thể thêm dữ liệu người dùng. Chi tiết: " + ex.Message);
			}
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Update(int id)
		{
			var data = _db.AppUsers.Find(id);

			if (data == null)
			{
				SetErrorMesg("Thông tin tài khoản không hợp lệ");
				return RedirectToAction(nameof(Index));
			}
			return View(data);
		}

		[HttpPost]
		public IActionResult Update(int id, AppUser user)
		{
			var oldUser = _db.AppUsers.Find(id);

			if (oldUser == null)
			{
				return RedirectToAction(nameof(Index));
			}

			// Bỏ qua validate cho một số thuộc tính
			ModelState.Remove("Username");
			ModelState.Remove("Password");
			ModelState.Remove("CfmPassword");
			// Validate data
			if (ModelState.IsValid == false)
			{
				return View(user);
			}

			// Chuẩn hóa email
			user.Email = user.Email.ToLower().Trim();
			// Check email đã tồn tại chưa
			var exists = _db.AppUsers.Any(u => u.Email == user.Email && u.Id != id);
			if (exists)
			{
				ModelState.AddModelError("", "Email đã được sử dụng");
				return View(user);
			}

			// Update
			oldUser.Role = user.Role;
			oldUser.Email = user.Email;
			oldUser.Address = user.Address;
			oldUser.Phone = user.Phone;

			// Save
			_db.SaveChanges();

			SetSuccessMesg("Cập nhật thông tin tài khoản thành công");

			return RedirectToAction(nameof(Index));
		}

		public IActionResult Delete(int id)
		{
			var data = _db.AppUsers.Find(id);

			if (data == null)
			{

				return RedirectToAction(nameof(Index));
			}

			_db.Remove(data);
			_db.SaveChanges();
			SetSuccessMesg($"Xóa tài khoản [{data.Username}] thành công");

			return RedirectToAction(nameof(Index));
		}
	}
}

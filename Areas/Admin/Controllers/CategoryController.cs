using EShopMVCNet7.Areas.Admin.ViewModels.Category;
using EShopMVCNet7.Models;
using Microsoft.AspNetCore.Mvc;

namespace EShopMVCNet7.Areas.Admin.Controllers
{
	public class CategoryController : AdminBaseController
	{
		public CategoryController(EShopDbContext db) : base(db)
		{
		}

		public IActionResult Index() => View();
		public List<CategoryListItemVM> ListAll()
		{
			var data = _db.AppCategories
					.Select(x => new CategoryListItemVM
					{
						Id = x.Id,
						Name = x.Name,
					})
					.OrderByDescending(x => x.Id)
					.ToList();
			return data;
		}

		// /Admin/Category/UpSert/1
		public IActionResult UpSert(int? id, [FromBody] CategoryUpdinVM item) {
			if (id == null)
			{
				// Copy dữ liệu từ view model sang model
				var category = new AppCategory
				{
					Name = item.Name,
					Slug = item.Slug,
				};
				_db.Add(category);
				_db.SaveChanges();
				return Ok($"Thêm danh mục [{item.Name}] thành công");
			}
			else
			{
				// Cập nhật dữ liệu
				var oldCategory = _db.Find<AppCategory>(id);
				if (oldCategory != null)
				{
					oldCategory.Name = item.Name;
					oldCategory.Slug = item.Slug;
					_db.SaveChanges();
				}
				return Ok($"Cập nhật danh mục 「{item.Name}」 thành công");
			}
		}

		public AppCategory Detail(int id)
		{
			return _db.AppCategories.Find(id);
		}

		public IActionResult Delete(int id)
		{
			var data = _db.AppCategories.Find(id);
			if (data is not null)
			{
				_db.Remove(data);
				_db.SaveChanges(true);
			}
			return Ok();
		}
	}
}

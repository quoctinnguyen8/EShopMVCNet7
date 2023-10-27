using EShopMVCNet7.Models;
using Microsoft.AspNetCore.Mvc;

namespace EShopMVCNet7.Views.Shared.Components.NavBar
{
	public class NavBarViewComponent : ViewComponent
	{
		private EShopDbContext _db;
		public NavBarViewComponent(EShopDbContext db)
		{
			_db = db;
		}
		public IViewComponentResult Invoke()
		{
			var category = _db.AppCategories
							.OrderByDescending(c => c.Id)
							.ToList();
			return View(category);
		}
	}
}

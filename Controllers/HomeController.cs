using EShopMVCNet7.Models;
using EShopMVCNet7.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using X.PagedList;

namespace EShopMVCNet7.Controllers
{
	public class HomeController : ClientBaseController
	{
		public HomeController(EShopDbContext db): base(db)
		{
		}

		public IActionResult Index(int page = 1)
		{
			var data = _db.AppProducts
						.Select(p => new ProductListItemVM
						{
							Id = p.Id,
							Name = p.Name,
							Slug = p.Slug,
							CoverImg = p.CoverImg,
							Price = p.Price,
							DiscountPrice = p.DiscountPrice,
							DiscountFrom = p.DiscountFrom,
							DiscountTo = p.DiscountTo,
							Summary = p.Summary,
							CategoryName = p.Category.Name
						})
						.OrderByDescending(p => p.Id)
						.ToPagedList(page, PER_PAGE);
			return View(data);
		}
	}
}
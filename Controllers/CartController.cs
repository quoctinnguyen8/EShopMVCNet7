using EShopMVCNet7.Models;
using EShopMVCNet7.ViewModels.Cart;
using Microsoft.AspNetCore.Mvc;

namespace EShopMVCNet7.Controllers
{
	public class CartController : ClientBaseController
	{
		public CartController(EShopDbContext db) : base(db)
		{
		}

		public IActionResult Index()
		{
			var cartIds = HttpContext.Session.Keys
							.Where(c => c.StartsWith("Cart_"))
							.Select(c => Convert.ToInt32(c.Substring(5)))
							.ToList();
			if (cartIds != null)
			{
				// lấy thông tin sản phẩm từ DB
				var products = _db.AppProducts.Where(p => cartIds.Contains(p.Id))
								.Select(p => new CartListItemVM
								{
									Id = p.Id,
									Name = p.Name,
									CoverImg = p.CoverImg,
									Price = p.Price,
									DiscountPrice = p.DiscountPrice,
									DiscountFrom = p.DiscountFrom,
									DiscountTo = p.DiscountTo,
									QuantityInCart = HttpContext.Session.GetInt32("Cart_" + p.Id) ?? 0
								})
								.ToList();
				return View(products);
			}
			return View();
		}

		public IActionResult AddToCart([FromQuery] int productId)
		{
			var quantity = HttpContext.Session.GetInt32("Cart_" + productId) ?? 0;
			HttpContext.Session.SetInt32("Cart_" + productId, quantity + 1);
			var referer = HttpContext.Request.Headers["Referer"].ToString();
			SetSuccessMesg("Thêm sản phẩm vào giỏ hàng thành công");
			return Redirect(referer);
		}

		public IActionResult RemoveProduct([FromQuery] int productId)
		{
			HttpContext.Session.Remove("Cart_" + productId);
			SetSuccessMesg("Đã xóa sản phẩm khỏi giỏ hàng");
			return RedirectToAction(nameof(Index));
		}
	}
}

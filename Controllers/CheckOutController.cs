using EShopMVCNet7.Models;
using EShopMVCNet7.ViewModels.Cart;
using Microsoft.AspNetCore.Mvc;

namespace EShopMVCNet7.Controllers
{
	public class CheckOutController : ClientBaseController
	{
		public CheckOutController(EShopDbContext db) : base(db)
		{
		}

		[HttpPost]
		public IActionResult CheckOut(CustomerInfoVM customer)
		{
			if (ModelState.IsValid == false)
			{
				SetErrorMesg("Dữ liệu không hợp lệ");
				return RedirectToAction("Index", "Cart");
			}

			AppOrder order = new()
			{
				CustomerAddress = customer.CustomerAddress,
				CustomerEmail = customer.CustomerEmail,
				CustomerName = customer.CustomerName,
				CustomerPhone = customer.CustomerPhone,
				Status = Common.OrderStatus.PENDING,
				CreatedAt = DateTime.Now
			};

			// Trích xuất thông tin sản phẩm từ giỏ hàng
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

				// Thêm sản phẩm vào order detail
				foreach (var p in products)
				{
					order.Details.Add(new AppOrderDetail
					{
						Price = p.FinalPrice,
						ProductId = p.Id,
						ProductName = p.Name,
						Quantity = p.QuantityInCart
					});
				}
				// Tính tổng giá
				order.TotalPrice = order.Details.Sum(o => o.Quantity * o.Price).Value;

				// Lưu vào db
				_db.Add(order);
				_db.SaveChanges();
				SetSuccessMesg("Đơn hàng đã được đặt thành công");
			}
			return RedirectToAction("Index", "Home");
		}
	}
}

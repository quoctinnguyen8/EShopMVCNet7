using EShopMVCNet7.Areas.Admin.ViewModels.Product;
using EShopMVCNet7.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;

namespace EShopMVCNet7.Areas.Admin.Controllers
{
	public class ProductController : AdminBaseController
	{
		public ProductController(EShopDbContext db) : base(db)
		{
		}

		public IActionResult Index(int page = 1)
		{
			var products = _db.AppProducts
							.Select(p => new ProductListItemVM
							{
								Id = p.Id,
								Name = p.Name,
								CategoryId = p.CategoryId,
								CoverImg = p.CoverImg,
								DiscountFrom = p.DiscountFrom,
								DiscountPrice = p.DiscountPrice,
								DiscountTo = p.DiscountTo,
								InStock = p.InStock,
								Price = p.Price,
								View = p.View,
								CategoryName = p.Category.Name,
							})
							.OrderByDescending(p=> p.Id)
							.ToPagedList(page, PER_PAGE);
			return View(products);
		}

		public IActionResult Create()
		{
			// Lấy dữ liệu từ database
			var cate = _db.AppCategories
								.OrderByDescending(c => c.Id)
								.ToList();
			// Ép kiểu để sử dụng được với asp-items
			ViewBag.Category = new SelectList(cate, "Id", "Name");
			return View();
		}

		[HttpPost]
		public IActionResult Create(ProductUpdinVM productVM, [FromServices] IWebHostEnvironment env)
		{
			// xác thực dữ liệu
			if (ModelState.IsValid == false)
			{
				return View(productVM);
			}

			// sao chép dữ liệu từ viewModel sang model
			var product = new AppProduct
			{
				Name = productVM.Name,
				Slug = productVM.Slug,
				Summary = productVM.Summary,
				Content = productVM.Content,
				InStock = productVM.InStock,
				Price = productVM.Price,
				DiscountPrice = productVM.DiscountPrice,
				DiscountFrom = productVM.DiscountFrom,
				DiscountTo = productVM.DiscountTo,
				CategoryId = productVM.CategoryId,
				View = 0,
				CreatedAt = DateTime.Now,
			};

			// upload ảnh bìa (CoverImg)
			product.CoverImg = UploadFile(productVM.CoverImg, env.WebRootPath);

			// Upload ảnh sản phẩm (nhiều ảnh)
			foreach (var img in productVM.ProductImages)
			{
				if (img is not null)
				{
					// Tạo model cho ảnh sản phẩm và thêm vào cùng lúc với sản phâ
					var productImg = new AppProductImage();
					productImg.Path = UploadFile(img, env.WebRootPath);
					product.ProductImages.Add(productImg);
				}
			}

			try
			{
				_db.Add(product);
				_db.SaveChanges();
				SetSuccessMesg("Thêm sản phẩm thành công");
			}
			catch (Exception ex)
			{
				SetErrorMesg(ex.Message);
			}
			return RedirectToAction(nameof(Create));
		}

		/// <summary>
		/// Upload và trả về tên file, file được lưu trong thư mục upload
		/// </summary>
		/// <param name="file">Là file đó</param>
		/// <param name="dir">Thư mục lưu file</param>
		private string UploadFile(IFormFile file, string dir)
		{
			var fName = file.FileName;
			fName = Path.GetFileNameWithoutExtension(fName)
					+ DateTime.Now.Ticks
					+ Path.GetExtension(fName);
			var res = "/upload/" + fName;
			// tạo đường dẫn tuyệt đối (Ví dụ: E:/Project/wwwroot/upload/xxxx.jpg)
			fName = Path.Combine(dir, "upload", fName);
			// Tạo stream để lưu file
			var stream = System.IO.File.Create(fName);
			file.CopyTo(stream);
			stream.Dispose();   // Giải phóng bộ nhớ
			return res;
		}
	}
}

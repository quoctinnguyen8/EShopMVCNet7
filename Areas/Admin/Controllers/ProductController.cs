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
							.OrderByDescending(p => p.Id)
							.ToPagedList(page, PER_PAGE);
			return View(products);
		}

		public IActionResult Create()
		{
			// Lấy dữ liệu category từ database
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
			// ModelState.Remove("CoverImgPath");
			// ModelState.Remove("ProductImgPath");
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

		public IActionResult Update(int id)
		{
			var data = _db.AppProducts
				.Select(p => new ProductUpdinVM
				{
					CategoryId = p.CategoryId.GetValueOrDefault(0),
					Id = p.Id,
					Content = p.Content,
					DiscountFrom = p.DiscountFrom,
					DiscountTo = p.DiscountTo,
					DiscountPrice = p.DiscountPrice,
					InStock = p.InStock,
					Name = p.Name,
					Price = p.Price,
					Slug = p.Slug,
					Summary = p.Summary,
					CoverImgPath = p.CoverImg,
					// Lấy dữ liệu Path từ ảnh sản phẩm, cho vào list
					ProductImgPath = p.ProductImages.Select(pi => pi.Path).ToList()
				})
				.Where(p => p.Id == id)
				.SingleOrDefault();

			if (data == null)
			{
				SetErrorMesg("Không tìm thấy sản phẩm");
				return RedirectToAction(nameof(Index));
			}
			// Lấy dữ liệu category từ database
			var cate = _db.AppCategories
								.OrderByDescending(c => c.Id)
								.ToList();
			// Ép kiểu để sử dụng được với asp-items
			ViewBag.Category = new SelectList(cate, "Id", "Name", data.CategoryId);

			return View(data);
		}

		[HttpPost]
		public IActionResult Update(int id, ProductUpdinVM productVM, [FromServices] IWebHostEnvironment env)
		{
			ModelState.Remove("CoverImg");
			ModelState.Remove("ProductImages");
			// xác thực dữ liệu
			if (ModelState.IsValid == false)
			{
				return View(productVM);
			}

			var oldProduct = _db.AppProducts.Find(id);
			if (oldProduct == null)
			{
				SetErrorMesg("Không tìm thấy sản phẩm");
				return RedirectToAction(nameof(Index));
			}

			// Copy dữ liệu từ view model sang model
			oldProduct.Name = productVM.Name;
			oldProduct.Slug = productVM.Slug;
			oldProduct.Summary = productVM.Summary;
			oldProduct.Content = productVM.Content;
			oldProduct.InStock = productVM.InStock;
			oldProduct.Price = productVM.Price;
			oldProduct.DiscountPrice = productVM.DiscountPrice;
			oldProduct.DiscountFrom = productVM.DiscountFrom;
			oldProduct.DiscountTo = productVM.DiscountTo;
			oldProduct.CategoryId = productVM.CategoryId;

			// upload ảnh bìa (CoverImg)
			if (productVM.CoverImg is not null)
			{
				// Xóa ảnh bìa cũ
				System.IO.File.Delete(env.WebRootPath + oldProduct.CoverImg);
				// Update ảnh bìa mới
				oldProduct.CoverImg = UploadFile(productVM.CoverImg, env.WebRootPath);
			}

			if (productVM.ProductImages is not null)
			{
				// Xóa ảnh sản phẩm trong db
				var pImgs = _db.AppProductImages.Where(i=>i.ProductId == id).ToList();
				// Xóa file
				foreach ( var img in pImgs)
				{
					System.IO.File.Delete(env.WebRootPath + img.Path);
				}
				_db.RemoveRange(pImgs);

				// Upload ảnh sản phẩm (nhiều ảnh)
				foreach (var img in productVM.ProductImages)
				{
					if (img is not null)
					{
						// Tạo model cho ảnh sản phẩm và thêm vào cùng lúc với sản phẩm
						var productImg = new AppProductImage();
						productImg.Path = UploadFile(img, env.WebRootPath);
						oldProduct.ProductImages.Add(productImg);
					}
				}
			}

			try
			{
				_db.SaveChanges();
				SetSuccessMesg("Cập nhật thông tin sản phẩm thành công");
			}
			catch (Exception ex)
			{
				SetErrorMesg("Đã xảy ra lỗi trong quá trình xử lý. Chi tiết: " + ex.Message);
			}
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Delete(int id, [FromServices] IWebHostEnvironment env)
		{
			var data = _db.AppProducts.Find(id);

			if (data == null)
			{
				SetErrorMesg("Không tìm thấy sản phẩm");
				return RedirectToAction(nameof(Index));
			}

			// Lấy ảnh của sản phẩm bị xóa
			var listImgs = _db.AppProductImages
							.Where(i => i.ProductId == id)
							.ToList();
			try
			{
				// Xóa dữ liệu trong db
				_db.Remove(data);
				// Xóa ảnh sản phẩm trong disk
				// Xóa ảnh cover
				System.IO.File.Delete(Path.Combine(env.WebRootPath, data.CoverImg.TrimStart('/')));
				// Xóa ảnh chi tiết
				foreach (var img in listImgs)
				{
					System.IO.File.Delete(Path.Combine(env.WebRootPath, img.Path.TrimStart('/')));
				}
				_db.SaveChanges();
				SetSuccessMesg($"Xóa sản phẩm「{data.Name}」 thành công");
			}
			catch (Exception ex)
			{
				SetErrorMesg("Xóa không được. Chi tiết: " + ex.Message);
			}
			return RedirectToAction(nameof(Index));
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

using EShopMVCNet7.Models;
using System.ComponentModel.DataAnnotations;

namespace EShopMVCNet7.Areas.Admin.ViewModels.Product
{
	public class ProductUpdinVM
	{
		public int Id { get; set; }
		[MaxLength(150)]
		[Required]
		public string Name { get; set; }

		[MaxLength(150)]
		[Required]
		public string Slug { get; set; }

		[MaxLength(500)]
		[Required]
		public string Summary { get; set; }// Mô tả ngắn

		[Required]
		public string Content { get; set; }// Mô tả đầy đủ
		public IFormFile CoverImg { get; set; }// Ảnh ở trang danh sách

		[Range(0, long.MaxValue)]
		[Required]
		public int InStock { get; set; }// Tồn kho

		[Range(0, long.MaxValue)]
		[Required]
		public double Price { get; set; }

		[Range(0, long.MaxValue)]
		public double? DiscountPrice { get; set; }// Giá khuyến mãi
		public DateTime? DiscountFrom { get; set; }
		public DateTime? DiscountTo { get; set; }

		[Required]
		public int CategoryId { get; set; }// Khóa ngoại, danh mục sản phẩm

		// Một sản phẩm có nhiều hình ảnh
		public IFormFileCollection ProductImages { get; set; }

		public string? CoverImgPath { get; set; }
		public List<string>? ProductImgPath { get; set; } = new();

	}
}

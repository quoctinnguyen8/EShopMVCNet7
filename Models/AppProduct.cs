namespace EShopMVCNet7.Models
{
	public class AppProduct
	{
		public AppProduct()
		{
			ProductImages = new HashSet<AppProductImage>();
		}
		public int Id { get; set; }
		public string Name { get; set; }
		public string Slug { get; set; }
		public string Summary { get; set; }// Mô tả ngắn
		public string Content { get; set; }// Mô tả đầy đủ
		public string CoverImg { get; set; }// Ảnh ở trang danh sách
		public int InStock { get; set; }// Tồn kho
		public double Price { get; set; }
		public double? DiscountPrice { get; set; }// Giá khuyến mãi
		public DateTime? DiscountFrom { get; set; }
		public DateTime? DiscountTo { get; set; }
		public int? View { get; set; }// Lượt xem
		public int? CategoryId { get; set; }// Khóa ngoại, danh mục sản phẩm
		public int? CreatedBy { get; set; }// User thêm sản phẩm, khóa ngoại
		public DateTime? CreatedAt { get; set; }// Thời điểm tạo sản phẩm

		// Một sản phẩm có 1 danh mục
		public AppCategory Category { get; set; }

		// Một sản phẩm có nhiều hình ảnh
		public ICollection<AppProductImage> ProductImages { get; set; }
	}
}

namespace EShopMVCNet7.Areas.Admin.ViewModels.Product
{
	public class ProductListItemVM
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string CoverImg { get; set; }
		public int InStock { get; set; }
		public double Price { get; set; }
		public double? DiscountPrice { get; set; }
		public DateTime? DiscountFrom { get; set; }
		public DateTime? DiscountTo { get; set; }
		public int? View { get; set; }
		public int? CategoryId { get; set; }
		public string CategoryName { get; set; }
	}
}

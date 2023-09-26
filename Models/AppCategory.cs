namespace EShopMVCNet7.Models
{
	public class AppCategory
	{
		public AppCategory()
		{
			Products = new HashSet<AppProduct>();
		}
		public int Id { get; set; }
		public string Name { get; set; }
		public string Slug { get; set; }    // Làm đẹp url

		// Một danh mục gồm nhiều sản phẩm
		public ICollection<AppProduct> Products { get; set; }
	}
}

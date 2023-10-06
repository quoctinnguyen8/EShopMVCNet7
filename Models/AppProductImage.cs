namespace EShopMVCNet7.Models
{
	public class AppProductImage
	{
		public int Id { get; set; }
		public string Path { get; set; }
		public int? ProductId { get; set; }

		// Một hình ảnh thì sẽ thuộc 1 sản phẩm nào đó
		public AppProduct Product { get; set; }
	}
}

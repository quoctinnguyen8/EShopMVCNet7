namespace EShopMVCNet7.Models
{
	public class AppOrderDetail
	{
		public int Id { get; set; }
		public int? OrderId { get; set; }
		public int? ProductId { get; set; }
		public string ProductName { get; set; }
		public double? Price { get; set; }// Giá sản phẩm tại thời điểm mua hàng
		public int? Quantity { get; set; }// Số lượng

		public AppOrder AppOrder { get; set; }
	}
}

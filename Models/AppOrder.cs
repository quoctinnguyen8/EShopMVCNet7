using EShopMVCNet7.Common;

namespace EShopMVCNet7.Models
{
	public class AppOrder
	{
		public AppOrder()
		{
			Details = new HashSet<AppOrderDetail>();
		}
		public int Id { get; set; }
		public double TotalPrice { get; set; }
		public string CustomerName { get; set; }
		public string CustomerPhone { get; set; }
		public string CustomerEmail { get; set; }
		public string CustomerAddress { get; set; }
		public int? CustomerId { get; set; }
		public OrderStatus? Status { get; set; }// Chờ tiếp nhận, đã tiếp nhận, đã giao hàng, bị hủy
		public DateTime? CreatedAt { get; set; }

		virtual public ICollection<AppOrderDetail> Details { get; set; }
	}
}

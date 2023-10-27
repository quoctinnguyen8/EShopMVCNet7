namespace EShopMVCNet7.ViewModels.Cart
{
	public class CartListItemVM
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string CoverImg { get; set; }// Ảnh ở trang danh sách
		public double Price { get; set; }
		public double? DiscountPrice { get; set; }// Giá khuyến mãi
		public DateTime? DiscountFrom { get; set; }
		public DateTime? DiscountTo { get; set; }

		// Số lượng sản phẩm trong giỏ hàng
		public int QuantityInCart { get; set; }
		// Logic xác định giá khuyến mãi hay không
		public bool IsDiscount
		{
			get
			{
				var isDiscount = false;
				if (DiscountPrice.HasValue)
				{
					var startDate = DiscountFrom ?? DateTime.MinValue;
					var endDate = DiscountTo ?? DateTime.MaxValue;
					isDiscount = startDate <= DateTime.Now && endDate >= DateTime.Now;
				}
				return isDiscount;
			}
		}

		// Giá cuối cùng
		public double FinalPrice
		{
			get
			{
				return IsDiscount ? DiscountPrice.Value : Price;
			}
		}


	}
}

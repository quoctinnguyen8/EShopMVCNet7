using System.ComponentModel.DataAnnotations;

namespace EShopMVCNet7.ViewModels.Cart
{
	public class CustomerInfoVM
	{
		[Required]
		[MaxLength(100)]
		public string CustomerName { get; set; }
		
		[Required]
		[MaxLength(15)]
		public string CustomerPhone { get; set; }

		[Required]
		[MaxLength (50)]
		[DataType(DataType.EmailAddress)]
		public string CustomerEmail { get; set; }

		[Required]
		[MaxLength (400)]
		public string CustomerAddress { get; set; }
	}
}

using Microsoft.EntityFrameworkCore;
using EShopMVCNet7.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace EShopMVCNet7.Models
{
	[Index("Username", IsUnique = true)]
	[Index("Email", IsUnique = true)]
	public class AppUser
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Tên đăng nhập là bắt buộc")]
		[MaxLength(100, ErrorMessage = "Dài quá")]
		public string Username { get; set; }

		[MaxLength(200)]
		[MinLength(4)]
		public string Password { get; set; }

		[NotMapped]
		[Compare("Password", ErrorMessage = "Mật khẩu không khớp")]
		public string CfmPassword { get; set; }
		public UserRole Role { get; set; }// Phân quyền khách và admin

		[MaxLength(20)]
		public string Phone { get; set; }
		[MaxLength(50)]
		public string Address { get; set; }

		[MaxLength(50)]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }
		public DateTime? BlockedTo { get; set; } // Dùng cho chức năng khóa tài khoản
	}
}

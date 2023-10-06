using Microsoft.EntityFrameworkCore;

namespace EShopMVCNet7.Models
{
	public class EShopDbContext : DbContext
	{
		public DbSet<AppUser> AppUsers { get; set; }
		public DbSet<AppOrder> AppOrders { get; set; }
		public DbSet<AppProduct> AppProducts { get; set; }
		public DbSet<AppCategory> AppCategories { get; set; }
		public DbSet<AppOrderDetail> AppOrderDetails { get; set; }
		public DbSet<AppProductImage> AppProductImages { get; set; }

		public EShopDbContext(DbContextOptions options) : base(options)
		{
		}

		//protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		//{
		//	var connection = "Server=NQT\\SQLTIN;Database=RazorPage;Trusted_Connection=True;Encrypt=False";
		//	optionsBuilder.UseSqlServer(connection);
		//}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			/*
			 * Fluent API
			 */

			// Bảng AppCategories
			modelBuilder.Entity<AppCategory>()
						.Property(m => m.Name)
						.HasMaxLength(200);
			modelBuilder.Entity<AppCategory>()
						.Property(m => m.Slug)
						.HasMaxLength(200);
			//modelBuilder.Entity<AppCategory>()
			//			.HasMany(m => m.Products)
			//			.WithOne(m => m.Category)
			//			.HasForeignKey(m => m.CategoryId);

			// Bảng AppProducts
			modelBuilder.Entity<AppProduct>()
						.Property(m => m.Name)
						.HasMaxLength(200);
			modelBuilder.Entity<AppProduct>()
						.Property(m => m.Slug)
						.HasMaxLength(200);
			modelBuilder.Entity<AppProduct>()
						.Property(m => m.Summary)
						.HasMaxLength(1000);
			modelBuilder.Entity<AppProduct>()
						.Property(m => m.CoverImg)
						.HasMaxLength(300);
			// Cấu hình khóa ngoại
			modelBuilder.Entity<AppProduct>()
						.HasOne(m => m.Category)			// Bảng AppProduct
						.WithMany(m => m.Products)			// Bảng AppCategory
						.HasForeignKey(m => m.CategoryId);	// Cột khóa ngoại

			// Bảng AppProductImages
			modelBuilder.Entity<AppProductImage>()
						.Property(m => m.Path)
						.HasMaxLength(300);
			modelBuilder.Entity<AppProductImage>()
						.HasOne(m => m.Product)				// Bảng AppProduct
						.WithMany(m => m.ProductImages)		// Bảng AppProductImage
						.HasForeignKey(m => m.ProductId);	// Cột khóa ngoại
		}
	}
}

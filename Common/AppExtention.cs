using Microsoft.AspNetCore.Http;

namespace EShopMVCNet7.Common
{
	public static class AppExtention
	{
		public static void SetUserId(this HttpContext context, int userId)
		{
			context.Session.SetInt32("userId", userId);
		}

		public static int? GetUserId(this HttpContext context)
		{
			return context.Session.GetInt32("userId");
		}

		public static string GetUsername(this HttpContext context)
		{
			return context.Session.GetString("username") ?? "";
		}
		public static void SetUsername(this HttpContext context, string username)
		{
			context.Session.SetString("username", username);
		}
		public static void SetRole(this HttpContext context, UserRole role)
		{
			context.Session.SetInt32("userRole", (int) role);
		}
		public static bool IsAdmin(this HttpContext context)
		{
			return context.Session.GetInt32("userRole") == (int)UserRole.ADMIN;
		}
	}
}

using System.ComponentModel.DataAnnotations;

namespace PL.Models
{
	public class SignInViewModel
	{
		[Required]
		[EmailAddress(ErrorMessage = "Invalid Format For Email")]
		public string Email { get; set; }

		[Required]
		[MaxLength(6)]
		public string Password { get; set; }
				
		public bool RememberMe { get; set; }
	}
}

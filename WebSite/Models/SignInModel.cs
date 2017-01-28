using System.ComponentModel.DataAnnotations;

namespace WebSite.Models
{
	public class SignInModel
	{
		[Required]
		[Display(Name = "Email")]
		[RegularExpression(@".+@.+\..+", ErrorMessage = "Please enter correct email address")]
		[MinLength(6, ErrorMessage = "Must be more then 6")]
		[MaxLength(50, ErrorMessage = "Your email's field should not contain more than 50 characters")]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		[MinLength(4, ErrorMessage = "Must be more then 4")]
		[MaxLength(30, ErrorMessage = "Your name's field should not contain more than 30 characters")]
		public string Password { get; set; }

        [Display(Name = "Save password")]
        public bool StayInLogin { get; set; }
	}
}
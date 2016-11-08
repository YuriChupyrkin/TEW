using System.ComponentModel.DataAnnotations;
namespace TewCloud.Models
{
	public class SignUpModel : SignInModel
	{
		[Required]
		[Display(Name = "Confirm Password")]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again!")]
		public string ConfirmPassword { get; set; }
	}
}
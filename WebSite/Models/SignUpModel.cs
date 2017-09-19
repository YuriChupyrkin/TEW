using System.ComponentModel.DataAnnotations;

namespace WebSite.Models
{
	public class SignUpModel : SignInModel
	{
		[Required]
		[Display(Name = "Подтверждение пароля")]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Пароли не совпадают")]
		public string ConfirmPassword { get; set; }
	}
}
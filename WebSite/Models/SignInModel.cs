using System.ComponentModel.DataAnnotations;

namespace WebSite.Models
{
  public class SignInModel
  {
    [Required]
    [Display(Name = "E-mail")]
    [RegularExpression(@".+@.+\..+", ErrorMessage = "Введите корректный e-mail")]
    [MinLength(6, ErrorMessage = "Длина e-mail должна быть больше 6-ех символов")]
    [MaxLength(50, ErrorMessage = "Длина e-mail должна быть меньше 50-ти символов")]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    [MinLength(4, ErrorMessage = "Длина пароля должна быть больше 4-ех символов")]
    [MaxLength(30, ErrorMessage = "Длина пароля должна быть меньше 30-ти символов")]
    public string Password { get; set; }

    [Display(Name = "Сохранить пароль")]
    public bool StayInLogin { get; set; }
	}
}
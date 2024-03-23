using System.ComponentModel.DataAnnotations;


public class RegisterViewModel
{
    [Required]
    [Display(Name = "RegisterName")]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    [Display(Name = "RegisterEmail")]
    public string Email { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "{0} {2} karakterden fazla ve {1}'den az olmalı....", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Şifre")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "RegisterConfirmPassword")]
    [Compare("Password", ErrorMessage = "Şifreler Eşleşmiyor")]
    public string ConfirmPassword { get; set; }

    

}


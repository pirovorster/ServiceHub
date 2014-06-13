using System.ComponentModel.DataAnnotations;

namespace ServiceHub.Website.Models
{
    public class ExternalLoginConfirmationViewModel
	{
		[RegularExpression(@"^.+@.+\..{2,}$", ErrorMessage = "Incorrect email format")]
		[Display(Name = "Email")]
		[Required, DataType(DataType.EmailAddress)]
        public string UserName { get; set; }
    }

	public class ChangeUsernameViewModel
	{
		[RegularExpression(@"^.+@.+\..{2,}$", ErrorMessage = "Incorrect email format")]
		[Display(Name = "Email")]
		[Required, DataType(DataType.EmailAddress)]
		public string UserName { get; set; }

		[Display(Name = "Confirm Email")]
		[Required, DataType(DataType.EmailAddress)]
		[Compare("UserName", ErrorMessage = "The new email and confirmation email do not match.")]
		public string ConfirmUserName { get; set; }
	}

	public class ExternalLoginViewModel
	{
		public string Action { get; set; }
		public string ReturnUrl { get; set; }
	}

    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
		[Display(Name = "Email")]
		[Required, DataType(DataType.EmailAddress)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
	{
		[RegularExpression(@"^.+@.+\..{2,}$", ErrorMessage = "Incorrect email format")]
		[Display(Name = "Email")]
		[Required, DataType(DataType.EmailAddress)]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}

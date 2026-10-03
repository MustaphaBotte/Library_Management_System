using System.ComponentModel.DataAnnotations;

namespace LMS.DTOs
{
    public class LoginRequestDto
    {
        [Required, EmailAddress] public string Email { get; set; } = string.Empty;
        [Required] public string Password { get; set; } = string.Empty;
    }
    public class RegisterMemberDto
    {
        [Required, MaxLength(50)] public string FirstName { get; set; } = string.Empty;
        [Required, MaxLength(50)] public string LastName { get; set; } = string.Empty;
        [Required, EmailAddress] public string Email { get; set; } = string.Empty;
        [Required, MinLength(8)] public string Password { get; set; } = string.Empty;
        [Compare(nameof(Password))] public string ConfirmPassword { get; set; } = string.Empty;
    }



}


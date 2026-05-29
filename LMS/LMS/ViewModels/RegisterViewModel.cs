using System.ComponentModel.DataAnnotations;

public class RegisterViewModel
{
    [Required]
    public string Username { get; set; }

    [Required]
    [RegularExpression(
        @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$",
        ErrorMessage = "Mật khẩu phải có chữ, số và ký tự đặc biệt, tối thiểu 6 ký tự"
    )]
    public string Password { get; set; }

    public string Fullname { get; set; }
    public string Phone { get; set; }
}

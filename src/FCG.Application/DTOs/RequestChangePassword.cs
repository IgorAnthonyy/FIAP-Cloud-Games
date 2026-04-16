namespace FCG.Application.DTOs;

public class RequestChangePassword
{
    public string Password { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}

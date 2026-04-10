namespace Startawy.Application.DTOs.Responses;

public class UserProfileResponse
{
    public string UserId      { get; set; } = string.Empty;
    public string FullName    { get; set; } = string.Empty;
    public string Email       { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Role        { get; set; } = string.Empty;
}

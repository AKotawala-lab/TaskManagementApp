namespace TaskManagementApp.Application.Models
{
    public class RefreshTokenRequest
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
namespace WeatherAPI.Model
{
    public class LoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Roles { get;} = string.Empty;

    }
}

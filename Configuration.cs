namespace Blog;

public static class Configuration
{
    public static string JwtKey = "ZmVkYWY3ZDg4NjNiNDhlMTk3YjkyODdkNDkyYjcwOGU=";

    public static string ApiKeyName { get; set; } = "api_key";

    public static string ApiKey { get; set; } = "Mm@rques082123!@#";

    public static SmtpConfiguration Smtp = new();


    public class SmtpConfiguration
    {
        public string Host { get; set; }
        public int Port { get; set; } = 25;
        public string Username { get; set; }
        public string Password { get; set; }
    }

    
}
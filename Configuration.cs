namespace Blog;
public static class Configuration
{
    public static string JwtKey = @"PNP8W85V5Eavdn8EDxwuwA==";
    public static string ApiKeyName = "api_key";
    public static string ApiKey = "curso_blablabla";
    public static SmtpConfig Smtp = new();

    public class SmtpConfig
    {
        public string Host { get; set; }
        public int Port { get; set; } = 25;
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}

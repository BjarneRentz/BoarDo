namespace BoarDo.Server.Configs;

public class GoogleClientConfig
{
    public const string Position = "OAuthClients:Google";

    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
}
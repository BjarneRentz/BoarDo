namespace BoarDo.Server.Database.Models;

public class OAuthClient
{
    public OAuthClientProvider Id { get; set; }

    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }

    public string ClientId { get; set; }

    public string ClientSecret { get; set; }
}
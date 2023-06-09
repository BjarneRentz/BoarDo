namespace BoarDo.Server.Database.Models;

public class OAuthClient
{
	public string Id { get; set; }
	
	public string AccessToken { get; set; }
	public string RefreshToken { get; set; }
}
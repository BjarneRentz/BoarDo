namespace BoarDo.Server.Configs;

public class UrlConfig
{
	public const string Position = "UrlConfig";
	
	public required string BackendUrl { get; set; }
	
	public required string FrontendUrl { get; set; }
}
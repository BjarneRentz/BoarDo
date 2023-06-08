using BoarDo.Server.Configs;
using BoarDo.Server.Repos;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BoarDo.Server.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController : Controller
{
	private readonly GoogleAuthorizationCodeFlow _googleFlow;
	private readonly GoogleClientConfig _googleClientConfig;

	private readonly IAuthClientsRepo _authClientsRepo;

	public AuthController(IOptions<GoogleClientConfig> googleClient, IAuthClientsRepo authClientsRepo)
	{
		_googleClientConfig = googleClient.Value;
		_authClientsRepo = authClientsRepo;
		_googleFlow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
		{
			ClientSecrets = new ClientSecrets
			{
				ClientId = googleClient.Value.ClientId,
				ClientSecret = googleClient.Value.ClientSecret
			},
			Prompt = "consent",
			Scopes = new[] {"https://www.googleapis.com/auth/calendar.readonly"}
		});
	}

	[HttpGet]
	public async Task<ActionResult<List<string>>> GetConnectedClients()
	{
		return (await _authClientsRepo.GetClients()).Select(c => c.Id).ToList();
	}
	
		
	[HttpGet("Connect/Google")]
	public ActionResult ConnectGoogle()
	{
		var request = _googleFlow.CreateAuthorizationCodeRequest("https://localhost:7117/auth/callback/google");
		return Redirect(request.Build().ToString());
		
	}

	[HttpGet("Callback/Google")]
	public async Task<ActionResult> AuthCallback([FromQuery] string code)
	{
		var token = await _googleFlow.ExchangeCodeForTokenAsync(_googleClientConfig.ClientId, code,
			"https://localhost:7117/auth/callback/google", CancellationToken.None);

		await _authClientsRepo.AddGooleClient(token.AccessToken, token.RefreshToken);
		
		return Ok();
	}
}
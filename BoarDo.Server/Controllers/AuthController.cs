using System.Net.Mime;
using BoarDo.Server.Configs;
using BoarDo.Server.Repos;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BoarDo.Server.Controllers;

[Route("[controller]")]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
public class AuthController : Controller
{
    private readonly IAuthClientsRepo _authClientsRepo;
    private readonly GoogleClientConfig _googleClientConfig;
    private readonly GoogleAuthorizationCodeFlow _googleFlow;

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
            Scopes = new[] { "https://www.googleapis.com/auth/calendar.readonly" }
        });
    }

    /// <summary>
    /// Gets all clients and their state (connected or not).
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<Dictionary<string, bool>>> GetClients()
    {
        return (await _authClientsRepo.GetClientsAsync());
    }


    /// <summary>
    /// Entry point to start an server side google login.
    /// </summary>
    /// <returns></returns>
    [HttpGet("Connect/Google")]
    public ActionResult ConnectGoogle()
    {
        var request = _googleFlow.CreateAuthorizationCodeRequest("https://localhost:7117/auth/callback/google");
        return Redirect(request.Build().ToString());
    }

    /// <summary>
    /// Callback route for google login. Should not be called manually.
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    [HttpGet("Callback/Google")]
    public async Task<ActionResult> AuthCallback([FromQuery] string code)
    {
        var token = await _googleFlow.ExchangeCodeForTokenAsync(_googleClientConfig.ClientId, code,
            "https://localhost:7117/auth/callback/google", CancellationToken.None);

        await _authClientsRepo.AddGoogleClientAsync(token.AccessToken, token.RefreshToken, _googleClientConfig.ClientId,
            _googleClientConfig.ClientSecret);

        return Ok();
    }
}
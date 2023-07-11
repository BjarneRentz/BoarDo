using System.Net.Mime;
using BoarDo.Server.Configs;
using BoarDo.Server.Dtos;
using BoarDo.Server.Repos;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BoarDo.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
public class AuthController : Controller
{
    private readonly IAuthClientsRepo _authClientsRepo;
    private readonly GoogleClientConfig _googleClientConfig;
    private readonly GoogleAuthorizationCodeFlow _googleFlow;
    private const string RedirectUri = "https://localhost:7117/api/auth/callback/google";

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
    public ActionResult<UrlResult> ConnectGoogle()
    {
        var request = _googleFlow.CreateAuthorizationCodeRequest(RedirectUri);
        return Ok(new UrlResult{Url = request.Build().ToString()});
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
            RedirectUri, CancellationToken.None);

        await _authClientsRepo.AddOrUpdateGoogleClientAsync(token.AccessToken, token.RefreshToken, _googleClientConfig.ClientId,
            _googleClientConfig.ClientSecret);

        return Redirect("http://localhost:5173/settings");
    }
}
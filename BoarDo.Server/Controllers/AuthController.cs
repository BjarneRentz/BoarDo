using System.Net.Mime;
using BoarDo.Server.Configs;
using BoarDo.Server.Database.Models;
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
    private const string ApplicationUrl = "https://localhost:8080";
    private const string GoogleCallbackPath = "/api/auth/callback/google";

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
    public async Task<ActionResult<Dictionary<OAuthClientProvider, bool>>> GetClients()
    {
        return (await _authClientsRepo.GetClientsAsync());
    }

    [HttpGet("Supported")]
    public ActionResult<List<OAuthClientProvider>> GetSupportedClients()
    {
        return Ok(new List<OAuthClientProvider> { OAuthClientProvider.Google, OAuthClientProvider.TickTick });
    }


    /// <summary>
    /// Entry point to start an server side google login.
    /// </summary>
    /// <returns></returns>
    [HttpGet("Connect/{provider}")]
    public ActionResult<UrlResult> ConnectGoogle(OAuthClientProvider provider)
    {
        if (provider == OAuthClientProvider.Google)
        {
            var request = _googleFlow.CreateAuthorizationCodeRequest(ApplicationUrl + GoogleCallbackPath);
            return Ok(new UrlResult{Url = request.Build().ToString()});    
        }

        throw new NotSupportedException();


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
            ApplicationUrl + GoogleCallbackPath, CancellationToken.None);

        await _authClientsRepo.AddOrUpdateGoogleClientAsync(token.AccessToken, token.RefreshToken, _googleClientConfig.ClientId,
            _googleClientConfig.ClientSecret);

        return Redirect(ApplicationUrl + "/settings");
    }

    [HttpDelete("{provider}")]
    public async Task<ActionResult> RemoveClient(OAuthClientProvider provider)
    {
        var result = await _authClientsRepo.DeleteClientAsync(provider);

        return result ? Ok() : BadRequest();
    }
}
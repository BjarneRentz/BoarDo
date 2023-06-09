using BoarDo.Server.Repos;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;

namespace BoarDo.Server.Services;

public class GoogleCalendarService
{
    private readonly IAuthClientsRepo _authClientsRepo;

    private readonly ILogger<GoogleCalendarService> _logger;
    private GoogleCredential? _googleCredential;


    public GoogleCalendarService(IAuthClientsRepo authClientsRepo, ILogger<GoogleCalendarService> logger)
    {
        _authClientsRepo = authClientsRepo;
        _logger = logger;
    }

    public async Task<Events> GetEvents()
    {
        _googleCredential ??= await CreateGoogleCredential();

        var calendarApi = new CalendarService(new BaseClientService.Initializer
        {
            HttpClientInitializer = _googleCredential
        });

        var request = calendarApi.Events.List("primary");
        request.TimeMin = DateTime.Today;
        request.TimeMax = DateTime.Today + TimeSpan.FromDays(7);
        request.SingleEvents = true;
        request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

        _logger.LogDebug("Requesting events");

        return await request.ExecuteAsync();
    }

    private async Task<GoogleCredential> CreateGoogleCredential()
    {
        var client = await _authClientsRepo.GetGoogleAccessTokenAsync();

        if (client != null)
            return GoogleCredential.FromJsonParameters(new JsonCredentialParameters
            {
                RefreshToken = client.RefreshToken, ClientId = client.ClientId,
                ClientSecret = client.ClientSecret, Type = JsonCredentialParameters.AuthorizedUserCredentialType
            });
        _logger.LogWarning("No Google client is currently registered!");
        throw new Exception();
    }
}
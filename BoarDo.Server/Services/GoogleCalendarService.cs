using BoarDo.Server.Configs;
using BoarDo.Server.Jobs;
using BoarDo.Server.Repos;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Microsoft.Extensions.Options;
using Quartz;

namespace BoarDo.Server.Services;

public class GoogleCalendarService
{
    private readonly IAuthClientsRepo _authClientsRepo;

    private readonly ILogger<GoogleCalendarService> _logger;
    private readonly IRenderService _renderService;
    private readonly ISchedulerFactory _schedulerFactory;
    private readonly SyncSettings _syncSettings;
    
    private GoogleCredential? _googleCredential;


    public GoogleCalendarService(IAuthClientsRepo authClientsRepo, IRenderService renderService,
        ILogger<GoogleCalendarService> logger, ISchedulerFactory schedulerFactoryFactory, IOptions<SyncSettings> syncSettings)
    {
        _authClientsRepo = authClientsRepo;
        _renderService = renderService;
        _schedulerFactory = schedulerFactoryFactory;
        _syncSettings = syncSettings.Value;
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
        request.TimeMax = DateTime.Today + TimeSpan.FromDays(3);
        request.SingleEvents = true;
        request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

        _logger.LogDebug("Requesting events");

        var events = await request.ExecuteAsync();
        _renderService.RenderEvents(events.Items.ToList());

        return events;
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

    public async Task ToggleSync(bool enable)
    {
        var scheduler = await _schedulerFactory.GetScheduler();

        if (enable)
        {
            _logger.LogInformation("Enabling Calendar Sync");
            var detail = await scheduler.GetJobDetail(CalenderJob.Key);

            if (detail != null)
            {
                // Triggered job is already present;
                return;
            }
            var jobDetail = JobBuilder.Create<CalenderJob>().WithIdentity(CalenderJob.Key).Build();
            var cronTrigger = TriggerBuilder.Create().WithCronSchedule(_syncSettings.Calendar)
                .ForJob(CalenderJob.Key).StartNow()
                .Build();
            
            await scheduler.ScheduleJob(jobDetail, cronTrigger);
            // Trigger job instantly.
            await scheduler.TriggerJob(CalenderJob.Key);
        }
        else
        {
            _logger.LogInformation("Disabling Calendar Sync");
            await scheduler.DeleteJob(CalenderJob.Key);
        }
    }
}
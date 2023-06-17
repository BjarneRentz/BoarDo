using BoarDo.Server.Services;
using Quartz;

namespace BoarDo.Server.Jobs;

public class CalenderJob :IJob
{
	public static readonly JobKey Key = new ("calendar-job", "calendar-job-group");
	
	private readonly ILogger<CalenderJob> _logger;
	private readonly GoogleCalendarService _calendarService;
	private readonly IRenderService _renderService;

	public CalenderJob(ILogger<CalenderJob> logger, GoogleCalendarService calendarService, IRenderService renderService)
	{
		_logger = logger;
		_calendarService = calendarService;
		_renderService = renderService;
	}
	
	public async Task Execute(IJobExecutionContext context)
	{
		_logger.LogInformation("Requesting Events");

		var events = (await _calendarService.GetEvents()).Items.ToList();
		_logger.LogInformation("Received {0} events", events.Count);
		
		_renderService.RenderEvents(events);

	}
}
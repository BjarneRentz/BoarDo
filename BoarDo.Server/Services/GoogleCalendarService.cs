using Google.Apis.Calendar.v3;
using Google.Apis.Services;

namespace BoarDo.Server.Services;

public class GoogleCalendarService
{
	public async Task<Google.Apis.Calendar.v3.Data.Events> GetEvents()
	{
		var calendarApi = new CalendarService(new BaseClientService.Initializer()
		{
			
		});

		var request = calendarApi.Events.List("primary");
		request.TimeMin = DateTime.Today;
		request.TimeMax = DateTime.Today + TimeSpan.FromDays(7);
		request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

		var events = await request.ExecuteAsync();
		return events;
	}
}
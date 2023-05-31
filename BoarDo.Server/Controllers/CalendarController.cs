using BoarDo.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace BoarDo.Server.Controllers;

[Route("[controller]")]
[ApiController]
public class CalendarController : Controller
{
	private GoogleCalendarService _calendarService;

	public CalendarController(GoogleCalendarService calendarService)
	{
		_calendarService = calendarService;
	}
	[HttpGet]
	public async Task<ActionResult<List<Google.Apis.Calendar.v3.Data.Events>>> GetEvents()
	{
		return Ok(await _calendarService.GetEvents());
	}
}
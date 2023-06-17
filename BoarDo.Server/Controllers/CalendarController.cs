using BoarDo.Server.Services;
using Google.Apis.Calendar.v3.Data;
using Microsoft.AspNetCore.Mvc;

namespace BoarDo.Server.Controllers;

[Route("[controller]")]
[ApiController]
public class CalendarController : Controller
{
    private readonly GoogleCalendarService _calendarService;

    public CalendarController(GoogleCalendarService calendarService)
    {
        _calendarService = calendarService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Events>>> GetEvents()
    {
        return Ok(await _calendarService.GetEvents());
    }

    [HttpPost("Sync/{enable:bool}")]
    public async Task<ActionResult> ToggleAutoSync(bool enable)
    {
        await _calendarService.ToggleSync(enable);

        return Ok();
    }
}
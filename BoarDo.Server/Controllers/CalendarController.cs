using System.Net.Mime;
using BoarDo.Server.Services;
using Google.Apis.Calendar.v3.Data;
using Microsoft.AspNetCore.Mvc;

namespace BoarDo.Server.Controllers;

[Route("[controller]")]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]

public class CalendarController : Controller
{
    private readonly GoogleCalendarService _calendarService;

    public CalendarController(GoogleCalendarService calendarService)
    {
        _calendarService = calendarService;
    }

    /// <summary>
    /// Returns all events and also renders them on the screen.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<List<Events>>> GetEvents()
    {
        return Ok(await _calendarService.GetEvents());
    }

    /// <summary>
    /// Activates or disables the automatic background fetching and displaying of the events.
    /// </summary>
    /// <param name="enable">Weather enable or disable the sync</param>
    /// <returns></returns>
    [HttpPost("Sync/{enable:bool}")]
    public async Task<ActionResult> ToggleAutoSync(bool enable)
    {
        await _calendarService.ToggleSync(enable);

        return Ok();
    }
}
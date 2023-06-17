using System.Net.Mime;
using BoarDo.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace BoarDo.Server.Controllers;

[Route("[controller]")]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]

public class ScreenController : Controller
{
    private readonly IRenderService _renderService;

    public ScreenController(IRenderService renderService)
    {
        _renderService = renderService ?? throw new ArgumentNullException(nameof(renderService));
    }

    /// <summary>
    /// Returns the currently rendered screen.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public ActionResult GetCurrentScreen()
    {
        return File(_renderService.CurrentScreen, "image/jpeg");
    }
    
}
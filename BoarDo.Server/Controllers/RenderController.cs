﻿using BoarDo.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace BoarDo.Server.Controllers;

[Route("[controller]")]
[ApiController]
public class RenderController : Controller
{
	private readonly IRenderService _renderService;

	public RenderController(IRenderService renderService)
	{
		_renderService = renderService ?? throw new ArgumentNullException(nameof(renderService));
	}

	[HttpGet("Screen")]
	public ActionResult GetCurrentScreen()
	{
		return File(_renderService.CurrentScreen, "image/jpeg");
	}

	
	[HttpPost("RenderText/${name}")]
	public ActionResult RenderName(string name)
	{
		_renderService.RenderHeader(name);

		return Ok();
	}
}
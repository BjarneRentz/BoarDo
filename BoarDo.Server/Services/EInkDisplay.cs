using BoarDo.Server.Events;
using Waveshare;
using Waveshare.Devices;
using Waveshare.Interfaces;

namespace BoarDo.Server.Services;

public class EInkDisplay : IHostedService
{
	private readonly IEPaperDisplay _display;
	private readonly IRenderService _renderService;

	public EInkDisplay(IRenderService renderService)
	{
		_display = EPaperDisplay.Create(EPaperDisplayType.WaveShare7In5_V2);
		_renderService = renderService;
	}

	private void OnScreenChanged(object? sender, ScreenChangedEventArgs args)
	{
		_display.PowerOn();
		_display.DisplayImage(new(_renderService.CurrentScreen));
		_display.PowerOff();
	}

	public Task StartAsync(CancellationToken cancellationToken)
	{
		_display.Clear();
		_display.WaitUntilReady();		
		_renderService.ScreenChanged += OnScreenChanged;

		return Task.CompletedTask;
	}

	public Task StopAsync(CancellationToken cancellationToken)
	{
		_display.PowerOff();
		_display.Dispose();
		_renderService.ScreenChanged -= OnScreenChanged;

		return Task.CompletedTask;
	}
}
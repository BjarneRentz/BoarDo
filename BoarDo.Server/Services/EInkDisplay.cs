using System.Drawing;
using BoarDo.Server.ScreenEvents;
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

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _display.Clear();
        _display.WaitUntilReady();
        _renderService.ScreenChanged += OnScreenChanged;

        _display.PowerOn();
        _display.DisplayImage(new Bitmap(_renderService.CurrentScreen));
        _display.PowerOff();

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _display.PowerOff();
        _display.Dispose();
        _renderService.ScreenChanged -= OnScreenChanged;

        return Task.CompletedTask;
    }

    private void OnScreenChanged(object? sender, ScreenChangedEventArgs args)
    {
        _display.PowerOn();
        _display.DisplayImage(new Bitmap(_renderService.CurrentScreen));
        _display.PowerOff();
    }
}
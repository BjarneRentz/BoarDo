using System.Net;
using System.Net.Sockets;
using BoarDo.Server.ScreenEvents;
using SkiaSharp;

namespace BoarDo.Server.Services;

public class RenderService : IRenderService, IDisposable
{
    private const int ScreenWidth = 800;
    private const int ScreenHeight = 480;
    private readonly SKCanvas _canvas;
    private readonly SKBitmap _screen;


    public RenderService()
    {
        _screen = new SKBitmap(ScreenWidth, ScreenHeight);
        _canvas = new SKCanvas(_screen);
        RenderInitialScreen();
    }

    public void Dispose()
    {
        _screen.Dispose();
        _canvas.Dispose();
    }

    public event EventHandler<ScreenChangedEventArgs>? ScreenChanged;
    public Stream CurrentScreen => GetScreen();


    public void RenderHeader(string name)
    {
        _canvas.Clear(SKColors.White);

        _canvas.DrawLine(20.0f, 56.0f, ScreenWidth - 20, 56.0f, new SKPaint { Color = SKColors.Black });
        RenderText(name, ScreenWidth / 2.0f, 50, 48.0f);

        var args = new ScreenChangedEventArgs
        {
            X = 20,
            Y = 0,
            Width = 440,
            Height = 56
        };

        OnScreenChanged(args);
    }


    private MemoryStream GetScreen()
    {
        var stream = new MemoryStream();
        _screen.Encode(stream, SKEncodedImageFormat.Jpeg, 100);
        stream.Position = 0;
        return stream;
    }

    /// <summary>
    ///     Renders the initial screen that displays the Ip-Address.
    /// </summary>
    private void RenderInitialScreen()
    {
        _canvas.Clear(SKColors.White);

        RenderText("Hey there", ScreenWidth / 2.0f, 40, 48.0f);
        RenderText("To connect, enter this IP in the App", ScreenWidth / 2.0f, 70, 24.0f);
        RenderText(GetLocalIpAddress(), ScreenWidth / 2.0f, 150, 48.0f);

        var args = new ScreenChangedEventArgs
        {
            X = 0,
            Y = 0,
            Width = ScreenWidth,
            Height = ScreenHeight
        };
        OnScreenChanged(args);
    }

    private void RenderText(string text, float x, float y, float fontSize, SKTextAlign textAlign = SKTextAlign.Center,
        bool striked = false)
    {
        using var paint = new SKPaint
        {
            Color = SKColors.Black,
            IsAntialias = true,
            TextSize = fontSize,
            TextAlign = textAlign
        };

        _canvas.DrawText(text, x, y, paint);
        if (!striked) return;
        var textBounds = new SKRect();
        var textWidth = paint.MeasureText(text, ref textBounds);
        _canvas.DrawLine(20, y - textBounds.Size.Height / 2.0f, 20 + textWidth,
            y - textBounds.Size.Height / 2.0f,
            new SKPaint
            {
                Color = SKColors.Black, IsAntialias = true, Style = SKPaintStyle.StrokeAndFill, StrokeWidth = 4,
                StrokeCap = SKStrokeCap.Round
            });
    }

    private string GetLocalIpAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
            if (ip.AddressFamily == AddressFamily.InterNetwork)
                return ip.ToString();
        throw new Exception("No network adapters with an IPv4 address in the system!");
    }

    protected virtual void OnScreenChanged(ScreenChangedEventArgs e)
    {
        var handler = ScreenChanged;
        handler?.Invoke(this, e);
    }
}
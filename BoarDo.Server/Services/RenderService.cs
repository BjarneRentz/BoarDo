using System.Net;
using System.Net.Sockets;
using BoarDo.Server.ScreenEvents;
using Google.Apis.Calendar.v3.Data;
using SkiaSharp;

namespace BoarDo.Server.Services;

public sealed class RenderService : IRenderService, IDisposable
{
    private const int ScreenWidth = 800;
    private const int ScreenHeight = 480;
    private const int CalendarStartX = 300;
    private const int CalendarStartY = 60;
    private const int CalendarDayWidth = 165;
    private const int TextWidthPerDay = 160;
    private const int SpaceBetweenEvents = 10;
    private const int DaySpan = 2;
    private readonly SKCanvas _canvas;
    private readonly SKBitmap _screen;
    
    private readonly SKPaint _paint = new()
    {
        Color = SKColors.Black,
        IsAntialias = false,
        TextSize = 16,
        TextAlign = SKTextAlign.Left,
        TextEncoding = SKTextEncoding.Utf8,
    };

    private bool _firstChangedRendered;


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
        _paint.Dispose();
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

    public void RenderEvents(List<Event> events)
    {
        ClearArea(CalendarStartX, 0, ScreenWidth, ScreenHeight);
        var renderedEventsPerDay = new Dictionary<DateTime, int>();

        foreach (var calendarEvent in events)
        {
            if (!calendarEvent.Start.DateTime.HasValue)
            {
                // All day Event
                var eventStart = DateTime.Parse(calendarEvent.Start.Date).Date;
                var eventEnd = DateTime.Parse(calendarEvent.End.Date).Date;

                var today = DateTime.Today.Date;
                var endRangeDay = today + TimeSpan.FromDays(DaySpan);

                var renderStartDate = eventStart > today ? eventStart : today;
                var renderEndDate = eventEnd < endRangeDay ? eventEnd : endRangeDay;


                while (renderStartDate <= renderEndDate)
                {
                    RenderEvent(calendarEvent, renderStartDate, renderedEventsPerDay);
                    renderStartDate = renderStartDate.AddDays(1);
                }
                

            }
            else
            {
                RenderEvent(calendarEvent, calendarEvent.Start.DateTime.Value, renderedEventsPerDay);
            }
        }

        if (renderedEventsPerDay.Keys.Count != 3)
        {
            // Get Missing days
            var expectedDays = new[] { 0, 1, 2 }.Select(i => DateTime.Today + TimeSpan.FromDays(i));
            var missingDays = expectedDays.Except(renderedEventsPerDay.Keys);

            // Render Missing days
            foreach (var missingDay in missingDays)
            {
                RenderDate(missingDay);
                RenderEmptyDate(missingDay);
            }
        }
        
        OnScreenChanged(new ScreenChangedEventArgs());
    }

    private void RenderEmptyDate(DateTime missingDay)
    {
        var dayDifference = (missingDay.Date - DateTime.Today).Days;
        RenderText("Keine Termine", CalendarStartX + dayDifference * CalendarDayWidth, CalendarStartY, 16, SKTextAlign.Left);
    }

    private int RenderEvent(Event calendarEvent, DateTime date, Dictionary<DateTime, int> renderedDays)
    {

        date = date.Date;

        var dayDifference = (date - DateTime.Today.Date).Days;
        var x = CalendarStartX + dayDifference* CalendarDayWidth;

        var hasKey = renderedDays.TryGetValue(date, out var renderedHeight);
        if (!hasKey)
        {
            renderedHeight = 0;
            renderedDays[date] = 0;
            RenderDate(date);
        }
        
        var y = CalendarStartY  + renderedHeight;
        
        var height = 0;
        
        var text = calendarEvent.Summary;
        if (calendarEvent.Start.DateTime.HasValue)
        {
            text = $"{calendarEvent.Start.DateTime.Value:t} {text}";
        }
        
        while (text.Length != 0)
        {
            var drawableLength = _paint.BreakText(text, TextWidthPerDay);
            if (drawableLength > text.Length)
            {
                drawableLength = text.Length;
            }
            _canvas.DrawText(text.Substring(0,Convert.ToInt32(drawableLength)), x, y + height, _paint);
            text = text[Convert.ToInt32(drawableLength)..];
            height += 20;
        }

        renderedDays[date] += height + SpaceBetweenEvents;

        return height;
        
    }

    private void RenderDate(DateTime date)
    {
        var text = DateTime.Today == date.Date ? "Heute" : date.ToString("d MMM");
        var dayDifference = (date.Date - DateTime.Today).Days;
        RenderText(text, CalendarStartX + dayDifference * CalendarDayWidth, 30, 28.0f, SKTextAlign.Left);
    }


    /// <summary>
    ///     Clears the specified area by drawing a white rect on it.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="top"></param>
    /// <param name="right"></param>
    /// <param name="bottom"></param>
    private void ClearArea(int left, int top, int right, int bottom)
    {
        if (!_firstChangedRendered)
        {
            _canvas.Clear(SKColors.White);
            _firstChangedRendered = true;
        }
        else
        {
            var rect = new SKRect(left, top, right, bottom);
            var paint = new SKPaint { Color = SKColors.White, Style = SKPaintStyle.Fill };
            _canvas.DrawRect(rect, paint);
        }
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
        bool underlined = false)
    {

        _canvas.DrawText(text, x, y, _paint);
        if (!underlined) return;
        var textBounds = new SKRect();
        var textWidth = _paint.MeasureText(text, ref textBounds);
        _canvas.DrawLine(x, y + textBounds.Size.Height, x + textWidth,
            y + textBounds.Size.Height,
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

    private void OnScreenChanged(ScreenChangedEventArgs e)
    {
        var handler = ScreenChanged;
        handler?.Invoke(this, e);
    }
}
using BoarDo.Server.ScreenEvents;
using Google.Apis.Calendar.v3.Data;

namespace BoarDo.Server.Services;

public interface IRenderService
{
    /// <summary>
    ///     Represents the current Screen used by the display. This Image is rotated
    /// </summary>
    public Stream CurrentScreen { get; }

    public void RenderHeader(string name);
    public void RenderEvents(List<Event> events);


    /// <summary>
    ///     Gets triggered whenever the underlying screen changess
    /// </summary>
    public event EventHandler<ScreenChangedEventArgs> ScreenChanged;
}
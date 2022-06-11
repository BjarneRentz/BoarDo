using BoarDo.Server.Events;

namespace BoarDo.Server.Services;

public interface IRenderService
{
	public event EventHandler<ScreenChangedEventArgs> ScreenChanged;
	
	/// <summary>
	/// Represents the current Screen used by the display. This Image is rotated
	/// </summary>
	public Stream CurrentScreen { get; }
	
	/// <summary>
	///		Represents the current Screen without correct rotation.
	/// </summary>
	public Stream CurrentDebugScreen { get; }
}

namespace BoarDo.Server.Events;

/// <summary>
///     Contains information about the part (rectangle) where the change occured and therefore what to update.
/// </summary>
public class ScreenChangedEventArgs
{
	/// <summary>
	///     X coordinate
	/// </summary>
	public int X { get; set; }

	/// <summary>
	///     Y coordinate
	/// </summary>
	public int Y { get; set; }

	/// <summary>
	///     Width of the area that changed.
	/// </summary>
	public int Width { get; set; }

	/// <summary>
	///     Height of the changed area.
	/// </summary>
	public int Height { get; set; }
}
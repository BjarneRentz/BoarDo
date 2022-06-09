using BoarDo.Server.Events;

namespace BoarDo.Server.Services;

public class IRenderService
{
	public event EventHandler<ScreenChangedEventArgs> ScreenChanged;
}

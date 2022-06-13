using BoarDo.Server.Models;

namespace BoarDo.Server.Events;

public class ToDoSetChangedEventArgs : EventArgs
{
	public ToDoSet Set { get; set; }
}
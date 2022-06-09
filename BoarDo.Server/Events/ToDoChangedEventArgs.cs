using BoarDo.Server.Models;

namespace BoarDo.Server.Events;

public class ToDoChangedEventArgs : EventArgs
{
	public ToDo ToDo { get; set; }
}
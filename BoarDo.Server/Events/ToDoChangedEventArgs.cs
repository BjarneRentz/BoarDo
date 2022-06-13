using BoarDo.Server.Models;

namespace BoarDo.Server.Events;

public class ToDoChangedEventArgs : EventArgs
{
	public bool IsNew { get; set; }
	public ToDo ToDo { get; set; }
}
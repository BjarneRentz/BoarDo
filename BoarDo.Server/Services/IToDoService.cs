using BoarDo.Server.Events;
using BoarDo.Server.Models;

namespace BoarDo.Server.Services;

public interface IToDoService
{
	public ToDoSet? ToDoSet { get; }

	public event EventHandler<ToDoChangedEventArgs> ToDoChanged;

	public event EventHandler<ToDoSetChangedEventArgs> ToDoSetChanged;

	public bool SyncToDoSet(ToDoSet set);

	public bool SyncToDo(ToDo toDo);
}
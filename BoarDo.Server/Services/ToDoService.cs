using BoarDo.Server.Events;
using BoarDo.Server.Models;

namespace BoarDo.Server.Services;

public class ToDoService : IToDoService
{
	public ToDoSet? ToDoSet { get; private set; }

	public event EventHandler<ToDoChangedEventArgs>? ToDoChanged;
	public event EventHandler<ToDoSetChangedEventArgs>? ToDoSetChanged;


	public bool SyncToDoSet(ToDoSet set)
	{
		ToDoSet = set;
		var args = new ToDoSetChangedEventArgs { Set = set };
		OnToDoSetChanged(args);
		return true;
	}

	public bool SyncToDo(ToDo todo)
	{
		if (ToDoSet == null)
			return false;

		var trackedToDo = ToDoSet.ToDos.Find(t => t.Id == todo.Id);

		ToDoChangedEventArgs args;

		if (trackedToDo != null)
		{
			trackedToDo.Title = todo.Title;
			trackedToDo.Completed = todo.Completed;
			args = new ToDoChangedEventArgs { ToDo = trackedToDo, IsNew = false };
		}
		else
		{
			ToDoSet.ToDos.Add(todo);
			args = new ToDoChangedEventArgs { ToDo = todo, IsNew = true };
		}

		OnToDoChanged(args);

		return true;
	}

	protected virtual void OnToDoSetChanged(ToDoSetChangedEventArgs e)
	{
		var handler = ToDoSetChanged;
		handler?.Invoke(this, e);
	}

	protected virtual void OnToDoChanged(ToDoChangedEventArgs e)
	{
		var handler = ToDoChanged;
		handler?.Invoke(this, e);
	}
}
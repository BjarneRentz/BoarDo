using BoarDo.Server.Events;
using BoarDo.Server.Models;

namespace BoarDo.Server.Services;

public class ToDoService : IToDoService
{
	public ToDoSet? ToDoSet { get; private set; }

	public event EventHandler<ToDoChangedEventArgs>? ToDoChanged;
	public event EventHandler? ToDoSetChanged;


	public bool SyncToDoSet(ToDoSet set)
	{
		ToDoSet = set;
		OnToDoSetChanged();
		return true;
	}

	public bool SyncToDo(ToDo todo)
	{
		if (ToDoSet == null)
			return false;

		var trackedToDo = ToDoSet.ToDos.Find(t => t.Id == todo.Id);
		
		if (trackedToDo != null)
		{
			trackedToDo.Title = todo.Title;
			trackedToDo.Completed = todo.Completed;
		}
		else
		{
			ToDoSet.ToDos.Add(todo);
		}
		
		var args = new ToDoChangedEventArgs { ToDo = todo };
		OnToDoChanged(args);

		return true;

	}

	protected virtual void OnToDoSetChanged()
	{
		var handler = ToDoSetChanged;
		handler?.Invoke(this, EventArgs.Empty);
	}

	protected virtual void OnToDoChanged(ToDoChangedEventArgs e)
	{
		var handler = ToDoChanged;
		handler?.Invoke(this, e);
	}
}
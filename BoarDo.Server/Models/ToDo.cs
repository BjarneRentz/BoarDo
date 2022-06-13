namespace BoarDo.Server.Models;

/// <summary>
///     Represents a ToDo.
/// </summary>
public class ToDo
{
	public int Id { get; set; }

	public string Title { get; set; }

	public bool Completed { get; set; }
}
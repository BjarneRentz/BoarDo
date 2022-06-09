using System.ComponentModel.DataAnnotations;

namespace BoarDo.Server.Models;

/// <summary>
///		A ToDoSet is a set of ToDos that are displayed at once.
/// </summary>
public class ToDoSet
{
	[Required]
	public string Name { get; set; }

	[MinLength(1)]
	public List<ToDo> ToDos { get; set; }= new();
}
using BoarDo.Server.Models;
using BoarDo.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace BoarDo.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class ToDoController : Controller
{

	private readonly IToDoService _toDoService;

	public ToDoController(IToDoService toDoService)
	{
		_toDoService = toDoService ?? throw new ArgumentNullException(nameof(toDoService));
	}
	
	/// <summary>
	///		Returns the current ToDoSet.
	/// </summary>
	/// <returns></returns>
	[HttpGet("Set")]
	public ActionResult<ToDoSet?> GetToDoSet()
	{
		return Ok(_toDoService.ToDoSet);
	}
	
	/// <summary>
	///		Sets the new active ToDoSet
	/// </summary>
	/// <param name="set"></param>
	/// <returns></returns>
	[HttpPost("Set")]
	public ActionResult SetToDoSet([FromBody] ToDoSet set)
	{
		var result = _toDoService.SyncToDoSet(set);

		return result ? Ok() : BadRequest();
	}

	/// <summary>
	///		Updates the given ToDo.
	/// </summary>
	/// <param name="todo"></param>
	/// <returns></returns>
	[HttpPost]
	public ActionResult SetToDo([FromBody] ToDo todo)
	{
		var result = _toDoService.SyncToDo(todo);
		return result ? Ok() : BadRequest();
	}
}
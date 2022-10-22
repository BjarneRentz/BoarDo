using System.Net;
using System.Net.Sockets;
using BoarDo.Server.Events;
using BoarDo.Server.Models;
using SkiaSharp;

namespace BoarDo.Server.Services;

public class RenderService : IRenderService, IDisposable
{
	private readonly SKCanvas _canvas;
	private readonly SKBitmap _screen;

	private readonly IToDoService _toDoService;

	private const int ScreenWidth = 480;
	private const int ScreenHeight = 800;
	

	public RenderService(IToDoService toDoService)
	{
		_screen = new SKBitmap(ScreenWidth, ScreenHeight);
		_canvas = new SKCanvas(_screen);
		RenderInitialScreen();
		toDoService.ToDoSetChanged += OnToDoSetChanged;
		toDoService.ToDoChanged += OnToDoChanged;
		_toDoService = toDoService;
	}

	public void Dispose()
	{
		_screen.Dispose();
		_canvas.Dispose();
	}

	public event EventHandler<ScreenChangedEventArgs>? ScreenChanged;
	public Stream CurrentScreen => GetScreen();
	public Stream CurrentDebugScreen => GetDebugScreen();

	private void OnToDoSetChanged(object? sender, ToDoSetChangedEventArgs args)
	{
		_canvas.Clear(SKColors.White);
		RenderHeader(args.Set.Name);
		RenderToDos(args.Set.ToDos);
	}

	private void OnToDoChanged(object? sender, ToDoChangedEventArgs args)
	{
		if (args.IsNew)
		{
			// Render new Entry at the end of the list.
			if (_toDoService.ToDoSet == null)
				return;
			RenderToDo(args.ToDo, _toDoService.ToDoSet.ToDos.Count - 1);
		}
		else
		{
			// Rerender updated version
			if (_toDoService.ToDoSet == null)
				return;
			var pos = _toDoService.ToDoSet.ToDos.IndexOf(args.ToDo);
			RenderToDo(args.ToDo, pos);
		}
	}

	private void RenderHeader(string name)
	{
		_canvas.DrawLine(20.0f, 56.0f, 460.0f, 56.0f, new SKPaint { Color = SKColors.Black });
		RenderText(name, ScreenWidth / 2.0f, 50, 48.0f);

		var args = new ScreenChangedEventArgs
		{
			X = 20,
			Y = 0,
			Width = 440,
			Height = 56
		};

		OnScreenChanged(args);
	}

	private void RenderToDos(List<ToDo> todos)
	{
		for (var i = 0; i < todos.Count; i++)
			RenderToDo(todos[i], i);
	}

	private void RenderToDo(ToDo todo, int position)
	{
		var y = 100 + 50 * position;
		
		RenderText(todo.Title, 20,y, 32.0f, SKTextAlign.Left, todo.Completed);
		
		
		var args = new ScreenChangedEventArgs
		{
			X = 0,
			Y = y,
			Width = ScreenWidth,
			Height = 50
		};

		OnScreenChanged(args);
	}


	private MemoryStream GetScreen()
	{
		using SKBitmap landscapeBitmap = new(ScreenHeight, ScreenWidth, _screen.ColorType, _screen.AlphaType, _screen.ColorSpace);
		using var landscapeCanvas = new SKCanvas(landscapeBitmap);
		landscapeCanvas.Translate(0, ScreenWidth);
		landscapeCanvas.RotateDegrees(270);
		landscapeCanvas.DrawBitmap(_screen, 0, 0);


		var stream = new MemoryStream();
		landscapeBitmap.Encode(stream, SKEncodedImageFormat.Jpeg, 100);
		stream.Position = 0;
		return stream;
	}

	private MemoryStream GetDebugScreen()
	{
		var stream = new MemoryStream();
		_screen.Encode(stream, SKEncodedImageFormat.Jpeg, 100);
		stream.Position = 0;
		return stream;
	}

	/// <summary>
	///     Renders the initial screen that displays the Ip-Address.
	/// </summary>
	private void RenderInitialScreen()
	{
		_canvas.Clear(SKColors.White);
		
		RenderText("Hey there", ScreenWidth / 2.0f, 40, 48.0f);
		RenderText("To connect, enter this IP in the App", ScreenWidth / 2.0f, 70,24.0f);
		RenderText(GetLocalIPAddress(), ScreenWidth / 2.0f, 150,48.0f);
		
	}

	private void RenderText(string text, float x, float y, float fontSize, SKTextAlign textAlign = SKTextAlign.Center,
		bool striked = false)
	{
		using var paint = new SKPaint
		{
			Color = SKColors.Black,
			IsAntialias = true,
			TextSize = fontSize,
			TextAlign = textAlign,
		};
		
		_canvas.DrawText(text, x,y,paint);
		if (striked)
		{
			var textBounds = new SKRect();
			var textWidth = paint.MeasureText(text, ref textBounds);
			_canvas.DrawLine(20, y - textBounds.Size.Height / 2.0f, 20 + textWidth,
				y - textBounds.Size.Height / 2.0f,
				new SKPaint
				{
					Color = SKColors.Black, IsAntialias = true, Style = SKPaintStyle.StrokeAndFill, StrokeWidth = 4,
					StrokeCap = SKStrokeCap.Round
				});
		}
		
	}

	private string GetLocalIPAddress()
	{
		var host = Dns.GetHostEntry(Dns.GetHostName());
		foreach (var ip in host.AddressList)
			if (ip.AddressFamily == AddressFamily.InterNetwork)
				return ip.ToString();
		throw new Exception("No network adapters with an IPv4 address in the system!");
	}

	protected virtual void OnScreenChanged(ScreenChangedEventArgs e)
	{
		var handler = ScreenChanged;
		handler?.Invoke(this, e);
	}
}
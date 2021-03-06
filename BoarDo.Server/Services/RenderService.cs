using System.Net;
using System.Net.Sockets;
using BoarDo.Server.Events;
using BoarDo.Server.Models;
using SkiaSharp;
using Topten.RichTextKit;

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
		var rs = new RichString()
			.Alignment(TextAlignment.Center)
			.FontFamily("Quicksand")
			.Add(name, fontSize: 48.0f, fontWeight: 700);
		rs.Paint(_canvas, new SKPoint((ScreenHeight - rs.MeasuredWidth) / 2.0f, 0));

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
		var y = 60 + 50 * position;
		_canvas.DrawRect(0, y, ScreenWidth, 50, new SKPaint { Color = SKColors.White });
		var rs = new RichString().FontFamily("Quicksand").Alignment(TextAlignment.Left)
			.Add(todo.Title, fontSize: 32.0f);
		rs.Paint(_canvas, new SKPoint(20, y));
		if (todo.Completed)
			_canvas.DrawLine(20, y + 2 + rs.MeasuredHeight / 2.0f, 20 + rs.MeasuredWidth,
				y + 2 + rs.MeasuredHeight / 2.0f,
				new SKPaint
				{
					Color = SKColors.Black, IsAntialias = true, Style = SKPaintStyle.StrokeAndFill, StrokeWidth = 4,
					StrokeCap = SKStrokeCap.Round
				});

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
		using var canvas = new SKCanvas(_screen);
		canvas.Clear(SKColors.White);


		var rs = new RichString()
			.Alignment(TextAlignment.Center)
			.FontFamily("Quicksand")
			.MarginBottom(40)
			.Add("Hey there", fontSize: 48.0f)
			.Paragraph()
			.MarginBottom(20)
			.Add("To connect, enter this IP in the App", fontSize: 24.0f)
			.Paragraph()
			.Add(GetLocalIPAddress(), fontSize: 48.0f, underline: UnderlineStyle.Solid);

		rs.Paint(canvas, new SKPoint((ScreenWidth - rs.MeasuredWidth) / 2.0f, 100));
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
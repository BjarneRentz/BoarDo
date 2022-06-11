using System.Net;
using System.Net.Sockets;
using BoarDo.Server.Events;
using BoarDo.Server.Models;
using SkiaSharp;
using Topten.RichTextKit;

namespace BoarDo.Server.Services;

public class RenderService : IRenderService, IDisposable
{
	private readonly SKBitmap _screen;

	private readonly SKCanvas _canvas;
	
	public RenderService(IToDoService toDoService)
	{
		_screen = new SKBitmap(480, 800);
		_canvas = new SKCanvas(_screen);
		RenderInitialScreen();
		toDoService.ToDoSetChanged += OnToDoSetChanged;
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

	private void RenderHeader(string name)
	{
		_canvas.DrawLine(20.0f, 50.0f, 460.0f, 50.0f, new SKPaint{Color = SKColors.Black});
		var rs = new RichString()
			.Alignment(TextAlignment.Center)
			.FontFamily("Quicksand")
			.Add(name, fontSize: 48.0f, fontWeight: 100);
		rs.Paint(_canvas, new SKPoint((480 - rs.MeasuredWidth) / 2.0f, 0));
	}

	private void RenderToDos(List<ToDo> todos)
	{
		var y = 60.0f;

		foreach (var t in todos)
		{
			var rs = new RichString().FontFamily("Quicksand").Alignment(TextAlignment.Left)
				.Add(t.Title, fontSize:32.0f);
			rs.Paint(_canvas, new SKPoint(20, y));
			if(t.Completed)
				_canvas.DrawLine(20, y +2 + rs.MeasuredHeight / 2.0f, 20 + rs.MeasuredWidth,y +2 + rs.MeasuredHeight / 2.0f, new SKPaint{Color = SKColors.Black, IsAntialias = true, Style = SKPaintStyle.StrokeAndFill, StrokeWidth = 4, StrokeCap = SKStrokeCap.Round});
			y += rs.MeasuredHeight + 10;
		}
	}

	
	
	private MemoryStream GetScreen()
	{
		using SKBitmap landscapeBitmap = new(800, 480, _screen.ColorType, _screen.AlphaType, _screen.ColorSpace);
		using var landscapeCanvas = new SKCanvas(landscapeBitmap);
		landscapeCanvas.Translate(0, 480);
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
	///     Renders the initial screen that displays the Ip-Adress.
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

		rs.Paint(canvas, new SKPoint((480 - rs.MeasuredWidth) / 2.0f, 100));
	}

	private string GetLocalIPAddress()
	{
		var host = Dns.GetHostEntry(Dns.GetHostName());
		foreach (var ip in host.AddressList)
			if (ip.AddressFamily == AddressFamily.InterNetwork)
				return ip.ToString();
		throw new Exception("No network adapters with an IPv4 address in the system!");
	}

	public void Dispose()
	{
		_screen.Dispose();
		_canvas.Dispose();
	}
}
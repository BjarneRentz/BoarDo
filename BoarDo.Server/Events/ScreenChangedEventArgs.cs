﻿namespace BoarDo.Server.Events;

public class ScreenChangedEventArgs
{
	public int X { get; set; }
	
	public int Y { get; set; }
	
	public int Width { get; set; }
	
	public int Height { get; set; }
}
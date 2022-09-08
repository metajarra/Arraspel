using Godot;
using System;

public class PixelCost : Node2D
{
	[Export] public int startingPixels;
	public int pixels;
	
	private Action<EventParams> onPixelsListener;
	
	public override void _Ready(){
		onPixelsListener = new Action<EventParams>(ReducePixels);
		EventManager.StartListening("onPixels", onPixelsListener);
		
		pixels = startingPixels;
	}
	
	private void ReducePixels(EventParams pixelParams){
		int pixelCost = pixelParams.intParam[0];
		pixels -= pixelCost;
		GD.Print($"Current pixels: {pixels}");
	}
}

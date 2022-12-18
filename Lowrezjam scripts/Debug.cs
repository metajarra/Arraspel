using Godot;
using System;

public class Debug : TileMap
{
	public override void _Process(float delta){
		Vector2 mousePos = GetGlobalMousePosition();
		Vector2 mouseCoords = WorldToMap(mousePos);
		
		//GD.Print($"MC: {mouseCoords}");
		
		Vector2 mouseDistance = Vector2.Zero - mousePos;
		Vector2 mouseDistanceAbs = new Vector2(Mathf.Abs(mouseDistance.x), Mathf.Abs(mouseDistance.y));
		
		int pixelCost;
		if (mouseDistanceAbs.x > mouseDistanceAbs.y){
			pixelCost = (int)Mathf.Ceil(mouseDistanceAbs.x);
		} else {
			pixelCost = (int)Mathf.Ceil(mouseDistanceAbs.y);
		}
		
		//GD.Print($"PX: {pixelCost}");
		
		
	}
}

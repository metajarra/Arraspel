using Godot;
using System;

public class Grid : TileMap
{
	public Vector2 GetGridPos(Vector2 worldPos){
		return WorldToMap(worldPos);
	}
	
	public Vector2 GetWorldPos(Vector2 gridPos){
		return MapToWorld(gridPos);
	}
	
	public int GetPixelCost(Vector2 gridPos1, Vector2 gridPos2){
		Vector2 mouseDistance = gridPos2 - gridPos1;
		Vector2 mouseDistanceAbs = new Vector2(Mathf.Abs(mouseDistance.x), Mathf.Abs(mouseDistance.y));
		
		int pixelCost;
		if (mouseDistanceAbs.x > mouseDistanceAbs.y){
			pixelCost = (int)Mathf.Ceil(mouseDistanceAbs.x);
		} else {
			pixelCost = (int)Mathf.Ceil(mouseDistanceAbs.y);
		}
		
		return pixelCost;
	}
}

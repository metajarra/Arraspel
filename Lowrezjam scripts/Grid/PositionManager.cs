using Godot;
using System;
using System.Collections.Generic;

public class PositionManager : Node2D
{
	private Grid grid;
	private Node2D[,] objects;
	
	public override void _Ready(){
		grid = (Grid)(GetTree().Root.GetNode("Root/Grid/8x8 Tilemap"));
		objects = new Node2D[8, 8];
	}
	
	public void AddObject(Node2D obj){
		Vector2 objCoordinates = grid.GetGridPos(obj.GlobalPosition);
		objects[(int)objCoordinates.x, (int)objCoordinates.y] = obj;
		
		GD.Print($"Added object {obj.Name} at coordinates {objCoordinates}");
	}
	
	public Node2D GetObjectWithMouse(Vector2 mouseCoordinates){
		Vector2 coordinates = grid.GetGridPos(mouseCoordinates);
		return(objects[(int)coordinates.x, (int)coordinates.y]);
	}
	
	public Node2D GetObjectWithCoordinates(Vector2 coordinates){
		return(objects[(int)coordinates.x, (int)coordinates.y]);
	}
}

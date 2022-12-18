using Godot;
using System;

public class Unit : Node2D
{
	[Export] public string shipName;
	[Export] public string shipID;
	
	public Unit(){
		shipName = "default name";
		shipID = "default id";
	}
	
	public Unit(string _name, string _id){
		shipName = _name;
		shipID = _id;
	}
	
	public void UpdatePosition(){
		GD.Print($"Position updated. New position is: {GlobalPosition}");
	}
	
	public override bool Equals(object obj){
		Unit unit = (Unit)obj;
		if (unit == null)
			return false;
		if (unit.shipName == shipName && unit.shipID == shipID)
			return true;
		
		return false;
	}
}

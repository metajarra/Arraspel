using Godot;
using System;

public class Unit : Node2D
{
	[Export] public string shipName;
	[Export] public string shipID;
	
	private InfoPacket info;
	private Orders orders;
	
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
	
	public void ReceiveInfo(InfoPacket info){
		this.info = info;
		GD.Print($"Received info. Message: {info.GetInfo()}");
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

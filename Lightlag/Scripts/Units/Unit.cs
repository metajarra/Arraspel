using Godot;
using System;

public class Unit : Node2D
{
	protected InfoPacket info;
	protected Orders orders;
	
	[Export] public string shipName;
	[Export] public string shipID;
	[Export] public string shipNavy;
	[Export] public string shipCaptain;
	
	public virtual void ReceiveInfo(InfoPacket info){
		this.info = info;
	}
	
	public virtual void ReceiveOrders(Orders orders){
		this.orders = orders;
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

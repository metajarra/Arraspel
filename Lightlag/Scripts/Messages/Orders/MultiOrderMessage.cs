using Godot;
using System;

public class MultiOrderMessage : MultiTargetMessage
{
	[Export] private float TEMP_defaultMaxRange;
	
	public override void _Ready(){
		GetAreaOfEffect();
		Connect("area_entered", this, "OnAreaEntered");
		
		maxRange = TEMP_defaultMaxRange;
	}
	
	protected override void OnAreaEntered(Node area){
		Unit unit = area as Unit;
		if (unit == null) return;
		
		if (intendedTargets.Contains((Unit)area)){
			((Unit)area).ReceiveOrders(orders);
		}
	}
	
	public Orders ReadMessage(){
		return orders;
	}
}

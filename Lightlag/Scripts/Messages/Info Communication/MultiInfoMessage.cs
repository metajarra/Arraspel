using Godot;
using System;
using System.Collections.Generic;

public class MultiInfoMessage : MultiTargetMessage
{
	[Export] private float TEMP_defaultMaxRange;
	
	public override void _Ready(){
		GetAreaOfEffect();
		Connect("area_entered", this, "OnAreaEntered");
		
		maxRange = TEMP_defaultMaxRange;
	}
	
	protected override void OnAreaEntered(Node2D area){
		if (intendedTargets.Contains((Unit)area)){
			((Unit)area).ReceiveInfo(info);
		}
	}
	
	public InfoPacket ReadMessage(){
		return info;
	}
}

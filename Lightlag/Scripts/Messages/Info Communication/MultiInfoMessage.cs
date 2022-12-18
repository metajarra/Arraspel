using Godot;
using System;
using System.Collections.Generic;

public class MultiInfoMessage : MultiTargetMessage
{
	[Export] private float TEMP_defaultMaxRange;
	
	public MultiInfoMessage(){
		this.info = new InfoPacket();
		this.intendedTargets = new List<Unit>();
	}
	
	public MultiInfoMessage(string message){
		this.info = new InfoPacket(message);
		this.intendedTargets = new List<Unit>();
	}
	
	public MultiInfoMessage(string message, List<Unit> targets){
		this.info = new InfoPacket(message);
		this.intendedTargets = targets;
	}
	
	public override void _Ready(){
		GetAreaOfEffect();
		Connect("area_entered", this, "OnAreaEntered");
		
		maxRange = TEMP_defaultMaxRange;
	}
	
	protected override void OnAreaEntered(Node2D area){
		GD.Print("who goes there?");
		if (intendedTargets.Contains((Unit)area)){
			GD.Print("a friend!");
		}
	}
}

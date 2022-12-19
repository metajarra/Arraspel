using Godot;
using System;
using System.Collections.Generic;

public abstract class MultiTargetMessage : Message
{
	protected List<Unit> intendedTargets;
	
	public void Init(string message, List<Unit> targets){
		this.info = new InfoPacket(message);
		this.intendedTargets = targets;
	}
	
	protected void GetFurthestUnit(){
		float distanceToFurthestUnit = 0.0f;
		for (int i = 0; i < intendedTargets.Count; i++){
			if (GlobalPosition.DistanceTo(intendedTargets[i].GlobalPosition) > distanceToFurthestUnit)
				distanceToFurthestUnit = GlobalPosition.DistanceTo(intendedTargets[i].GlobalPosition);
		}
		
		maxRange = distanceToFurthestUnit * 2;
	}
}

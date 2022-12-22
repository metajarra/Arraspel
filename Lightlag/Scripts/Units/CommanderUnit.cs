using Godot;
using System;

public class CommanderUnit : Unit
{
	public override void ReceiveInfo(InfoPacket info){
		if (info.type == InfoType.PositionUpdate){
			float angle = info.angle * 180 / Mathf.Pi + 90;
			((ControllableUnit)info.unit).GetIndicator().UpdateIndicator(angle, info.vector2);
		}
		
		this.info = info;
	}
}

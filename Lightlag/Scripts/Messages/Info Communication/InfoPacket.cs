using Godot;
using System;

public class InfoPacket
{
	public InfoType type;
	
	public string TEMP_messageInfo;
	public Vector2 vector2;
	public float angle;
	public Unit unit;
	
	public InfoPacket(){
		type = InfoType.Default;
		this.TEMP_messageInfo = "default message";
	}
	
	public InfoPacket(string message){
		type = InfoType.Default;
		this.TEMP_messageInfo = message;
	}
	
	public string GetInfo(){
		return TEMP_messageInfo;
	}
}

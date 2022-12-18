using Godot;
using System;

public class InfoPacket
{
	private string TEMP_messageInfo;
	
	public InfoPacket(){
		this.TEMP_messageInfo = "default message";
	}
	
	public InfoPacket(string message){
		this.TEMP_messageInfo = message;
	}
	
	public string GetInfo(){
		return TEMP_messageInfo;
	}
}

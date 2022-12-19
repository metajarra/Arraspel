using Godot;
using System;
using System.Collections.Generic;

public class CommanderControl : Control
{
	private Node2D commanderPosition;
	private TextEdit TEMP_textInput;
	
	private PackedScene multiInfoMessagePrefab;
	
	public override void _Ready(){
		commanderPosition = (Node2D)GetTree().Root.GetNode("Main/Commander Position");
		TEMP_textInput = (TextEdit)GetNode("TextEdit");
		
		multiInfoMessagePrefab = GD.Load<PackedScene>("res://Prefabs/Messages/Multi Info Message.tscn");
	}
	
	private void TestSendMessage()
	{
		string messageString = TEMP_textInput.Text;
		
		List<Unit> testUnits = new List<Unit>();
		testUnits.Add(new Unit("name 1", "123"));
		
		MultiInfoMessage msg = (MultiInfoMessage)multiInfoMessagePrefab.Instance();
		commanderPosition.AddChild(msg);
		msg.Init(messageString, testUnits);
		
	}
}

using Godot;
using System;

public class CommanderControl : Control
{
	private Node2D commanderPosition;
	PackedScene commsPrefab = GD.Load<PackedScene>("res://Prefabs/Communication.tscn");
	
	public override void _Ready(){
		commanderPosition = (Node2D)GetTree().Root.GetNode("Main/Commander Position");
	}
	
	private void TestSendMessage()
	{
		Command command = (Command)commsPrefab.Instance();
		Unit unit = new Unit("name 1", "123");
		
		command.SetInfo("test message woo", unit);
		
		commanderPosition.AddChild(command);
	}
}

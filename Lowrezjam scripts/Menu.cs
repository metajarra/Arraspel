using Godot;
using System;

public class Menu : Control
{
	private void _on_Start_Button_pressed()
	{
		GetTree().ChangeScene("res://Scenes/Root.tscn");
	}
}

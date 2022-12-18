using Godot;
using System;

public class EndScreen : Control
{
	private void _on_TextureButton_pressed()
	{
		GetTree().ChangeScene("res://Scripts/Menu.tscn");
	}
}




using Godot;
using System;

public class buttonloadlevel : Button
{
	[Export] private bool LoadNext = true;
	
	private void _on_Button_pressed()
	{
		// Replace with function body.
		LevelManager manager = GetNode<LevelManager>("/root/LevelManager");
		
		if (LoadNext) manager.LoadNextLevel();
		else manager.LoadPreviousLevel();
	}
}

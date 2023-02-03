using Godot;
using System;

public class MainMenu : Node
{
	private void OnPlayButtonPressed()
	{
		LevelManager manager = GetNode<LevelManager>("/root/LevelManager");
		manager.LoadNextLevel();
	}
}

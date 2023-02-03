using Godot;
using System;

public class AreaTransition : Node
{
	[Export] private bool LoadNext = true;
	[Export] private int LoadIndex = 0;
	
	public override void _Ready(){
		Area2D collisionDetector = (Area2D)GetParent();
		collisionDetector.Connect("body_entered", this, "OnChangeLevel");
	}
	
	private void OnChangeLevel(Node body){
		if (body.IsInGroup("Player")){
			LevelManager manager = GetNode<LevelManager>("/root/LevelManager");
			
			if (LoadNext) manager.LoadNextLevel();
			else manager.LoadLevelAtIndex(LoadIndex);
		}
	}
}

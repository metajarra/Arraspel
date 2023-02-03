using Godot;
using System;

public class LevelManager : Node
{
	[Export] private string[] Levels;
	[Export] private string CurrentLevel;
	
	private Node CurrentLevelNode;
	private Resource CurrentLevelResource;
	
	// Notice that these are only useful for basic animations (ie fade in/out)
	private AnimationPlayer TransitionPlayer;
	private bool LoadOutAnimFinished = false;
	
	public override void _Ready(){
		Viewport root = GetTree().Root;
		CurrentLevelNode = root.GetChild(root.GetChildCount() - 1);
		
		TransitionPlayer = (AnimationPlayer)GetTree().Root.GetNode("SceneTransition/AnimationPlayer");
		TransitionPlayer.Connect("animation_finished", this, "OnTransitionFinished");
		TransitionPlayer.Play("level_load_in");
	}
	
	private void OnTransitionFinished(string anim_name){
		if (anim_name == "level_load_out") LoadOutAnimFinished = true;
	}
	
	public override void _Process(float delta){
		if (CurrentLevelResource == null) return;
		
		TransitionPlayer.Play("level_load_out");
		
		if (LoadOutAnimFinished){
			GD.Print($"Loaded resource: {CurrentLevelResource}");
			
			CurrentLevelNode.QueueFree();
			CurrentLevelNode = ((PackedScene)CurrentLevelResource).Instance();
			GetTree().Root.AddChild(CurrentLevelNode);
			
			CurrentLevelResource = null;
			LoadOutAnimFinished = false;
			
			TransitionPlayer.Play("level_load_in");
		}
	}
	
	public void ReloadCurrentLevel(){
		LoadLevelAtIndex(Array.IndexOf(Levels, CurrentLevel));
	}
	
	public void LoadNextLevel(){
		int index = Array.IndexOf(Levels, CurrentLevel);
		
		if (index >= Levels.Length - 1){ 
			// TEMPORARY! Replace with something else (eg. load credits)
			return; 
		}
		
		LoadLevelAtIndex(index + 1);
	}
	
	public void LoadPreviousLevel(){
		int index = Array.IndexOf(Levels, CurrentLevel);
		
		if (index <= 0){ 
			// TEMPORARY! Replace with something else (eg. load main menu)
			return; 
		}
		
		LoadLevelAtIndex(index - 1);
	}
	
	public void LoadLevelAtIndex(int index){
		CallDeferred(nameof(DeferredLoadLevel), Levels[index]);
	}
	
	private void DeferredLoadLevel(string level){
		CurrentLevelResource = ResourceLoader.Load(level);
		GD.Print($"Started loading resource {level}");
		
		CurrentLevel = level;
	}
}

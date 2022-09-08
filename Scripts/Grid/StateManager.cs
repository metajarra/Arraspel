using Godot;
using System;

public class StateManager : Node2D
{
	[Export] public States state;
	
	/*
	public override void _Process(float delta){
		if (Input.IsActionJustPressed("start_recording_attack")){
			state = States.EDIT;
			GD.Print("edit state activated");
		} else if (Input.IsActionJustPressed("end_recording_attack")){
			state = States.BATTLE;
			GD.Print("battle state activated");
		}
	}
	*/
	
	public void ActivateEditState(){
		state = States.EDIT;
		GD.Print("edit state activated");
	}
	
	public void ActivateBattleState(){
		state = States.BATTLE;
		GD.Print("battle state activated");
	}
}

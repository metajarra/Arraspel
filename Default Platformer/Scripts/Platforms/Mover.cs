using Godot;
using System;

// THIS CODE DOES NOT WORK!
// Don't use it but use as reference in case you want to develop movable platforms later

public class Mover : Node
{
	/*
	private KinematicBody2D Platform;
	private Vector2 MoveDirection;
	
	[Export] private float MaxHorizontalMoveSpeed = 100;
	[Export] private float ForceOfGravity = 12;
	
	private enum ContactState { On_Floor, In_Air }
	private ContactState CState = ContactState.In_Air;
	
	public override void _Ready(){
		Platform = (KinematicBody2D)GetParent();
	}
	
	public override void _PhysicsProcess(float delta){
		// Determines contact state
		if (Platform.IsOnFloor()){
			CState = ContactState.On_Floor;
		}
		
		else {
			if (Platform.IsOnCeiling()) MoveDirection.y = 0;
			CState = ContactState.In_Air;
		}
		
		switch (CState){
			case ContactState.In_Air:
				MoveDirection.y += ForceOfGravity;
				break;
			default:
				MoveDirection.y = 0;
				break;
		}
		
		Platform.MoveAndSlide(MoveDirection, new Vector2(0, -1));
	}
	*/
}

using Godot;
using System;

public class InputController : Node
{
	Movement PlayerMovement;
	
	public override void _Ready(){
		PlayerMovement = (Movement)GetParent();
	}
	
	public override void _Process(float delta){
		int h_direction = 0;
		if (Input.IsActionPressed("move_left")) h_direction = -1;
		if (Input.IsActionPressed("move_right")) h_direction = 1;
		PlayerMovement.Move_LR(h_direction);
		
		int v_direction = 0;
		if (Input.IsActionPressed("move_up")) v_direction = -1;
		if (Input.IsActionPressed("move_down")) v_direction = 1;
		PlayerMovement.Move_UD(v_direction);
		
		
		if (Input.IsActionJustPressed("hold_wall")) PlayerMovement.Hold();
		else if (Input.IsActionJustReleased("hold_wall")) PlayerMovement.Release();
		
		if (Input.IsActionJustPressed("jump")) PlayerMovement.Jump();
	}
}

using Godot;
using System;
using System.Collections.Generic;

public class LaserTurretTest : Node2D
{
	private Line2D laser;
	private Grid grid;
	
	private bool isRecordingAttack;
	
	[Export] private int maxAttackSteps;
	private List<Vector2> attackPositions;
	
	public override void _Ready(){
		laser = (Line2D)GetNode("Line2D");
		grid = (Grid)GetTree().Root.GetNode("Root/Grid/Pixel Res Tilemap");
		
		attackPositions = new List<Vector2>();
	}
	
	public override void _Process(float delta){
		Vector2[] pointArray = new Vector2[] { Vector2.Zero, grid.GetGridPos(GetGlobalMousePosition()) - GlobalPosition };
		laser.SetPoints(pointArray);
		
		if (isRecordingAttack){
			if (Input.IsActionJustPressed("click")){
				if (attackPositions.Count < maxAttackSteps){
					attackPositions.Add(grid.GetGridPos(GetGlobalMousePosition()));
					GD.Print($"Added target {attackPositions[attackPositions.Count - 1]}");
				} else {
					GD.Print("You are at max attack steps!");
				}
			}
		}
		
		else {
			if (Input.IsActionJustPressed("click")){
				for (int i = 0; i < attackPositions.Count; i++){
					GD.Print($"Attacked at target {attackPositions[i]}");
				}
			}
		}
		
		if (Input.IsActionJustPressed("start_recording_attack")){
			isRecordingAttack = true;
		} else if (Input.IsActionJustPressed("end_recording_attack")){
			isRecordingAttack = false;
		}
	}
}

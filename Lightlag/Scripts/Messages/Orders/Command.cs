using Godot;
using System;

public class Command : Area2D
{
	private string message;
	private Unit target; // change from TestUnit to Unit
	
	private CollisionShape2D areaOfEffect;
	
	[Export] private float travelSpeed; // temporary
	
	public override void _Ready(){
		areaOfEffect = (CollisionShape2D)GetNode("Area of Effect");
	}
	
	public override void _Process(float delta){
		((CircleShape2D)areaOfEffect.Shape).Radius += travelSpeed;
	}
	
	private void OnAreaEntered(Node2D area)
	{
		// Only compatible with single-target messages, needs to be updated for
		// messages to be sent to multiple targets
		if (area.Equals(target)){
			GD.Print($"Target hit. Name: {target.shipName} | ID: {target.shipID}");
			QueueFree();
		}
	}
	
	public void SetInfo(string message, Unit target){
		this.message = message;
		this.target = target;
	}
}

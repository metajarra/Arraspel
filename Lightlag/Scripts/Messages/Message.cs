using Godot;
using System;

public abstract class Message : Area2D
{
	protected float maxRange;
	[Export] private float travelSpeed; // temporary
	
	protected InfoPacket info;
	protected Orders orders;
	
	private CircleShape2D areaOfEffect;
	
	public override void _Ready(){
		GetAreaOfEffect();
	}
	
	protected void GetAreaOfEffect(){
		CollisionShape2D t_areaOfEffect = (CollisionShape2D)GetNode("Area of Effect");
		areaOfEffect = (CircleShape2D)t_areaOfEffect.Shape;
	}
	
	public override void _Process(float delta){
		areaOfEffect.Radius += travelSpeed;
		
		if (areaOfEffect.Radius >= maxRange){
			QueueFree();
		}
	}
	
	protected abstract void OnAreaEntered(Node2D area);
}

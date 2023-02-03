using Godot;
using System;

public class Platform : Node2D
{
	private CollisionShape2D Collider;
	
	public override void _Ready(){
		Collider = (CollisionShape2D)GetNode("Collider");
		
		Collider.Scale = Scale;
		Scale = Vector2.One;
	}
}

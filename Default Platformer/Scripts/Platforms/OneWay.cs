using Godot;
using System;

public class OneWay : Node
{
	public override void _Ready(){
		((CollisionShape2D)GetParent().GetNode("Collider")).OneWayCollision = true;
	}
}

using Godot;
using System;

public class CollisionDetector : Area2D
{
	[Export] private bool CanDie = false;
	
	private void OnCollisionDetectorBodyEntered(Node body)
	{
		if (body.IsInGroup("Danger") && CanDie){
			((Death)GetParent()).Die();
		}
	}
}

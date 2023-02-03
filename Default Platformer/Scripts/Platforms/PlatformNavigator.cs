using Godot;
using System;

public class PlatformNavigator : PathFollow2D
{
	[Export] private float MoveSpeed = 50;
	private int Reversing = 1;
	
	public override void _PhysicsProcess(float delta){
		if (UnitOffset == 1 && !Loop) Reversing = -1;
		else if (UnitOffset == 0 && !Loop) Reversing = 1;
		
		Offset = Offset + MoveSpeed * Reversing * delta;
	}
}

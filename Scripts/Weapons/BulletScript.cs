using Godot;
using System;

public class BulletScript : KinematicBody2D
{
	[Signal] public delegate void Hit();
	
	[Export] private float timerSet;
	private float elapsedTime;
	
	[Export] private float launchSpeed;
	public int damage;
	
	public override void _Process(float delta)
	{
		elapsedTime += delta;
		
		if (elapsedTime >= timerSet){
			DestroySelf();
		}
	}
	
	public override void _PhysicsProcess(float delta)
	{
		MoveAndCollide(GlobalTransform.Orthonormalized().y * launchSpeed * -1);
	}
	
	public void DestroySelf()
	{
		QueueFree();
	}
	
	private void OnArea2DBodyEntered(Node body)
	{
		if (body.HasNode("Health System")){
			EventParams damageParams = new EventParams();
			damageParams.nodeParam = new Node[] { body.GetNode("Health System") };
			damageParams.intParam = new int[] { damage };
			EventManager.TriggerEvent("onHit", damageParams);
		}
		
		DestroySelf();
	}
}

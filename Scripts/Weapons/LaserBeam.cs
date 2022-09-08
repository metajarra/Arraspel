using Godot;
using System;

public class LaserBeam : Node2D
{
	[Export] private float timeToDecay;
	private float elapsedTime;
	
	public int damage;
	
	public override void _Process(float delta){
		elapsedTime += delta;
		if (elapsedTime >= timeToDecay){
			DestroySelf();
		}
	}
	
	public void DestroySelf()
	{
		QueueFree();
	}
	
	private void _on_Area2D_body_entered(Node2D body)
	{
		if (body.HasNode("Health System")){
			EventParams damageParams = new EventParams();
			damageParams.nodeParam = new Node[] { body.GetNode("Health System") };
			damageParams.intParam = new int[] { damage };
			EventManager.TriggerEvent("onHit", damageParams);
		}
	}
}

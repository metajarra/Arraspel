using Godot;
using System;

public class Enemy : KinematicBody2D
{
	[Export] private float moveSpeed;
	private Node2D boss;
	private bool isTouchingSurface;
	private bool isTouchingBoss;
	
	[Export] private float timeBetweenAttacks;
	private float elapsedTime;
	
	public override void _Process(float delta){
		if (!isTouchingSurface){
			MoveAndCollide(new Vector2(0, moveSpeed));
		} 
		
		if (isTouchingBoss){
			elapsedTime += delta;
			if (elapsedTime >= timeBetweenAttacks){
				elapsedTime = 0;
				Attack();
			}
		}
	}
	
	private void Attack(){
		GD.Print("attacked boss");
		EventParams damageParams = new EventParams();
		damageParams.nodeParam = new Node[] { boss.GetParent().GetNode("Health System") };
		damageParams.intParam = new int[] { 1 };
		EventManager.TriggerEvent("onHit", damageParams);
	}
	
	private void _on_Area2D_body_entered(Node2D body)
	{
		if (body.GetParent().Name == "Boss"){
			GD.Print("touched boss");
			isTouchingBoss = true;
			boss = body;
		}
	}
}

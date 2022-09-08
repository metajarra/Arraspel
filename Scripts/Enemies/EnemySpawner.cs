using Godot;
using System;

public class EnemySpawner : Node2D
{
	[Export] private float timeBetweenSpawn;
	private float elapsedTime;

	public bool reset;
	
	private PackedScene enemyPrefab;
	
	public override void _Ready(){
		enemyPrefab = GD.Load<PackedScene>("res://Prefabs/Enemy.tscn");
		reset = true;
	}
	
	public override void _Process(float delta){
		elapsedTime += delta;
		if (elapsedTime >= timeBetweenSpawn){
			reset = true;
		}
	}
	
	public void Spawn(){
		elapsedTime = 0;
		KinematicBody2D enemy = (KinematicBody2D)enemyPrefab.Instance();
		enemy.Position = GlobalPosition;
		enemy.Rotation = GlobalRotation;
		GetTree().Root.GetNode("Root").AddChild(enemy);
		reset = false;
		
		GD.Print($"Spawned enemy at spawn point {Name}"); 
		EventManager.TriggerEvent("onEnemySpawn");
	}
}

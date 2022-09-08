using Godot;
using System;
using System.Collections.Generic;

public class SpawnManager : Node2D
{
	[Export] public int targetNumOfEnemies;
	public int currentNumOfEnemies;
	
	private Action<EventParams> onHitListener;
	
	private List<EnemySpawner> enemySpawners;
	
	private StateManager stateManager;
	
	public override void _Ready(){
		onHitListener = new Action<EventParams>(RemoveEnemy);
		EventManager.StartListening("onDeath", onHitListener);
		
		enemySpawners = new List<EnemySpawner>();
		for (int i = 0; i < GetChildCount(); i++){
			enemySpawners.Add((EnemySpawner)GetChild(i));
			GD.Print($"Added spawner {enemySpawners[enemySpawners.Count - 1].Name}");
		}
		
		stateManager = (StateManager)GetTree().Root.GetNode("Root/Grid/State Manager");
	}
	
	public override void _Process(float delta){
		if (stateManager.state == States.BATTLE){
			if (currentNumOfEnemies < targetNumOfEnemies){
				Random r = new Random();
				int rand = r.Next(0, enemySpawners.Count);
				if (enemySpawners[rand].reset){
					enemySpawners[rand].Spawn();
					currentNumOfEnemies++;
				}
			}
		}
	}
	
	private void RemoveEnemy(EventParams enemyKilled){
		if (enemyKilled.nodeParam[0].Name.Contains("Enemy")){
			currentNumOfEnemies--;
		}
	}
}

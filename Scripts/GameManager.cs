using Godot;
using System;

public class GameManager : Node2D
{
	private Action<EventParams> onEditStateListener;
	private Action<EventParams> onEnemySpawnListener;
	
	[Export] private int tempTotalEnemyCount;
	[Export] private int maxConcurrentEnemies;
	private int totalEnemyCount;
	private int currentEnemyCount;
	
	[Export] private int maxLevel;
	public int currentLevel;
	public int maxTurrets;
	
	private SpawnManager spawnManager;
	private StateManager stateManager;
	private UIManager uiManager;
	
	public override void _Ready(){
		onEditStateListener = new Action<EventParams>(ToggleBattle);
		onEnemySpawnListener = new Action<EventParams>(CountEnemy);
		EventManager.StartListening("onEditButtonPress", onEditStateListener);
		EventManager.StartListening("onEnemySpawn", onEnemySpawnListener);
		
		currentLevel = 1;
		maxTurrets = currentLevel + 1;
		
		spawnManager = (SpawnManager)GetTree().Root.GetNode("Root/Grid/8x8 Tilemap/Spawn Manager");
		stateManager = (StateManager)GetTree().Root.GetNode("Root/Grid/State Manager");
		uiManager = (UIManager)GetTree().Root.GetNode("Root/All Visual/Control");
	}
	
	public override void _Process(float delta){
		maxTurrets = currentLevel + 1;
		if (stateManager.state == States.BATTLE){
			if (currentEnemyCount >= tempTotalEnemyCount){
				spawnManager.targetNumOfEnemies = 0;
				
				if (spawnManager.currentNumOfEnemies <= 0){
					GD.Print("---LEVEL CLEARED---");
					if (currentLevel < 5){
						ToggleEdit();
					} else {
						GetTree().ChangeScene("res://Scenes/EndScreen.tscn");
					}
				}
			}
		}
	}
	
	private void ToggleEdit(){
		stateManager.ActivateEditState();
		currentLevel++;
		GD.Print($"Current level: {currentLevel}");
	}
	
	private void ToggleBattle(EventParams args = default){
		stateManager.ActivateBattleState();
		spawnManager.targetNumOfEnemies = maxConcurrentEnemies;
		currentEnemyCount = 0;
		spawnManager.targetNumOfEnemies = (currentLevel * 2) + 4;
		
		EventManager.TriggerEvent("onBattleStart");
	}
	
	private void CountEnemy(EventParams args = default){
		currentEnemyCount++;
	}
}

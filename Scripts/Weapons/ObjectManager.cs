using Godot;
using System;

public class ObjectManager : Node2D
{
	private PositionManager positionManager;
	private Grid grid;
	private StateManager stateManager;
	private GameManager gameManager;
	
	private bool isRecordingPattern;
	public bool isPlacingTurret;
	private Turret recordingTurret;
	private Turret placingTurret;
	
	private int currentTurrets;
	
	private PackedScene gunTurret;
	private PackedScene laserTurret;
	
	public override void _Ready(){
		positionManager = (PositionManager)GetTree().Root.GetNode("Root/Grid/8x8 Tilemap/Position Manager");
		grid = (Grid)GetTree().Root.GetNode("Root/Grid/8x8 Tilemap");
		stateManager = (StateManager)GetTree().Root.GetNode("Root/Grid/State Manager");
		gameManager = (GameManager)GetTree().Root.GetNode("Root/Game Managers/Game Manager");
		
		int objCount = GetChildCount();
		for (int i = 0; i < objCount; i++){
			positionManager.AddObject((Node2D)GetChild(i));
		}
		
		gunTurret = GD.Load<PackedScene>("res://Prefabs/Turret.tscn");
		laserTurret = GD.Load<PackedScene>("res://Prefabs/LaserTurret.tscn");
	}
	
	public override void _Process(float delta){
		if (Input.IsActionJustPressed("click")){
			Vector2 mousePos = GetGlobalMousePosition();
			
			if (stateManager.state == States.EDIT && !isRecordingPattern && !isPlacingTurret){
				if (positionManager.GetObjectWithMouse(mousePos) != null && positionManager.GetObjectWithMouse(mousePos).Name.Contains("Turret")){
					Turret turret = (Turret)positionManager.GetObjectWithMouse(mousePos);
					recordingTurret = turret;
					
					
					if (!turret.fullPattern){
						turret.justSelected = true;
						turret.StartRecordingPattern();
						isRecordingPattern = true;
					}
					
				}
			} else if (stateManager.state == States.BATTLE){
				if (positionManager.GetObjectWithMouse(mousePos) != null && positionManager.GetObjectWithMouse(mousePos).Name.Contains("Turret")){
					Turret turret = (Turret)positionManager.GetObjectWithMouse(mousePos);
					
					turret.StartAttackPattern();
				}
			} else if (isPlacingTurret && placingTurret != null && currentTurrets < gameManager.maxTurrets){
				if (positionManager.GetObjectWithMouse(GetGlobalMousePosition()) == null){
					if (grid.GetGridPos(GetGlobalMousePosition()).x == 0 && grid.GetGridPos(GetGlobalMousePosition()).y != 7){
						positionManager.AddObject((Node2D)placingTurret);
						currentTurrets++;
						
						placingTurret = null;
						isPlacingTurret = false;
						GD.Print("placed turret");
					} else if (grid.GetGridPos(GetGlobalMousePosition()).x == 7 && grid.GetGridPos(GetGlobalMousePosition()).y != 7){
						placingTurret.SetRotationDegrees(180);
						
						positionManager.AddObject((Node2D)placingTurret);
						currentTurrets++;
						
						placingTurret = null;
						isPlacingTurret = false;
						GD.Print("placed turret");
					}
				}
			}
		}
		
		else if (Input.IsActionJustPressed("finish_recording_pattern") && isRecordingPattern){
			isRecordingPattern = false;
			if (recordingTurret != null){
				recordingTurret.StopRecordingPattern();
			}
		}
		
		else if (isPlacingTurret && placingTurret != null){
			placingTurret.Position = grid.GetGridPos(GetGlobalMousePosition()) * 8 + (Vector2.One * 4);
		}
	}
	
	public void PlaceNewGunTurret(){
		if (currentTurrets < gameManager.maxTurrets){
			isPlacingTurret = true;
			placingTurret = (Turret)gunTurret.Instance();
			AddChild(placingTurret);
		}
	}
	
	public void PlaceNewLaserTurret(){
		if (currentTurrets < gameManager.maxTurrets){
			isPlacingTurret = true;
			placingTurret = (Turret)laserTurret.Instance();
			AddChild(placingTurret);
		}
	}
}

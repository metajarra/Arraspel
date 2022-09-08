using Godot;
using System;

public class UIManager : Control
{
	private StateManager stateManager;
	private Grid grid;
	private PositionManager positionManager;
	private ObjectManager objectManager;
	
	private Sprite laserPreview;
	private Sprite bulletPreview;
	
	private Turret turret;
	
	public override void _Ready(){
		stateManager = (StateManager)GetTree().Root.GetNode("Root/Grid/State Manager");
		grid = (Grid)GetTree().Root.GetNode("Root/Grid/8x8 Tilemap");
		positionManager = (PositionManager)GetTree().Root.GetNode("Root/Grid/8x8 Tilemap/Position Manager");
		objectManager = (ObjectManager)GetTree().Root.GetNode("Root/Grid/Objects");
		
		laserPreview = (Sprite)GetParent().GetNode("Laser Beam Preview");
		bulletPreview = (Sprite)GetParent().GetNode("Bullet Preview");
		
		laserPreview.Visible = false;
		bulletPreview.Visible = false;
	}
	
	public override void _Process(float delta){
		if (stateManager.state == States.EDIT){
			Vector2 currentNode = grid.GetGridPos(GetGlobalMousePosition());
			if (Input.IsActionJustPressed("click") && !objectManager.isPlacingTurret){
				if (positionManager.GetObjectWithCoordinates(currentNode) != null 
				&& positionManager.GetObjectWithCoordinates(currentNode).Name.Contains("Turret") 
				&& turret == null){
					Turret tempTurret = (Turret)positionManager.GetObjectWithCoordinates(currentNode);
					if (!tempTurret.fullPattern){
						turret = (Turret)positionManager.GetObjectWithCoordinates(currentNode);
					}
				}
			}
			
			if (turret != null){
				EventParams data = turret.GetData();
				
				/* reference sheet for data params:
					weaponTypeParam = [weapon type]
					intParam = [param (width or diameter), step count, pixel cost]
					vector2Param = [target, beam/bullet pos]
					floatParam = [angle]
				*/
				
				if (data.weaponTypeParam[0] == WeaponType.GUN){
					bulletPreview.Visible = true; laserPreview.Visible = false;
					bulletPreview.Position = data.vector2Param[1];
					bulletPreview.SetRotationDegrees(data.floatParam[0]);
					bulletPreview.Scale = new Vector2(data.intParam[0] / 8.0f, data.intParam[0] / 8.0f);
				} else if (data.weaponTypeParam[0] == WeaponType.LASER){
					laserPreview.Visible = true; bulletPreview.Visible = false;
					laserPreview.Position = data.vector2Param[1];
					laserPreview.SetRotationDegrees(data.floatParam[0]);
					laserPreview.Scale = new Vector2(data.intParam[0] / 8.0f, (data.vector2Param[0] - turret.GlobalPosition).Length() / 2);
				}
			}
			
			if (Input.IsActionJustPressed("finish_recording_pattern")){
				turret = null;
				bulletPreview.Visible = false;
				laserPreview.Visible = false;
			}
		}
	}
	
	private void _on_Button_pressed(){
		if (stateManager.state == States.EDIT){
			EventManager.TriggerEvent("onEditButtonPress");
			bulletPreview.Visible = false;
			laserPreview.Visible = false;
		}
	}
	
	private void OnGunButtonPressed()
	{
		if (stateManager.state == States.EDIT){
			GD.Print("added new gun");
			objectManager.PlaceNewGunTurret();
		}
	}
	
	private void OnLaserButtonPressed()
	{
		if (stateManager.state == States.EDIT){
			GD.Print("added new laser");
			objectManager.PlaceNewLaserTurret();
		}
	}
	
	private void _on_Button4_pressed()
	{
		if (stateManager.state == States.EDIT){
			GD.Print("undo");
		}
	}
}

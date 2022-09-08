using Godot;
using System;
using System.Collections.Generic;

public class Turret : Node2D
{
	private Weapon weapon;
	private Grid grid;
	
	// Time data
	[Export] private float timeBetweenSteps;
	private float elapsedTime;
	
	// Attack pattern data
	[Export] private int maxAttackPatternSteps;
	private List<Vector2> attackPositions;
	private List<int> attackParams;
	private int currentAttackParam;
	
	// Internal process data
	private bool isRecordingPattern;
	private bool isAttacking;
	private int index;
	
	public bool justSelected;
	public bool uiSelected;
	
	public bool fullPattern;
	
	public override void _Ready(){
		weapon = (Weapon)GetNode("Weapon");
		grid = (Grid)GetTree().Root.GetNode("Root/Grid/Pixel Res Tilemap");
		
		attackPositions = new List<Vector2>();
		attackParams = new List<int>();
		currentAttackParam = (int)weapon.weaponLimits.x;
	}
	
	public override void _Process(float delta){
		if (isAttacking){
			if (elapsedTime == 0){
				if (index < attackPositions.Count){
					Attack(index);
					index++;
				} else {
					index = 0;
					isAttacking = false;
					
					GD.Print($"{Name} finished attack pattern");
				}
			}
			
			elapsedTime += delta;
			if (elapsedTime >= timeBetweenSteps){
				elapsedTime = 0.0f;
			}
		}
		
		else if (isRecordingPattern){
			if (Input.IsActionJustReleased("mouse_scroll_up") && currentAttackParam < weapon.weaponLimits.y){
				currentAttackParam++;
			} else if (Input.IsActionJustReleased("mouse_scroll_down") && currentAttackParam > weapon.weaponLimits.x){
				currentAttackParam--;
			}
			
			if (justSelected){
				justSelected = false;
			} else if (Input.IsActionJustPressed("click") && attackPositions.Count < maxAttackPatternSteps){
				attackPositions.Add(grid.GetGridPos(GetGlobalMousePosition()));
				attackParams.Add(currentAttackParam);
				
				GD.Print($"added {attackPositions[attackPositions.Count - 1]}");
			}
		}
		
		if (attackPositions.Count >= maxAttackPatternSteps){
			fullPattern = true;
		} else {
			fullPattern = false;
		}
	}
	
	public void StartRecordingPattern(){
		isRecordingPattern = true;
		GD.Print($"{Name} started recording attack pattern");
	}
	
	public void StopRecordingPattern(){
		isRecordingPattern = false;
		GD.Print($"{Name} stopped recording attack pattern");
	}
	
	public void StartAttackPattern(){
		isAttacking = true;
		GD.Print($"{Name} started attack pattern");
	}
	
	private void Attack(int i){
		weapon.Attack(attackPositions[i], attackParams[i]);
	}
	
	public EventParams GetData(){
		EventParams data = new EventParams();
		int param = currentAttackParam;
		int stepCount = attackPositions.Count;
		
		WeaponType type = weapon.type;
		data.weaponTypeParam = new WeaponType[] { type };
		
		if (type == WeaponType.GUN){
			Vector2 target = grid.GetGridPos(GetGlobalMousePosition());
			Vector2 bulletPos = GlobalPosition + (new Vector2(target - GlobalPosition).Normalized() * 12);
			float angle = Mathf.Atan2((target - GlobalPosition).y, (target - GlobalPosition).x) * (180 / Mathf.Pi) + 90.0f;
			int pixelCost = (int)(Mathf.Pi * Mathf.Pow(param / 2, 2));
			
			data.intParam = new int[] { param, stepCount, pixelCost };
			data.floatParam = new float[] { angle };
			data.vector2Param = new Vector2[] { target, bulletPos };
		}
		
		else if (type == WeaponType.LASER){
			Vector2 target = grid.GetGridPos(GetGlobalMousePosition());
			Vector2 beamPos = GlobalPosition + (target - GlobalPosition) / 2;
			float angle = Mathf.Atan2((target - GlobalPosition).y, (target - GlobalPosition).x) * (180 / Mathf.Pi) + 90.0f;
			
			Vector2 difference = new Vector2(Mathf.Abs(target.x - GlobalPosition.x), Mathf.Abs(target.y - GlobalPosition.y));
			int pixelCost = (int)Mathf.Max(difference.x, difference.y) * param;
			
			data.intParam = new int[] { param, stepCount, pixelCost };
			data.floatParam = new float[] { angle };
			data.vector2Param = new Vector2[] { target, beamPos };
		}
		
		return data;
	}
}

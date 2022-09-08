using Godot;
using System;
using System.Collections.Generic;

public class Weapon : Node2D
{
	public Vector2 weaponLimits;
	[Export] public WeaponType type;
	
	private PackedScene laser;
	private PackedScene gun;
	
	public override void _Ready(){
		if (type == WeaponType.LASER){
			weaponLimits = new Vector2(1, 8);
		} else if (type == WeaponType.GUN){
			weaponLimits = new Vector2(2, 8);
		}
		
		laser = GD.Load<PackedScene>("res://Prefabs/LaserBeam.tscn");
		gun = GD.Load<PackedScene>("res://Prefabs/Bullet.tscn");
	}
	
	public void Attack(Vector2 attackPos, int param){
		GD.Print($"Weapon {Name} attacked at position {attackPos} with param {param}");
		if (type == WeaponType.LASER){
			LaserAttack(attackPos, param);
		} else if (type == WeaponType.GUN){
			GunAttack(attackPos, param);
		}
	}
	
	private void LaserAttack(Vector2 attackPos, int width){
		LaserBeam beam = (LaserBeam)laser.Instance();
		
		Vector2 position = GlobalPosition + (attackPos - GlobalPosition) / 2;
		beam.Position = position;
		
		Vector2 orientDirection = attackPos - GlobalPosition;
		float angle = Mathf.Atan2(orientDirection.y, orientDirection.x) * (180 / Mathf.Pi) + 90.0f;
		beam.SetRotationDegrees(angle);
		
		Vector2 scale = new Vector2(width / 8.0f, (attackPos - GlobalPosition).Length() / 2);
		beam.Scale = scale;
		
		beam.damage = (int)Mathf.Pow(width, 2);
		
		GetTree().Root.GetNode("Root/Grid").AddChild(beam);
		
		Vector2 difference = new Vector2(Mathf.Abs(attackPos.x - GlobalPosition.x), Mathf.Abs(attackPos.y - GlobalPosition.y));
		int pixelCost = (int)Mathf.Max(difference.x, difference.y) * width;
		
		EventParams pixelParams = new EventParams();
		pixelParams.intParam = new int[] { pixelCost };
		
		EventManager.TriggerEvent("onPixels", pixelParams);
		GD.Print($"Triggered onPixels for beam with cost {pixelCost}");
	}
	
	private void GunAttack(Vector2 attackPos, int diameter){
		BulletScript bullet = (BulletScript)gun.Instance();
		
		bullet.Position = GlobalPosition;
		
		Vector2 orientDirection = attackPos - GlobalPosition;
		float angle = Mathf.Atan2(orientDirection.y, orientDirection.x) * (180 / Mathf.Pi) + 90.0f;
		bullet.SetRotationDegrees(angle);
		
		Vector2 scale = new Vector2(diameter / 8.0f, diameter / 8.0f);
		bullet.Scale = scale;
		
		bullet.damage = diameter;
		
		GetTree().Root.GetNode("Root/Grid").AddChild(bullet);
		
		int pixelCost = (int)(Mathf.Pi * Mathf.Pow(diameter / 2, 2));
		
		EventParams pixelParams = new EventParams();
		pixelParams.intParam = new int[] { pixelCost };
		
		EventManager.TriggerEvent("onPixels", pixelParams);
		GD.Print($"Triggered onPixels for bullet with cost {pixelCost}");
	}
}

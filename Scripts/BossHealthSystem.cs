using Godot;
using System;

public class BossHealthSystem : Node
{
	[Export] private float hp;
	
	private Action<EventParams> onHitListener;
	
	public override void _Ready()
	{
		onHitListener = new Action<EventParams>(DamageSelf);
		EventManager.StartListening("onHit", onHitListener);
	}
	
	private void DamageSelf(EventParams hitParams)
	{
		if (hitParams.nodeParam[0] == this){
			hp -= hitParams.intParam[0];
			if(hp <= 0){
				GD.Print($"{GetParent().Name} destroyed");
				EventManager.StopListening("onHit", onHitListener);
				if (GetParent().Name == "Boss"){
					EventManager.TriggerEvent("onPlayerDeath");
					GetTree().ChangeScene("res://Scenes/LoseScreen.tscn");
				}
				else{
					EventParams deathParams = new EventParams();
					deathParams.nodeParam = new Node[] { GetParent() };
					EventManager.TriggerEvent("onDeath", deathParams);
					GD.Print($"onDeath triggered: {deathParams.nodeParam[0].Name}");
				}
			}
		}
	}
}

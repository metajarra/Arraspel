using Godot;
using System;
using System.Collections.Generic;

public class CommanderControl : Control
{
	private Orders orders;
	private void AddDestination(Vector2 destination){ orders.destination = destination; }
	private void SendOrders(){
		MultiOrderMessage msg = (MultiOrderMessage)multiOrderMessagePrefab.Instance();
		GetNode("Radar Display").AddChild(msg);
		msg.GlobalPosition = commander.GlobalPosition;
		
		List<Unit> exportUnits = new List<Unit>();
		foreach (Unit unit in selectedUnits)
			exportUnits.Add(unit);
		
		msg.Init(orders, exportUnits);
		
		DeselectUnits();
		selecting = false;
	}
	
	private CommanderUnit commander;
	
	private Control shipInfo;
	private void UpdateSelectedInfo(Unit unit){
		shipInfo.Visible = true;
		
		((Label)shipInfo.GetNode("Ship Name")).Text = unit.shipName;
		((Label)shipInfo.GetNode("Ship ID")).Text = unit.shipID;
		((Label)shipInfo.GetNode("Navy")).Text = unit.shipNavy;
		((Label)shipInfo.GetNode("Captain")).Text = unit.shipCaptain;
		
		float distance = commander.GlobalPosition.DistanceTo(((ControllableUnit)unit).GetIndicator().RectPosition);
		((Label)shipInfo.GetNode("Distance")).Text = $"{distance} km";
		((Label)shipInfo.GetNode("Light Lag")).Text = $"{distance / 38} s";
	}
	
	private List<Unit> selectedUnits;
	private bool selecting;
	private void SelectUnit(RadarIndicator indicator){
		selecting = true;
		
		Unit unit = indicator.GetLinkedUnit();
		if (!selectedUnits.Contains(unit))
			selectedUnits.Add(unit);
		UpdateSelectedInfo(unit);
	}
	private void DeselectUnits(){
		selectedUnits.Clear();
		shipInfo.Visible = false;
	}
	
	private PackedScene multiInfoMessagePrefab;
	private PackedScene multiOrderMessagePrefab;
	
	public override void _Ready(){
		orders = new Orders();
		commander = (CommanderUnit)GetTree().Root.GetNode("Main/Space/Commander Unit");
		shipInfo = (Control)GetNode("Ship Info");
		selectedUnits = new List<Unit>();
		
		multiInfoMessagePrefab = GD.Load<PackedScene>("res://Prefabs/Messages/Multi Info Message.tscn");
		multiOrderMessagePrefab = GD.Load<PackedScene>("res://Prefabs/Messages/Multi Order Message.tscn");
	}
	
	public override void _Process(float delta){
		if (Input.IsActionPressed("multi_select")){
			selecting = true;
		}
	}
	
	public void AddIndicator(RadarIndicator indicator){
		Button button = (Button)indicator.GetNode("Button");
		
		Godot.Collections.Array ship = new Godot.Collections.Array(); 
		ship.Add(indicator);
		button.Connect("pressed", this, "SelectUnit", ship);
	}
	
	private void OnNothingPressed(object @event){
		if (@event is InputEventMouseButton){
			if (selecting){
				AddDestination(GetViewport().GetMousePosition());
				SendOrders();
			} else DeselectUnits();
		}
	}
}

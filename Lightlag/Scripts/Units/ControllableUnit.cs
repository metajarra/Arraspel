using Godot;
using System;
using System.Collections.Generic;

public class ControllableUnit : Unit
{
	private Timer updateTimer;
	[Export] private float timeBetweenUpdates;
	
	private Vector2 destination;
	[Export] private float moveSpeed;
	
	private CommanderUnit commander;
	private CommanderControl commanderUI;
	private Control radarDisplay;
	
	private PackedScene multiInfoMessagePrefab;
	private PackedScene radarIndicatorPrefab;
	private RadarIndicator linkedIndicator;
	public RadarIndicator GetIndicator() { return linkedIndicator; }
	private void SetupIndicator(){
		linkedIndicator = (RadarIndicator)radarIndicatorPrefab.Instance();
		linkedIndicator.InitIndicator(shipName, shipID, shipNavy, this);
		linkedIndicator.RectPosition = GlobalPosition;
		
		commanderUI.AddIndicator(linkedIndicator);
		radarDisplay.AddChild(linkedIndicator);
	}
	
	public override void _Ready(){
		updateTimer = (Timer)GetNode("Update Timer");
		updateTimer.WaitTime = timeBetweenUpdates;
		
		destination = Vector2.Zero;
		
		commander = (CommanderUnit)GetParent().GetNode("Commander Unit");
		commanderUI = (CommanderControl)GetTree().Root.GetNode("Main/Commander UI");
		radarDisplay = (Control)GetTree().Root.GetNode("Main/Commander UI/Radar Display");
		
		multiInfoMessagePrefab = GD.Load<PackedScene>("res://Prefabs/Messages/Multi Info Message.tscn");
		radarIndicatorPrefab = GD.Load<PackedScene>("res://Prefabs/Radar Indicator.tscn");
		
		SetupIndicator();
	}
	
	public override void _PhysicsProcess(float delta){
		Rotation = GlobalPosition.DirectionTo(destination).Angle();
		Position = Position.MoveToward(destination, moveSpeed);
	}
	
	private void OnTimeout(){
		InfoPacket message = new InfoPacket();
		message.vector2 = GlobalPosition;
		message.angle = GlobalRotation;
		message.unit = this;
		message.type = InfoType.PositionUpdate;
		
		List<Unit> commanderList = new List<Unit>();
		commanderList.Add(commander);
		
		MultiInfoMessage msg = (MultiInfoMessage)multiInfoMessagePrefab.Instance();
		msg.GlobalPosition = GlobalPosition;
		radarDisplay.AddChild(msg);
		
		msg.Init(message, commanderList);
	}
	
	public override void ReceiveOrders(Orders orders){
		GD.Print("Received orders...");
		this.orders = orders;
		if (orders.destination != null){
			GD.Print("Accepted orders and set new destination as " + orders.destination);
			SetDestination(orders.destination);
		}
	}
	
	private void SetDestination(Vector2 destination){
		this.destination = destination;
	}
}

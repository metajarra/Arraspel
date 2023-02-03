using Godot;
using System;

// PLAYER CONTROLLER
// This is the 2D platformer player controller from Default Platformer

/* Table of contents
	Presets + variables
		Horizontal movement
			Presets for movement values
			Move input detection
		Jumping
			Presets for movement values
			Jump State enum and current state
			Jump input detection
			
			Preset for bounce air control multiplier
			Bounce input detection
		Walls
			Wall climbing
				Presets for movement values
				Climb input detection
			Wall holding
				Custom RayCast2Ds for wall contact detection
				Hold/release input detection
		Contact
			Contact state enum and current state

	Initialization
		Gets wall hold raycasts for left and right sides

	Physics process
		Determines contact state
			On_Floor
			Holding_Wall
			Ascending/Descending
		Calculates force of gravity
		Catches falling on wall
		Calculates vertical movement (if holding wall)
		Calculates horizontal movement
		Calculates bounce
		Calculates jump
			Floor jump
			Wall jump
		Applies movement

	Interface method implementation
		Death
			Die

	Useful methods
		Lerp
		IsOnWallRaycast
*/

/* Description
	This is a 2D platformer player controller. It comes from the Default Platformer template.
	This handles the following behaviours:
		Horizontal movement
			Acceleration
			Slowing
		Air control
		Jumping
			Differential gravity when ascending/descending
			Wall jumping
				Climbing up/down walls
*/

/* Prerequisites
	This class must extend the following interfaces:
		Death
	This class must be implemented on a KinematicBody2D with a collider
	This class must have a first-generation child node with the InputController script on it

	This class should have a first-generation child node of the Collision Detector prefab. This is an Area2D with CollisionDetector.cs
*/

public class Movement : KinematicBody2D, Death
{
	// Modify the presets as necessary
	
	// Variables related to horizontal movement
	[Export] private float MaxHorizontalMoveSpeed = 300;	// Max horizontal move speed
	[Export] private float AccelerationRate = 0.6f;			// Rate of acceleration (towards max speed)
	[Export] private float StopRate = 0.8f;					// Rate of deceleration (towards 0)
	[Export] private float AirControlMultiplier = 0.4f;		// Air control multiplier. 0 is no air control, 1 is full control
	private int MoveInput_LR = 0;							// Move input left/right, represents input from player (or other source?)
	public void Move_LR(int direction) { MoveInput_LR = direction; }
	
	// Variables related to jumping
	[Export] private float JumpInitialForce = 380;				// Player's upward velocity when jump begins
	[Export] private float ForceOfGravityWhenAscending = 14;	// Force of gravity when player's upward velocity > 0
	[Export] private float ForceOfGravityWhenDescending = 10;	// Force of gravity when player's upward velocity <= 0
	
	public enum JumpState { Can_Jump, Cant_Jump };
	public JumpState JState { get; private set; } = JumpState.Cant_Jump;	// Current status of jump. Note that this is preset for a player above the ground - if the player spawns on the ground, set to Can_Jump
	
	private bool JumpPressed;
	private float JumpMultiplier = 1;
	public void Jump(float multi = 1) { JumpPressed = true; JumpMultiplier = multi; }
	
	// This is separate from Jump since it's used by the bouncing platform (and possibly other things)
	private bool BouncePressed;
	private bool ContinuousAirBounce = false;
	private Vector2 BounceDirection;
	[Export] private float BounceAirControlMultiplier = 0.02f;
	public void Bounce(Vector2 direction) { BouncePressed = true; BounceDirection = direction; }
	
	[Export] private float VerticalMoveSpeed = 125;	// Move speed up/down, when holding wall
	private int MoveInput_UD = 0;					// Move input up/down, when holding wall
	public void Move_UD(int direction) { MoveInput_UD = direction; }
	
	private RayCast2D WCRLeft;
	private RayCast2D WCRRight;
	
	private bool HoldingWallInput;
	private bool HoldingWall;
	public void Hold() { HoldingWallInput = true; HoldingWall = true; }
	public void Release() { HoldingWallInput = false; HoldingWall = false; }
	
	// Variables related to floor/wall contact
	public enum ContactState { On_Floor, Holding_Wall, Ascending, Descending }
	public ContactState CState { get; private set; } = ContactState.Descending;	// Current status of contact. Note that this is preset for a player above the ground - if the player spawns ont he ground, set to On_Floor
	
	// Parameters to be passed into MoveAndSlide
	private Vector2 MoveDirection;
	public Vector2 MoveDirectionCopy { get => MoveDirection; }
	
	public override void _Ready(){
		WCRLeft = (RayCast2D)GetNode("WCR Left");
		WCRRight = (RayCast2D)GetNode("WCR Right");
	}
	
	// Calculates and applies movement
	public override void _PhysicsProcess(float delta){
		// Determines contact state
		if (IsOnFloor()){
			JState = JumpState.Can_Jump;
			CState = ContactState.On_Floor;
			if (HoldingWallInput) HoldingWall = true;
		}
		
		else if (IsOnWallRaycast() && HoldingWall){
			JState = JumpState.Can_Jump;
			CState = ContactState.Holding_Wall;
		}
		
		else {
			JState = JumpState.Cant_Jump;
			
			if (IsOnCeiling()) { MoveDirection.y = 0; ContinuousAirBounce = false; }
			if (!IsOnWallRaycast() && HoldingWallInput) HoldingWall = true;
			if (MoveDirection.y < 0) CState = ContactState.Ascending;
			else CState = ContactState.Descending;
		}
		
		// Calculates force of gravity
		switch (CState){
			case ContactState.Ascending:
				MoveDirection.y += ForceOfGravityWhenAscending;
				break;
			case ContactState.Descending:
				MoveDirection.y += ForceOfGravityWhenDescending;
				break;
			default:
				MoveDirection.y = 0;
				if (ContinuousAirBounce) ContinuousAirBounce = false;
				break;
		} 
		
		// Catches falling on wall
		if (IsOnWallRaycast() && HoldingWallInput && CState == ContactState.Descending){
			CState = ContactState.Holding_Wall;
			HoldingWall = true;
		}
		
		// Calculates vertical movement (if holding wall)
		if (CState == ContactState.Holding_Wall){
			MoveDirection.y = MoveInput_UD * VerticalMoveSpeed;
		}
		
		// Calculates horizontal movement
		float trueAirControl = 1;
		if (CState != ContactState.On_Floor && CState != ContactState.Holding_Wall) trueAirControl = AirControlMultiplier;
		if (ContinuousAirBounce) trueAirControl = BounceAirControlMultiplier;
		if (CState != ContactState.Holding_Wall){
			if (MoveInput_LR == 0) MoveDirection.x = Lerp(MoveDirection.x, 0, StopRate * trueAirControl);
			else MoveDirection.x = Lerp(MoveDirection.x, MaxHorizontalMoveSpeed * MoveInput_LR, AccelerationRate * trueAirControl);
		}
		
		// Calculates bounce
		if (BouncePressed){
			MoveDirection.x -= BounceDirection.x;
			MoveDirection.y -= BounceDirection.y;
			BouncePressed = false;
			ContinuousAirBounce = true;
		}
		// Calculates jump
		else if (JumpPressed && JState == JumpState.Can_Jump){
			// Jump from floor (normal jump)
			if (CState == ContactState.On_Floor){
				MoveDirection.y -= JumpInitialForce * JumpMultiplier;
			}
			
			// Wall jump
			else if (CState == ContactState.Holding_Wall){
				MoveDirection.y -= JumpInitialForce;
				MoveDirection.x = MaxHorizontalMoveSpeed * MoveInput_LR;
				HoldingWall = false;
			}
		}
		JumpPressed = false;
		
		// Applies movement
		MoveAndSlide(MoveDirection, new Vector2(0, -1));
	}
	
	// Interface method implementation
	// Death
	public void Die(){
		LevelManager manager = GetNode<LevelManager>("/root/LevelManager");
		manager.ReloadCurrentLevel();
	}
	
	// Useful methods
	private float Lerp(float _from, float _to, float _rate){
		if (_from == _to) return _from;
		return _from * (1 - _rate) + _to * _rate;
	}
	
	private bool IsOnWallRaycast(){
		if (IsOnWall()) return true;
		return (WCRLeft.IsColliding() || WCRRight.IsColliding());
	}
}

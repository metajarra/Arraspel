using Godot;
using System;

public class PlayerAnimationController : Node
{
	private Movement PlayerMovement;
	private AnimatedSprite PlayerSprite;
	
	[Export] private SpriteFrames IdleAnim;
	[Export] private SpriteFrames WalkAnim;
	[Export] private SpriteFrames AscendAnim;
	[Export] private SpriteFrames DescendAnim;
	[Export] private SpriteFrames WallHoldAnim;
	[Export] private SpriteFrames WallClimbAnim;
	
	public override void _Ready(){
		PlayerMovement = (Movement)GetParent();
		PlayerSprite = (AnimatedSprite)GetParent().GetNode("Sprite");
	}
	
	public override void _Process(float delta){
		switch (PlayerMovement.CState) {
			case Movement.ContactState.Ascending:
				PlayerSprite.Frames = AscendAnim;
				break;
			case Movement.ContactState.Descending:
				PlayerSprite.Frames = DescendAnim;
				break;
			case Movement.ContactState.Holding_Wall:
				if (PlayerMovement.MoveDirectionCopy.y != 0)
					PlayerSprite.Frames = WallClimbAnim;
				else
					PlayerSprite.Frames = WallHoldAnim;
				break;
			default: // On floor
				if (PlayerMovement.MoveDirectionCopy.x > 0.1
					|| PlayerMovement.MoveDirectionCopy.x < -0.1)
					PlayerSprite.Frames = WalkAnim;
				else
					PlayerSprite.Frames = IdleAnim;
				break;
		}
		
		if (PlayerMovement.MoveDirectionCopy.x < 0)
			PlayerSprite.FlipH = true;
		else if (PlayerMovement.MoveDirectionCopy.x > 0)	// The reason for this is to prevent the player sprite
			PlayerSprite.FlipH = false;						// flipping at exactly x = 0
	}
}

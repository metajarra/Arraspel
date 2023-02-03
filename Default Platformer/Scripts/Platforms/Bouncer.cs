using Godot;
using System;

public class Bouncer : Node
{
	[Export] private SpriteFrames IdleAnimation;			// Animation played while idle
	[Export] private SpriteFrames BounceAnimation;			// Animation played while bouncing
	private AnimatedSprite PlatformSprite;				// The sprite used by the platform
	
	private bool Bouncing;
	[Export] private float BounceForce = 650;
	
	public override void _Ready(){
		PlatformSprite = (AnimatedSprite)GetParent().GetParent().GetNode("AnimatedSprite");
		
		GetParent().Connect("body_entered", this, "OnStep");
		PlatformSprite.Connect("animation_finished", this, "OnAnimationEnd");
	}
	
	private void OnStep(Node body){
		if (body.Name != "Player") return;
		
		KinematicBody2D player = (KinematicBody2D)body;
		Vector2 normal = player.MoveAndCollide(((Node2D)GetParent().GetParent()).GlobalPosition - player.GlobalPosition).Normal;
		Vector2 localUp = ((Node2D)GetParent().GetParent()).Transform.y.Normalized() * -1;
		
		normal = StepVector2(normal);
		localUp = StepVector2(localUp);
		
		if (normal == localUp){
			((Movement)body).Bounce(localUp * BounceForce * -1);
			
			Bouncing = true;
			PlatformSprite.Frames = BounceAnimation;
			PlatformSprite.Frame = 0;
		}
	}
	
	private void OnAnimationEnd() {
		if (Bouncing){
			PlatformSprite.Frames = IdleAnimation;
			PlatformSprite.Frame = 0;
			PlatformSprite.Playing = true;
			Bouncing = false;
		}
	}
	
	private Vector2 StepVector2(Vector2 original, float step = 0.01f){
		return new Vector2(Mathf.Stepify(original.x, step), Mathf.Stepify(original.y, step));
	}
}

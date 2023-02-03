using Godot;
using System;

public class Disintegrator : Node
{
	[Export] private SpriteFrames IdleAnimation;			// Animation played while idle
	[Export] private SpriteFrames ShakeAnimation;			// Animation played while about to disintegrate
	[Export] private SpriteFrames DisintegrateAnimation;	// Animation played while disintegrating
	[Export] private SpriteFrames ReintegrateAnimation;		// Animation played while reintegrating
	
	[Export] private bool CanReintegrate = true;
	[Export] private float TimeToDisintegrate = 1;
	[Export] private float TimeToReintegrate = 5;
	private Timer DTimer;	// Disintegrate timer
	private Timer RTimer;	// Reintegrate timer
	
	private CollisionShape2D DetectorCollider;	// The collider used by the collision detector node
	private CollisionShape2D HitCollider;		// The collider used by the platform
	private AnimatedSprite PlatformSprite;				// The sprite used by the platform
	
	private bool CurrentlyDisintegrating = false;
	
	public override void _Ready(){
		DTimer = (Timer)GetNode("Disintegrate Timer");
		RTimer = (Timer)GetNode("Reintegrate Timer");
		
		DTimer.WaitTime = TimeToDisintegrate;
		RTimer.WaitTime = TimeToReintegrate;
		
		DetectorCollider = (CollisionShape2D)GetParent()?.GetNode("Collider");
		HitCollider = (CollisionShape2D)GetParent().GetParent().GetNode("Collider");
		PlatformSprite = (AnimatedSprite)GetParent().GetParent().GetNode("AnimatedSprite");
		
		GetParent().Connect("body_entered", this, "OnStep");
		PlatformSprite.Connect("animation_finished", this, "OnAnimationEnd");
	}
	
	private void OnStep(Node body) {
		if (body.Name != "Player" || CurrentlyDisintegrating) return;
		
		CurrentlyDisintegrating = true;
		
		PlatformSprite.Frames = ShakeAnimation;
		PlatformSprite.Frame = 0;
		
		DTimer.Start();
	}
	
	private void OnAnimationEnd() {
		if (!CurrentlyDisintegrating) {
			PlatformSprite.Frames = IdleAnimation;
			PlatformSprite.Frame = 0;
			PlatformSprite.Playing = true;
		}
	}
	
	private void OnDTimeout()
	{
		PlatformSprite.Frames = DisintegrateAnimation;
		PlatformSprite.Frame = 0;
		
		DetectorCollider.Disabled = true;
		HitCollider.Disabled = true;
		
		RTimer.Start();
	}
	
	private void OnRTimeout()
	{
		// Replace with QueueFree() if disintegrated platforms are invisible to save on performance
		if (!CanReintegrate) return; 
		
		CurrentlyDisintegrating = false;
		
		PlatformSprite.Frames = ReintegrateAnimation;
		PlatformSprite.Frame = 0;
		PlatformSprite.Playing = true;
		
		DetectorCollider.Disabled = false;
		HitCollider.Disabled = false;
	}
}

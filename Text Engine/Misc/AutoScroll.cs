using Godot;
using System;

// This script automatically scrolls to the bottom of a VBoxContainer child of this ScrollContainer
// It should be attached to a ScollContainer with a VBoxContainer child

public class AutoScroll : ScrollContainer
{
	private Action<EventParams> OnAddNodeListener; // This action is triggered whenever a new node is added
	
	public override void _Ready(){
		OnAddNodeListener = new Action<EventParams>(Scroll);
		EventManager.StartListening("OnAddNode", OnAddNodeListener);
	}
	
	private void Scroll(EventParams args = default){
		GetVScrollbar().AllowGreater = true; // Temporarily allows overscrolling to reach right position
		ScrollVertical = (int)GetVScrollbar().MaxValue + 54; // Scrolls viewport
		GetVScrollbar().AllowGreater = false; // Disallows overscrolling
	}
}

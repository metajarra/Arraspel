using Godot;
using System;

public class AutoScroll : ScrollContainer
{
	private Action<EventParams> OnAddNodeListener;
	
	public override void _Ready(){
		OnAddNodeListener = new Action<EventParams>(Scroll);
		EventManager.StartListening("OnAddNode", OnAddNodeListener);
	}
	
	private void Scroll(EventParams args = default){
		GetVScrollbar().AllowGreater = true;
		ScrollVertical = (int)GetVScrollbar().MaxValue + 54;
		GetVScrollbar().AllowGreater = false;
	}
}

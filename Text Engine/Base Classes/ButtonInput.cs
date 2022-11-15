using Godot;
using System;
using System.Collections.Generic;

public class ButtonInput : Node
{
	public List<Button> buttons;
	public List<string> texts;
	public List<string> events;
	public List<EventParams> eventParams;
	
	public ButtonInput(){
		buttons = new List<Button>();
		texts = new List<string>();
		events = new List<string>();
		eventParams = new List<EventParams>();
	}
	
	public void AddButton(string text, string eventToTrigger, EventParams args = default){
		texts.Add(text);
		events.Add(eventToTrigger);
		eventParams.Add(args);
	}
	
	/*
	Toll the Great Bell once!
	Pull the Lever forward to engage the piston and pump...
	
	Toll the Great Bell twice!
	With push of Button fire the engine and spark turbine into life...
	
	Toll the Great Bell thrice!
	Sing praise to the god of all machines
	*/
	
	public Button CreateButton(int index){ 
		Button button = new Button();
		button.Name = texts[index];
		button.Text = texts[index];
		
		button.RectMinSize = new Vector2(20, 20);
		
		buttons.Add(button);
		
		Godot.Collections.Array name = new Godot.Collections.Array();
		name.Add(button.Name);
		button.Connect("pressed", this, "OnButtonPressed", name);
		
		return button;
	}
	
	private void OnButtonPressed(String name){
		// Find index of button
		int index = -1;
		int i = 0;
		
		while (index == -1){
			if (buttons[i].Name == name)
				index = i;
			
			i++;
		}
		
		// Trigger event with params
		EventManager.TriggerEvent(events[index], eventParams[index]);
	}
}

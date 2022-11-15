using Godot;
using System;
using System.Collections.Generic;

// This script handles the adding and storing of buttons and their actions
// It should be attached to a scene containing whatever Display node will hold the buttons, ideally an HBoxContainer

public class ButtonInput : Node
{
	public List<Button> buttons; // The list of added buttons
	public List<string> texts; // The list of button display texts
	public List<string> events; // The list of events triggered by button presses
	public List<EventParams> eventParams; // The list of parameters passed for button press events
	
	// Initialization
	public ButtonInput(){
		buttons = new List<Button>();
		texts = new List<string>();
		events = new List<string>();
		eventParams = new List<EventParams>();
	}
	
	// Handles adding new button
	// Should be called from some other script, ideally DisplayStorage
	
	/* Parameters represent the following:
		text: Text which will be displayed on the button
		eventToTrigger: Event to which the button will be connected to
		args (Optional): Arguments to be passed in button press event
	*/
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
	
	// Handles button creation
	// Should be called from another script, ideally Interpreter
	
	/* Parameters represent the following:
		index: The index at which the data this button will represent is stored. This button must have been added previously using AddButton
	*/
	public Button CreateButton(int index){ 
		Button button = new Button();
		button.Name = texts[index]; // Gets name
		button.Text = texts[index]; // Gets text
		
		button.RectMinSize = new Vector2(20, 20); // Sets minimum size so button is not too compressed by HBox and VBox
		
		buttons.Add(button); // Adds new button to button list
		
		// Connects button press to event trigger
		// Don't know how this works, please don't touch it
		Godot.Collections.Array name = new Godot.Collections.Array(); 
		name.Add(button.Name);
		button.Connect("pressed", this, "OnButtonPressed", name);
		
		return button; // Newly created button is returned. This is because, in the intended use, this button is now added as a child to the ButtonInput HBox containing it
	}
	
	// Handes button press events
	// Attempts to match name of pressed button to stored name, triggers correct event
	
	/* Parameters represent the following:
		name: The name of the pressed button
	*/
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

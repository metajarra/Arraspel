using Godot;
using System;
using System.Collections.Generic;

// This script handles the creation, deletion, and calling of events
// It should be attached to a node of type Node called "Event Manager"

public class EventManager : Node
{
	private Dictionary<string, Action<EventParams>> eventDictionary; // List of known Actions and their names
	
	// Finds instance of EventManager in scene    
	public static EventManager instance;
	
	public override void _Ready()
	{
		instance = (EventManager)GetParent().GetNode("Event Manager");
		instance.Init();
	}
	
	// Initializes eventDictionary in instance
	void Init() 
	{
		if (eventDictionary == null)
			eventDictionary = new Dictionary<string, Action<EventParams>>();
	}
	
	// Adds a listener to an Action - if the Action does not exist, create and add it to eventDictionary and add the listener
	
	/* Parameters represent the following:
		eventName: The name of the event to be added
		listener: Reference to the listener to be added
	*/
	public static void StartListening(string eventName, Action<EventParams> listener) 
	{
		Action<EventParams> thisEvent; // Reference to named event
		if (instance.eventDictionary.TryGetValue(eventName, out thisEvent)) // If the named Action exists:
		{
			thisEvent += listener;
			instance.eventDictionary[eventName] = thisEvent;
		}

		else // If the named Action does not exist:
		{
			thisEvent += listener;
			instance.eventDictionary.Add(eventName, thisEvent);
		}
	}
	
	// Removes listeners from an Action - used when the object listening is destroyed/disabled
	
	/* Parameters represent the following:
		eventName: The name of the event to be removed
		listener: Reference to the listener to be removed
	*/
	public static void StopListening(string eventName, Action<EventParams> listener) 
	{
		if (instance != null)
		{
			Action<EventParams> thisEvent; // Reference to named Action
			if (instance.eventDictionary.TryGetValue(eventName, out thisEvent)) // If attempt to find named Action is successful:
			{
				thisEvent -= listener;
				instance.eventDictionary[eventName] = thisEvent;
			}
		}
	}
	
	// Triggers an event with EventParams parameters
	
	/* Parameters represent the following:
		eventName: The name of the event to be triggered
		args (Optional): The parameters with which the event will be triggered
	*/
	public static void TriggerEvent(string eventName, EventParams args = default) 
	{
		Action<EventParams> thisEvent; // Reference to named Action
		if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
			thisEvent?.Invoke(args); // Invokes action with passed parameters
	}
}

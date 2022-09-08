using Godot;
using System;
using System.Collections.Generic;

public class EventManager : Node2D
{
	private Dictionary<string, Action<EventParams>> eventDictionary; // List of known Actions and their names
	
	// Finds instance of EventManager in scene    
	public static EventManager instance;
	
	public override void _Ready()
	{
		instance = (EventManager)GetParent().GetNode("Event Manager");
		instance.Init();
	}
	
	void Init() // Initializes eventDictionary in instance
	{
		if (eventDictionary == null)
			eventDictionary = new Dictionary<string, Action<EventParams>>();
	}
	
	public static void StartListening(string eventName, Action<EventParams> listener) // Adds a listener to an Action - if the Action does not exist, create and add it to eventDictionary and add the listener
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

	public static void StopListening(string eventName, Action<EventParams> listener) // Removes listeners from an Action - used when the object listening is destroyed/disabled
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

	public static void TriggerEvent(string eventName, EventParams args = default) // Triggers an event with EventParams parameters
	{
		Action<EventParams> thisEvent; // Reference to named Action
		if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
			thisEvent.Invoke(args); // Invokes action with passed parameters
	}
}

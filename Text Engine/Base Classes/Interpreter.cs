using Godot;
using Godot.Collections;
using System;

// This script handles the interpretation of stored data into an on-screen format
// This script should be attached to a VBoxContainer child of a ScrollContainer with the AutoScroll script

public class Interpreter : VBoxContainer
{
	private Scene currentScene; // The currently active scene
	
	private int currentSceneIndex; // The index where that scene is stored in DisplayStorage script
	private int currentIndex; // The index for the current display/input being read from that scene
	
	private Action<EventParams> onLoadSceneListener; // This action is triggered whenever a scene loading event is
	
	public override void _Ready(){
		// Initialization
		onLoadSceneListener = new Action<EventParams>(LoadScene);
		EventManager.StartListening("OnLoadScene", onLoadSceneListener);
		
		DisplayStorage.Init(); // Initializes DisplayStorage. This is a very important line, do not change it
		currentScene = LoadNewScene(currentSceneIndex); // Loads first scene
	}
	
	public override void _Process(float delta){
		if (Input.IsActionJustPressed("mouse_click")){ // Detects mouse click input
			if (currentScene.textStorage.Count > currentIndex){ // Attempts to increment scene text element
				// If successful, loads next scene text element
				LoadNextSceneElement(currentScene, currentIndex);
				
				currentIndex++;
				
				EventManager.TriggerEvent("OnAddNode");
			}
			
			else if (currentScene.inputStorage.Count > currentIndex - currentScene.textStorage.Count){ // If unsuccessful, attempts to load any button elements in scene
				// If successful, loads next scene input element
				LoadNextInputElement(currentScene, 0);
				
				currentIndex++;
				
				EventManager.TriggerEvent("OnAddNode");
			}
		}
	}
	
	// Loads new active scene
	
	/* Parameters represent the following:
		indexParam: The index of the new scene to be loaded
	*/
	private void LoadScene(EventParams indexParam){
		currentScene = LoadNewScene(indexParam.intParam[0]);
	}
	
	// Clears current data, returns next scene
	
	/* Parameters represent the following:
		index: The index of the new scene to be loaded
	*/
	public Scene LoadNewScene(int index){
		foreach (Node n in GetChildren()){ // Clears current scene
			RemoveChild(n);
			n.QueueFree();
		}
		
		currentIndex = 0;
		
		return DisplayStorage.sceneStorage[index];
	}
	
	// Loads next text element from currently active scene
	
	/* Parameters represent the following:
		scene: The scene from which to load that element (usually the current scene)
		index: The index (in scene.textStorage) to be loaded
	*/
	public void LoadNextSceneElement(Scene scene, int index){
		// Instantiates new text display prefab (change location as needed)
		Control text = (Control)GD.Load<PackedScene>("res://Prefabs/Display Type Prefabs/TextDisplay.tscn").Instance();
		RichTextLabel label = ((RichTextLabel)text.GetNode("Text"));
		label.Text = scene.textStorage[currentIndex].GetText(); // Sets prefab data to stored data
		
		AddChild(text);
		
		// Sets minumum size so text is not compressed/overlapping
		text.RectMinSize = new Vector2(0, label.GetContentHeight());
	}
	// Loads next input element from currently active scene
	
	/* Parameters represent the following:
		scene: The scene from which to load the element (usually the current scene)
		index: The index (in scene.inputStorage) to be loaded
	*/
	public void LoadNextInputElement(Scene scene, int index){
		// Instantiates new button input prefab (change location as needed)
		// Note that this is  intended to be an HBoxContainer, and does not represent a single button
		ButtonInput buttons = (ButtonInput)GD.Load<PackedScene>("res://Prefabs/Input Type Prefabs/ButtonInput.tscn").Instance();
		
		// Sets prefab data to stored data
		buttons.buttons = currentScene.inputStorage[index].buttons;
		buttons.texts = currentScene.inputStorage[index].texts;
		buttons.events = currentScene.inputStorage[index].events;
		buttons.eventParams = currentScene.inputStorage[index].eventParams;
		
		AddChild(buttons);
		
		// Creates buttons to be added as children
		for (int i = 0; i < buttons.texts.Count; i++){
			buttons.AddChild(buttons.CreateButton(i));
		}
	}
}

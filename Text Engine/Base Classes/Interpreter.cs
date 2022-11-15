using Godot;
using Godot.Collections;
using System;

public class Interpreter : VBoxContainer
{
	private Scene currentScene;
	
	private int currentSceneIndex;
	private int currentIndex;
	
	private Action<EventParams> onLoadSceneListener;
	
	public override void _Ready(){
		onLoadSceneListener = new Action<EventParams>(LoadScene);
		EventManager.StartListening("OnLoadScene", onLoadSceneListener);
		
		DisplayStorage.Init();
		currentScene = LoadNewScene(currentSceneIndex);
	}
	
	public override void _Process(float delta){
		if (Input.IsActionJustPressed("mouse_click")){
			if (currentScene.textStorage.Count > currentIndex){
				LoadNextSceneElement(currentScene, currentIndex);
				
				currentIndex++;
				
				EventManager.TriggerEvent("OnAddNode");
			}
			
			else if (currentScene.inputStorage.Count > currentIndex - currentScene.textStorage.Count){
				LoadNextInputElement(currentScene, 0);
				
				currentIndex++;
				
				EventManager.TriggerEvent("OnAddNode");
			}
		}
	}
	
	private void LoadScene(EventParams indexParam){
		currentScene = LoadNewScene(indexParam.intParam[0]);
	}
	
	public Scene LoadNewScene(int index){
		foreach (Node n in GetChildren()){
			RemoveChild(n);
			n.QueueFree();
		}
		
		currentIndex = 0;
		
		return DisplayStorage.sceneStorage[index];
	}
	
	public void LoadNextSceneElement(Scene scene, int index){
		Control text = (Control)GD.Load<PackedScene>("res://Prefabs/Display Type Prefabs/TextDisplay.tscn").Instance();
		RichTextLabel label = ((RichTextLabel)text.GetNode("Text"));
		label.Text = scene.textStorage[currentIndex].GetText();
		
		AddChild(text);
		
		text.RectMinSize = new Vector2(0, label.GetContentHeight());
	}
	
	public void LoadNextInputElement(Scene scene, int index){
		ButtonInput buttons = (ButtonInput)GD.Load<PackedScene>("res://Prefabs/Input Type Prefabs/ButtonInput.tscn").Instance();
		buttons.buttons = currentScene.inputStorage[index].buttons;
		buttons.texts = currentScene.inputStorage[index].texts;
		buttons.events = currentScene.inputStorage[index].events;
		buttons.eventParams = currentScene.inputStorage[index].eventParams;
		
		AddChild(buttons);
		
		for (int i = 0; i < buttons.texts.Count; i++){
			buttons.AddChild(buttons.CreateButton(i));
		}
	}
}

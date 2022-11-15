using Godot;
using System;
using System.Collections.Generic;

// This script handles storage and modification of text and input data in a scene format
// This script should not be attached to any node

public class Scene
{
	public List<TextDisplay> textStorage; // The stored text data
	public List<ButtonInput> inputStorage; // The stored input data
	
	public Scene(){
		Init();
	}
	
	// Initialization
	private void Init(){
		textStorage = new List<TextDisplay>();
		inputStorage = new List<ButtonInput>();
	}
	
	// Note that the below methods are intended to be used in the DisplayStorage class
	public void AddText(TextDisplay text){
		textStorage.Add(text);
	}
	
	public void ReplaceText(int index, TextDisplay text){
		if (index <= textStorage.Count - 1)
			textStorage[index] = text;
	}
	
	public void RemoveText(int index){
		if (index <= textStorage.Count - 1)
			textStorage.Remove(textStorage[index]);
	}
	
	public void ResetAllText(){
		for (int i = textStorage.Count - 1; i >= 0; i--){
			textStorage.Remove(textStorage[i]);
		}
	}
	
	public void AddButton(ButtonInput input){
		inputStorage.Add(input);
	}
	
	public void ReplaceButton(int index, ButtonInput button){
		if (index <= inputStorage.Count - 1)
			inputStorage[index] = button;
	}
	
	public void RemoveButton(int index){
		if (index <= inputStorage.Count - 1)
			inputStorage.Remove(inputStorage[index]);
	}
	
	public void ResetAllButtons(){
		for (int i = inputStorage.Count - 1; i >= 0; i--){
			inputStorage.Remove(inputStorage[i]);
		}
	}
}

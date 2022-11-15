using Godot;
using System;

// This script handles the storage and display of basic text
// This script should not be attached to any node

public class TextDisplay : Node
{
	private string text; // The stored text
	
	public TextDisplay(){
		text = "";
	}
	
	public TextDisplay(string text){
		this.text = text;
	}
	
	public void SetText(string text){
		this.text = text;
	}
	
	public string GetText(){
		return text;
	}
	
	public static TextDisplay GetInstance(){
		return new TextDisplay();
	}
	
	public static TextDisplay GetInstance(string text){
		return new TextDisplay(text);
	}
	
	// Returns display with text
	public Control Display(){
		// instantiate a display node of the right type with the correct data
		Control instance = (Control)GD.Load<PackedScene>("res://Prefabs/Display Type Prefabs/TextDisplay.tscn").Instance();
		RichTextLabel instanceText = (RichTextLabel)instance.GetNode("Text");
		instanceText.Text = text;
		
		return instance;
	}
}

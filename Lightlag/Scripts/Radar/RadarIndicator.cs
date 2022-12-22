using Godot;
using System;

public class RadarIndicator : Control
{
	private Unit linkedUnit;
	public void LinkUnit(Unit unit){ linkedUnit = unit; }
	public Unit GetLinkedUnit(){ return linkedUnit; }
	
	private Control image;
	private Control highlight;
	private Control hoverInfo;
	private Line2D trail;
	
	public override void _Ready(){
		image = (Control)GetNode("Image");
		highlight = (Control)GetNode("Image/Highlight");
		hoverInfo = (Control)GetNode("Hover Info");
		trail = (Line2D)GetNode("Trail");
	}
	
	public void InitIndicator(string shipName, string shipID, string shipNavy, Unit unit){
		((Label)GetNode("Hover Info/Name")).Text = shipName;
		((Label)GetNode("Hover Info/ID")).Text = shipID;
		((Label)GetNode("Hover Info/Navy")).Text = shipNavy;
		LinkUnit(unit);
	}
	
	public void UpdateIndicator(float rotation, Vector2 position){
		image.RectRotation = rotation;
		RectPosition = position;
		
		trail.GlobalPosition = Vector2.Zero;
		trail.AddPoint(RectGlobalPosition + RectSize / 2);
		
		if (trail.GetPointCount() >= 15){
			trail.RemovePoint(0);
		}
	}
	
	private void OnMouseOver() { hoverInfo.Visible = true; highlight.Visible = true; }
	private void OnMouseExit() { hoverInfo.Visible = false; highlight.Visible = false; }
}

using Godot;
using System;
using System.Collections.Generic;

public class DisplayStorage
{
	public static List<Scene> sceneStorage;
	
	private static void AddScene(Scene scene) {
		sceneStorage.Add(scene);
	}
	
	public static void Init(){
		sceneStorage = new List<Scene>();
		
		// Scenes to store go below here
		Scene DarthPlagueis = new Scene();
		
		DarthPlagueis.AddText(new TextDisplay("Did you ever hear the tragedy of Darth Plagueis the Wise?"));
		DarthPlagueis.AddText(new TextDisplay("I thought not. It's not a story the Jedi would tell you. It's a Sith legend."));
		DarthPlagueis.AddText(new TextDisplay("Darth Plagueis was a Dark Lord of the Sith, so powerful and so wise he could use the Force to influence the midi-chlorians to create . . . life."));
		DarthPlagueis.AddText(new TextDisplay("He was so powerful, he could even keep the ones he cared about from dying."));
		DarthPlagueis.AddText(new TextDisplay("The Dark Side of the Force is a pathway to many abilities that some consider to be . . . unnatural."));
		DarthPlagueis.AddText(new TextDisplay("He became so powerful, the only thing he was afraid of was losing his power, which eventually, of course, he did."));
		DarthPlagueis.AddText(new TextDisplay("Unfortunately, he taught his apprentice everything he knew, then his apprentice killed him in his sleep."));
		DarthPlagueis.AddText(new TextDisplay("Ironic. He could save others from death, but not himself."));
		
		ButtonInput buttons = new ButtonInput();
		
		EventParams narratorParams = new EventParams();
		narratorParams.intParam = new int[] { 1 };
		buttons.AddButton("Proceed to Narrator", "OnLoadScene", narratorParams);
		buttons.AddButton("Try that again", "OnResetText");
		
		DarthPlagueis.AddButton(buttons);
		
		AddScene(DarthPlagueis);
		
		// ------------------------------
		
		Scene Narrator = new Scene();
		
		Narrator.AddText(new TextDisplay("Ruin has come to our family."));
		Narrator.AddText(new TextDisplay("You remember our old house, opulent and imperial. Gazing proudly from its stoic perch above the moor."));
		Narrator.AddText(new TextDisplay("I lived all my years in that ancient, rumour-shadowed manor. Fattened by decadence and luxury. And yet, I began to tire of conventional extravagence."));
		Narrator.AddText(new TextDisplay("Singular, unsettling tales suggested that the mansion itself was a gateway to some fabulous and unnamable power. With relic and ritual, I bent every effort towards the excavation and recovery of those long-buried secrets, exhausting what remained of our family fortune on swarthy workmen and sturdy shovels."));
		Narrator.AddText(new TextDisplay("At last, in the salt-soaked crags beneath the lowest foundations we unearthed that damnable portal of antediluvian evil. Our every step unsettled the ancient earth, but we were in a realm of death and madness! In the end, I alone fled laughing and wailing through those blackened arcades of antiquity. Until consciousness failed me."));
		Narrator.AddText(new TextDisplay("You remember our venerable house, opulent and imperial. It is a festering abomination!"));
		Narrator.AddText(new TextDisplay("I beg you, return home, claim your birthright, and deliver our family from the ravenous clutching shadows of the darkest dungeon."));
		
		AddScene(Narrator);
	}
}

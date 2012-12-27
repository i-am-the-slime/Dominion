using UnityEngine;
using System.Collections;

public class Card {
	private GameObject game_object;
	private string name;

	public Card(string name, string expansion){
		this.name = name;
		this.game_object = (GameObject) MonoBehaviour.Instantiate(Resources.Load("Prefabs/card"));
		Material[] materials = this.game_object.renderer.materials;
		Texture2D tex = (Texture2D) (Resources.Load("textures_"+Settings.LANG+"/"+expansion+"/"+name));
		materials[1].SetTexture("_MainTex", tex);
		this.game_object.renderer.materials = materials;	
	}

	public GameObject getGameObject(){
		return this.game_object;
	}
}

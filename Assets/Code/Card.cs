using UnityEngine;
using System.Collections;

public class Card {
	private GameObject game_object;
	private string name;

	public Card(string name, string expansion, bool face_down){
		this.name = name;
		this.game_object = (GameObject) MonoBehaviour.Instantiate(Resources.Load("Prefabs/card"));
		if (!face_down){
			this.game_object.transform.Rotate(new Vector3(1.0f, 0.0f, 0.0f), 180.0f);
		}
		Material[] materials = this.game_object.renderer.materials;
		Texture2D tex = (Texture2D) (Resources.Load("textures_"+Settings.LANG+"/"+expansion+"/"+name));
		materials[1].SetTexture("_MainTex", tex);
		this.game_object.renderer.materials = materials;	
	}

	public GameObject getGameObject(){
		return this.game_object;
	}
}

using UnityEngine;
using System.Collections;

public class Cam : MonoBehaviour {
	public enum Views {Hand, Table};
	
	public void ChangeViewTo(Views view){
		Vector3 pos;
		Vector3 rot;
		switch(view){
			case Views.Hand:
				pos = new Vector3(-37.0f, 76.0f, -500.0f);
				rot = new Vector3(38.0f, 0.0f, 0.0f);
				break;
			case Views.Table:
				pos = new Vector3(-37.0f, 81.4f, -488.84f);
				rot = new Vector3(90.0f, 0.0f, 0.0f);
				break;
			default:
				return;
		}	
		iTween.MoveTo(gameObject, pos, 2.0f);
		iTween.RotateTo(gameObject,  rot, 2.0f);	
	}
}

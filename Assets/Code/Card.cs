using UnityEngine;
using System.Collections;

public delegate void CardClickHandler(Card card);

public abstract class Card : MonoBehaviour {
	public event CardClickHandler CardClicked;
	private bool showCardInfo = false;
	
	public void OnMouseDown() {
		if (CardClicked != null) {
			CardClicked(this);
		}
	}
	
	public void OnMouseOver() {
		if (Input.GetMouseButtonDown(1)) {
			showCardInfo = true;
		}
		
		if (Input.GetMouseButtonUp(1)) {
			showCardInfo = false;
		}
	}
	
	public void OnMouseExit() {
		showCardInfo = false;
	}
	
	public void OnGUI() {
		if (showCardInfo) {
			GUI.Box(new Rect(Screen.width/2-326/2,Screen.height/2-503/2,326,503), renderer.materials[1].mainTexture);
		}
	}

    public virtual void Play(IPlayer player)
    {
        
    }

    public virtual int GetScore()
    {
        return 0;
    }

    public abstract bool IsPlayable();
    public abstract int GetCost();
}

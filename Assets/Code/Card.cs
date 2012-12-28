using System;
using UnityEngine;
using System.Collections;

public delegate void CardClickHandler(Card card);

public abstract class Card : MonoBehaviour {
	public event CardClickHandler CardClicked;
	private bool _showCardInfo = false;

    [Flags]
    public enum CardFlags {
        Action = 1,
        Treasure = 2,
        Victory = 4
    }

    public abstract int Cost { get; }
    public abstract int Score { get; }
    public abstract CardFlags Flags { get; }
    

    public void OnMouseDown() {
		if (CardClicked != null) {
			CardClicked(this);
		}
	}
	
	public void OnMouseOver() {
		if (Input.GetMouseButtonDown(1)) {
			_showCardInfo = true;
		}
		
		if (Input.GetMouseButtonUp(1)) {
			_showCardInfo = false;
		}
	}
	
	public void OnMouseExit() {
		_showCardInfo = false;
	}
	
	public void OnGUI() {
		if (_showCardInfo) {
			GUI.Box(new Rect(Screen.width/2-326/2,Screen.height/2-503/2,326,503), renderer.materials[1].mainTexture);
		}
	}

    public virtual void Play(IPlayer player)
    {
        
    }
}

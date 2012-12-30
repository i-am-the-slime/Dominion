using System;
using UnityEngine;
using System.Collections;

public delegate void CardClickHandler(Card card);

public abstract class Card : MonoBehaviour {
	public event CardClickHandler CardClicked;
	public bool ShowCardInfo = false;
    private Rect _cardInfoRect = new Rect(Screen.width / 2 - 326 / 2, Screen.height / 2 - 503 / 2, 326, 503);
    [Flags]
    public enum CardFlags {
        Action = 1,
        Treasure = 2,
        Victory = 4,
        Attack = 8
    }

    public abstract int Cost { get; }
    public abstract int Score { get; }
    public abstract CardFlags Flags { get; }
    

    public void OnMouseDown() {
		if (CardClicked != null) {
			CardClicked(this);
		}
	}

    public void GreyOut() {
        Material[] mats = gameObject.renderer.materials;
        mats[1].SetColor("_Color", new Color(0.7f, 0.7f, 0.7f));
        gameObject.renderer.materials = mats;
    }

    public void UnGreyOut() {
        Material[] mats = gameObject.renderer.materials;
        mats[1].SetColor("_Color", new Color(1, 1, 1));
        gameObject.renderer.materials = mats;
    }

	public void OnGUI() {
		if (ShowCardInfo) {
			GUI.Box(_cardInfoRect, renderer.materials[1].mainTexture);
		}
	}

    public virtual IEnumerator Play(Player player)
    {
        return null;
    }
}

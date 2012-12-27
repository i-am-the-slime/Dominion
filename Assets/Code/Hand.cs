using UnityEngine;
using System.Collections;

public class Hand {
	ArrayList cards;

	public Hand(){
		this.cards = new ArrayList();
	}	
	public void DrawCard(Card card){
		this.cards.Add (card);
		iTween.MoveTo (card.getGameObject(), new Vector3(-38.9f, 70.4f, -492.4f), 2.0f);
		iTween.RotateTo (card.getGameObject(), new Vector3(325.0f, 0.0f, 0.0f), 2.0f);
	}
}

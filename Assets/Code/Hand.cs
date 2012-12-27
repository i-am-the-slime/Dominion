using UnityEngine;
using System.Collections;

public class Hand : MonoBehaviour {
	private IList cards = new ArrayList();
	private const float cardWidth = 1.0f;

	public void DrawCard(Card card){
		this.cards.Add(card);
		
		int index = 0;
		foreach (Card c in cards) {
			iTween.MoveTo(c.getGameObject(), transform.position + CalcCardPosition(index), 2.0f);
			iTween.RotateTo(c.getGameObject(), transform.rotation.eulerAngles, 2.0f);
			index++;
		}
	}
	
	private Vector3 CalcCardPosition(int index) {
		float centerIndex = (cards.Count - 1) / 2.0f;
		float xPos = (index - centerIndex) * cardWidth;
		return new Vector3(xPos, 0.0f, 0.0f);
	}
}

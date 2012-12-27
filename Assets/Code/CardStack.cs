using UnityEngine;
using System.Collections;

public class CardStack : MonoBehaviour {
	private Stack stack = new Stack();
	public bool faceUp;
	
	public Card Peek(){
		return this.stack.Peek() as Card;
	}
	public void Push(Card card){
		Vector3 s = new Vector3(card.transform.position.x, card.transform.position.y, card.transform.position.z);
		Vector3 z = transform.position + new Vector3(0.0f, this.stack.Count*0.01f, 0.0f);
		Vector3 intermediate = s + 0.5f *(z-s);
		intermediate.y+=1.0f;
		iTween.MoveTo(card.gameObject, iTween.Hash("path", new Vector3[]{s, intermediate, z}, "time", 1.0f));
		if (faceUp){
			iTween.RotateTo(card.gameObject, transform.rotation.eulerAngles, 1.0f);
		}
		else {
			iTween.RotateTo(card.gameObject, transform.rotation.eulerAngles - new Vector3(180, 0, 0), 1.0f);
		}
		this.stack.Push(card);
	}
	public Card Pop(){
		return this.stack.Pop() as Card;
	}
}

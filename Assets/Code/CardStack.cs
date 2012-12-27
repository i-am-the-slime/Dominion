using UnityEngine;
using System.Collections;

public class CardStack : MonoBehaviour {
	private Stack stack = new Stack();
	
	public Card Peek(){
		return (Card)this.stack.Peek();
	}	
	public void Push(Card card){
		Vector3 s = new Vector3(card.getGameObject().transform.position.x, card.getGameObject().transform.position.y, card.getGameObject().transform.position.z);
		Vector3 z = transform.position + new Vector3(0.0f, this.stack.Count*0.01f, 0.0f);
		Vector3 intermediate = s + 0.5f *(z-s);
		intermediate.y+=1.0f;
		iTween.MoveTo(card.getGameObject(), iTween.Hash("path", new Vector3[]{s, intermediate, z}, "time", 1.0f));
		this.stack.Push(card);
	}
	public Card Pop(){
		return (Card)this.stack.Pop();
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardStack : MonoBehaviour {
	private Stack stack = new Stack();
	public bool faceUp;
	public bool untidy;
	
	public Card Peek(){
		return this.stack.Peek() as Card;
	}
	
	public void Push(Card card) {
		Push(card, false);
	}
	
	public void Push(Card card, bool flat){
		Vector3 s = new Vector3(card.transform.position.x, card.transform.position.y, card.transform.position.z);
		Vector3 z = transform.position + new Vector3(0.0f, this.stack.Count*0.01f, 0.0f);
		Vector3 intermediate = s + 0.5f *(z-s);
		intermediate.y+=1.0f;
		
		if (flat) {
			iTween.MoveTo(card.gameObject, z, 1.0f);
		}
		else {
			iTween.MoveTo(card.gameObject, iTween.Hash("path", new Vector3[]{s, intermediate, z}, "time", 1.0f));
		}
		
		Vector3 rot = transform.rotation.eulerAngles;
		if (!faceUp) {
			rot -= new Vector3(180, 0, 0);
		}
		if (untidy) {
			rot += new Vector3(0, Random.Range(-21, 21), 0);
			rot.y -= 360;
		}
		iTween.RotateTo(card.gameObject, rot , 1.0f);
		
		stack.Push(card);
	}
	
	public void Shuffle() {
		List<Card> tempList = new List<Card>();
		
		while (!IsEmpty) {
			tempList.Add(Pop());
		}
		
		while (tempList.Count > 0) {
			Card card = tempList[Random.Range(0, tempList.Count - 1)];
			tempList.Remove(card);
			Push(card, true);
		}
	}
	
	public void MoveAllCardsToStack(CardStack stack, bool flat) {
		Stack tempStack = new Stack();
		while (!IsEmpty) {
			tempStack.Push(Pop());
		}
		while (tempStack.Count > 0) {
			stack.Push(tempStack.Pop() as Card, flat);
		}
	}
	
	public Card Pop() {
		return this.stack.Pop() as Card;
	}
	
	public int Count {
		get {
			return stack.Count;
		}
	}
	
	public bool IsEmpty {
		get {
			return stack.Count <= 0;
		}
	}
}

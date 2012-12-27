using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour {
	public delegate void FlyCardsFromStackToStack(CardStack start, CardStack destination, int noCards);
	public static event FlyCardsFromStackToStack onFlyCardsFromStackToStack;
	
	public void doFlyCardsFromStackToStack(CardStack start, CardStack destination, int noCards){
		if (onFlyCardsFromStackToStack != null) 
			onFlyCardsFromStackToStack(start, destination, noCards);
	}

}

using UnityEngine;
using System.Collections;

public class SoundEventListener : MonoBehaviour {

	void onEnable(){
		EventManager.onFlyCardsFromStackToStack += onFlyCardsFromStackToStack;
	}

	void onDisable(){
		EventManager.onFlyCardsFromStackToStack -= onFlyCardsFromStackToStack;
	}

	// Camera
	public void onFlyCardsFromStackToStack(CardStack from, CardStack to, int noCards){
		audio.Play();
	}
}

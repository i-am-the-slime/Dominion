using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {
	// All the stacks on the table
	public CardStack copperStack;//   = new CardStack(AbsolutePosition(0,0));
	public CardStack silverStack;//   = new CardStack(AbsolutePosition(1,0));
	public CardStack goldStack;//     = new CardStack(AbsolutePosition(2,0));
	public CardStack estateStack;//   = new CardStack(AbsolutePosition(3,0));
	public CardStack duchyStack;//    = new CardStack(AbsolutePosition(4,0));
	public CardStack provinceStack;// = new CardStack(AbsolutePosition(5,0));
	public CardStack curseStack;//    = new CardStack(AbsolutePosition(6,0));
	public CardStack drawStack;//     = new CardStack(AbsolutePosition(0,4));
	public CardStack playedStack;
	public CardStack rubbishStack;
	public CardStack bigStack;
	private CardStack[] chosen_cards_stack;
	// Hand cards
	public Hand hand;
	// Camera
	public Cam cam;
	private const float DELAY = 0.025f; // TMP: was 0.15f
	
	// Use this for initialization
	IEnumerator Start() {
		//Initialize the base stack first thing.
//		CardStack big_stack = new CardStack(AbsolutePosition(3,3));
		for (int i=0; i<30; i++) bigStack.Push(CreateCard("Curse", "base"));
		for (int i=0; i<12; i++) bigStack.Push(CreateCard("Province", "common"));
		for (int i=0; i<12; i++) bigStack.Push(CreateCard("Duchy", "common"));
		for (int i=0; i<24; i++) bigStack.Push(CreateCard("Estate", "common"));
		for (int i=0; i<30; i++) bigStack.Push(CreateCard("Gold", "common"));
		for (int i=0; i<40; i++) bigStack.Push(CreateCard("Silver", "common"));
		for (int i=0; i<60; i++) bigStack.Push(CreateCard("Copper", "common"));
		
		// Money, money, money
		for (int i=0; i<60; i++) {
			yield return new WaitForSeconds(DELAY);
			copperStack.Push(bigStack.Pop());
		}
		for (int i=0; i<40; i++) {
			yield return new WaitForSeconds(DELAY);
			silverStack.Push(bigStack.Pop());
		}
		for (int i=0; i<30; i++) {
			yield return new WaitForSeconds(DELAY);
			goldStack.Push(bigStack.Pop());
		}
		
		cam.ChangeViewTo(Cam.Views.Hand);
		
		for (int i=0; i<24; i++) {
			yield return new WaitForSeconds(DELAY);
			estateStack.Push(bigStack.Pop());
		}
		for (int i=0; i<12; i++) {
			yield return new WaitForSeconds(DELAY);
			duchyStack.Push(bigStack.Pop());
		}
		for (int i=0; i<12; i++) {
			yield return new WaitForSeconds(DELAY);
			provinceStack.Push(bigStack.Pop());
		}
		for (int i=0; i<30; i++) {
			yield return new WaitForSeconds(DELAY);
			curseStack.Push(bigStack.Pop());
		}
		
		// Build draw stack
		float tmpdelay = 0.25f;
		for (int i=0; i<7; i++) {
			yield return new WaitForSeconds(tmpdelay);
			drawStack.Push(copperStack.Pop());
		}
		for (int i=0; i<3; i++) {
			yield return new WaitForSeconds(tmpdelay);
			drawStack.Push(estateStack.Pop());
		}
		
		yield return StartCoroutine(drawStack.Shuffle());
		yield return StartCoroutine(hand.DrawNewCards(5));
	}
	
	public IEnumerator MakeDrawStack() {
		yield return new WaitForSeconds(2.0f);
	}
	
	Card CreateCard(string name, string expansion){
		GameObject cardObject = MonoBehaviour.Instantiate(Resources.Load("Prefabs/card")) as GameObject;
		Material[] materials = cardObject.renderer.materials;
		Texture2D tex = (Texture2D) (Resources.Load("textures_"+Settings.LANG+"/"+expansion+"/"+name.ToLower()));
		materials[1].SetTexture("_MainTex", tex);
		cardObject.renderer.materials = materials;

        cardObject.AddComponent(name);
        return cardObject.GetComponent<Card>();
	}
	
	IEnumerator PlayIn(float secs){
		yield return new WaitForSeconds(secs);
		audio.Play();
	}

	IEnumerator PutCardOnStack(Card card, CardStack stack, float delay){
		yield return new WaitForSeconds(delay);
		stack.Push (card);
	}
	
	IEnumerator SetUpDrawStack(float delay){
		yield return new WaitForSeconds(delay);
		float tmpdelay = 0.5f;
		for (int i=0; i<7; i++)
			StartCoroutine(PutCardOnStack(copperStack.Pop(), drawStack, i*tmpdelay));
		for (int i=0; i<3; i++)
			StartCoroutine(PutCardOnStack(estateStack.Pop(), drawStack, i*tmpdelay + 7*tmpdelay));
		for (int i=0; i<5; i++)
			StartCoroutine(DrawCardFromStack(drawStack, i*tmpdelay + 10*tmpdelay));
	}
	
	IEnumerator DrawCardFromStack(CardStack stack, float delay){
		yield return new WaitForSeconds(delay);
		hand.DrawCard(stack.Pop());
	}
	
	public void Update(){
		if (Input.GetButtonDown("Jump")) {
			StartCoroutine(EndTurn());
		}
	}
	
	IEnumerator EndTurn() {
		yield return StartCoroutine(hand.EndTurn());
		playedStack.MoveAllCardsToStack(rubbishStack, true);
		yield return new WaitForSeconds(1.0f);
        hand.BeginNewTurn();
		yield return StartCoroutine(hand.DrawNewCards(5));
	}
}

public class Settings{
	public static string LANG = "en";
}

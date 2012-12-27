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
	public CardStack bigStack;
	private CardStack[] chosen_cards_stack;
	// Hand cards
	public Hand hand;
	// Event Manager
	public EventManager eventManager;
	// Camera
	public Cam cam;
	private const float DELAY = 0.025f; // TMP: was 0.15f
	
	// Use this for initialization
	void Start () {
		InitializeBaseStacks();
		//InitializeGameStacks();
		cam.ChangeViewTo(Cam.Views.Hand);
	}

	static Vector3 AbsolutePosition(int x, int y){
		float X = -40.0f;
		float Y = 68.4f;
		float Z = -487.0f;
		float width = 1.0f;
		float height = 1.2f;
		return new Vector3(x*width+X, Y, Z-(y*height));
	}
	
	void InitializeBaseStacks(){
		//Initialize the base stack first thing.
//		CardStack big_stack = new CardStack(AbsolutePosition(3,3));
		for (int i=0; i<30; i++) bigStack.Push(new Card("curse", "base"));
		for (int i=0; i<12; i++) bigStack.Push(new Card("province", "common"));
		for (int i=0; i<12; i++) bigStack.Push(new Card("duchy", "common"));
		for (int i=0; i<24; i++) bigStack.Push(new Card("estate", "common"));
		for (int i=0; i<30; i++) bigStack.Push(new Card("gold", "common"));
		for (int i=0; i<40; i++) bigStack.Push(new Card("silver", "common"));
		for (int i=0; i<60; i++) bigStack.Push(new Card("copper", "common"));
			
		eventManager.doFlyCardsFromStackToStack(bigStack, copperStack, 60);
		for (int i=0; i<60; i++)
			StartCoroutine(PutCardOnStack(bigStack.Pop(), copperStack, i*DELAY));
		for (int i=0; i<40; i++)
			StartCoroutine(PutCardOnStack(bigStack.Pop(), silverStack, i*DELAY + 60*DELAY));
		for (int i=0; i<30; i++)
			StartCoroutine(PutCardOnStack(bigStack.Pop(), goldStack, i*DELAY + 100*DELAY));
		for (int i=0; i<24; i++)
			StartCoroutine(PutCardOnStack(bigStack.Pop(), estateStack, i*DELAY + 130*DELAY));
		for (int i=0; i<12; i++)
			StartCoroutine(PutCardOnStack(bigStack.Pop(), duchyStack, i*DELAY + 154*DELAY));
		for (int i=0; i<12; i++)
			StartCoroutine(PutCardOnStack(bigStack.Pop(), provinceStack, i*DELAY + 166*DELAY));
		for (int i=0; i<30; i++)
			StartCoroutine(PutCardOnStack(bigStack.Pop(), curseStack, i*DELAY + 178*DELAY));
		//Wait until this is done
		//Make the draw stack
		StartCoroutine(SetUpDrawStack(155*DELAY));
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
	
	//Update is called once per frame
	
	RaycastHit hit = new RaycastHit();
	bool show_card_info = false;
	
	void Update () {
		if (show_card_info && Input.GetMouseButtonUp(1)){
			show_card_info = false;
		}
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if(Input.GetMouseButtonDown(1) && Physics.Raycast(ray,out hit, 100.0f) ){
			print(hit.collider.name);
			Material mat = hit.collider.transform.gameObject.renderer.materials[1]; 
			currentCard = (Texture2D) mat.mainTexture;
			show_card_info = true;
		}
	}
	
	Texture2D currentCard;
	void OnGUI() {
		if (show_card_info == true){
			GUI.Box(new Rect(Screen.width/2-326/2,Screen.height/2-503/2,326,503), currentCard);
		}
	}
}

public class Settings{
	public static string LANG = "en";
}

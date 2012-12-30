using System;
using UnityEngine;
using System.Collections;

public delegate bool RevealCardsCallback(Card card, Player player);
public delegate bool IsSelectedCardSelectable(Card card, Player player);
public delegate IEnumerator SelectedCardsCallback(ArrayList cardList, Player player);

public class Player : MonoBehaviour
{
    private bool InputAllowed = true;
    public CardStack playedStack;
    public CardStack rubbishStack;
    public CardStack drawStack;
    public CardStack silverStack;
    public CardStack DrawStack { get { return drawStack; } set { drawStack = value; } }
    public CardStack RubbishStack { get { return rubbishStack; } set { rubbishStack = value; } }
    public CardStack SilverStack { get { return silverStack; } set { silverStack = value; } }
    public Cam cam;
    private IList cards = new ArrayList();
    private const float cardWidth = 0.8f;

    private int money = 0;
    private int buys = 1;
    private int actions = 1;
    private bool buyPhase = false;

    public bool CardPlaying = false;

    public void IncreaseMoney(int by) { money += by; } 
    public void IncreaseActions(int by) { actions += by; }

    public void IncreaseBuys(int by) { buys += by; }

    // Selection mode
    private ArrayList selectedCards = new ArrayList();
    private bool selectionMode = false;
    public SelectedCardsCallback chooseCardsCallback;
    public IsSelectedCardSelectable selectedCardValidCallback;

    public IEnumerator DrawCard(Card card) {
        if (InputAllowed) {
            InputAllowed = false;
            cards.Add(card);
            card.CardClicked += OnCardClicked;
            //Trying to avoid cards flying back to the hand.
            yield return new WaitForSeconds(0.2f);
            ReorderCards();
            yield return new WaitForSeconds(0.3f);
            InputAllowed = true;
        }
    }

    private void ReorderCards() {
        int index = 0;
        foreach (Card c in cards) {
            iTween.MoveTo(c.gameObject, transform.position + CalcCardPosition(index), 2.0f);
            iTween.RotateTo(c.gameObject, transform.rotation.eulerAngles, 2.0f);
            index++;
        }
    }

    private Vector3 CalcCardPosition(int index) {
        float centerIndex = (cards.Count - 1) / 2.0f;
        float xPos = (index - centerIndex) * cardWidth;
        return new Vector3(xPos, 0.0f, 0.0f);
    }

    public void OnCardClicked(Card card) {
        if (!InputAllowed)
            return;
        if (selectionMode) {
            if (selectedCardValidCallback(card, this)) {
                if (selectedCards.Contains(card)) {
                    selectedCards.Remove(card);
                    cards.Add(card);
                    // Grey card out
                    card.GreyOut();
                    // Move card back down so that we know that we keep it on our hand.
                    iTween.MoveTo(card.gameObject, card.gameObject.transform.position - new Vector3(0, 0.1f, 0), 0.3f);
                }
                else {
                    cards.Remove(card);
                    selectedCards.Add(card);
                    // Make card have full brightness
                    card.UnGreyOut();
                    // Move card up a little so we know that this is selected
                    iTween.MoveTo(card.gameObject, card.gameObject.transform.position + new Vector3(0, 0.1f, 0), 0.3f);
                }
            }   
        }
        else {
            if ((card.Flags & Card.CardFlags.Action) == Card.CardFlags.Action && !buyPhase && actions > 0) {
                card.CardClicked -= OnCardClicked;
                cards.Remove(card);
                ReorderCards();
                playedStack.Push(card);
                actions--;
                try { StartCoroutine(card.Play(this)); }
                catch (NullReferenceException) { /*Ignore*/ }
            }
            else if ((card.Flags & Card.CardFlags.Treasure) == Card.CardFlags.Treasure) {
                card.CardClicked -= OnCardClicked;
                cards.Remove(card);
                ReorderCards();
                playedStack.Push(card);
                try { StartCoroutine(card.Play(this)); }
                catch (NullReferenceException) { /*Ignore*/ }
            }
        }
    }
    

    public IEnumerator EndTurn()
    {
        if (cards.Count == 0)
        {
            yield break;
        }

        while (cards.Count > 0)
        {
            Card c = cards[0] as Card;
            cards.Remove(c);
            c.CardClicked -= OnCardClicked;
            playedStack.Push(c);
        }

        yield return new WaitForSeconds(0.8f);
    }

    public void DiscardSelectedCards(ArrayList cardList)
    {
        foreach (Card card in cardList)
        {
            Debug.Log("Pushing " + card.name + " to discard pile.");
            rubbishStack.Push(card);
        }
        ReorderCards();
    }

    public void BeginNewTurn()
    {
        money = 0;
        actions = 1;
        buys = 1;
        buyPhase = false;
    }

    public void DrawCards(int number)
    {
        StartCoroutine(DrawNewCards(number));
    }

    public IEnumerator DrawNewCards(int number)
    {
        // If there are not enough cards, recycle the rubbish stack
        if (DrawStack.Count < number)
        {
            int drawable = DrawStack.Count;
            for (int i = 0; i < drawable; i++)
            {
                yield return StartCoroutine(DrawCard(DrawStack.Pop()));
            }

            rubbishStack.MoveAllCardsToStack(DrawStack, false);
            yield return new WaitForSeconds(1.0f);
            yield return StartCoroutine(DrawStack.Shuffle());

            for (int i = 0; i < number - drawable; i++)
            {
                yield return StartCoroutine(DrawCard(DrawStack.Pop()));
            }
        }
        else
        {
            for (int i = 0; i < number; i++)
            {
                yield return StartCoroutine(DrawCard(DrawStack.Pop()));
            }
        }

        yield return new WaitForSeconds(0.5f);
    }

	IEnumerator EndCurrentTurn() {
		yield return StartCoroutine(EndTurn());
		playedStack.MoveAllCardsToStack(rubbishStack, true);
		yield return new WaitForSeconds(1.0f);
        BeginNewTurn();
		yield return StartCoroutine(DrawNewCards(5));
	}

    public IEnumerator RevealCards(RevealCardsCallback callback)
    {
        yield return HideHand();
        while (true)
        {
            if (DrawStack.Count == 0)
            {
                if (rubbishStack.Count == 0)
                {
                    break;
                }
                rubbishStack.MoveAllCardsToStack(DrawStack, false);
                yield return DrawStack.Shuffle();
                yield return new WaitForSeconds(DrawStack.audio.clip.length);
            }
            Card card = DrawStack.Pop();
            iTween.MoveTo(card.gameObject, new Vector3(-37, 72.57f, -495.44f), 0.5f);
            iTween.RotateTo(card.gameObject, new Vector3(320.0f, 180.0f, 180.0f), 0.5f);
            yield return new WaitForSeconds(0.5f);
            if (callback(card, this))
            {
                yield return new WaitForSeconds(1);
                break;
            }
            yield return new WaitForSeconds(1);
        }
        yield return ShowHand();
    }


    public void ChooseCards(IsSelectedCardSelectable cardCallback, SelectedCardsCallback finalCallback)
    {
        Debug.Log("Entered selection mode.");
        selectionMode = true;
        selectedCards = new ArrayList();
        chooseCardsCallback = finalCallback;
        selectedCardValidCallback = cardCallback;
        foreach (Card card in cards)
        {
            card.GreyOut();            
        }
    }

    public IEnumerator ShowHand()
    {
        foreach (Card card in cards)
        {
            iTween.MoveTo(card.gameObject, card.gameObject.transform.position + new Vector3(0, 1.5f, 0), 0.5f);
        }
        yield return new WaitForSeconds(0.5f);
    }

    public IEnumerator HideHand()
    {
        foreach (Card card in cards)
        {
            iTween.MoveTo(card.gameObject, card.gameObject.transform.position - new Vector3(0, 1.5f, 0), 0.5f);
        }
        yield return new WaitForSeconds(0.5f);
    }

    public void BuyCard(CardStack cardStack)
    {
        Card card = cardStack.Peek();
        Debug.Log("Attempting to buy card " + card.name);
        if (card.Cost <= money && buys > 0)
        {
            Debug.Log("Actually buying it.");
            buyPhase = true;
            rubbishStack.Push(cardStack.Pop());
            buys--;
            money -= card.Cost;
        }
    }

    public int Money { get { return money; } }
    public int Buys { get { return buys; } }
    public int Actions { get { return actions; } }
    Card hoveredCard = null;

    public void Update() {
        if (!selectionMode && Input.GetButtonDown("Jump")) {
            StartCoroutine(EndCurrentTurn());
        }
        if (Input.GetMouseButtonDown(1)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit, 50.0f)) {
                Card card = hit.collider.gameObject.GetComponent<Card>();
                if(card!=null){
                    hoveredCard = card;
                    card.ShowCardInfo = true;
                }
            }
        }
        if (Input.GetMouseButtonUp(1)) {
            hoveredCard.ShowCardInfo = false;
            hoveredCard = null; 
        }

    }
    
    bool seeOnlyActionCards = false;
    public void OnGUI() {
        GUI.skin = Resources.Load("GUI/DominionGUISkin") as GUISkin;
        GUI.backgroundColor = new Color(0, 0, 0, 1);
        if (selectionMode)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 100, 20, 200, 20), "Done"))
            {
                Debug.Log("Done button clicked in selection mode.");                
                selectionMode = false;
                foreach (Card card in cards)
                {
                    card.UnGreyOut();
                }
                StartCoroutine(chooseCardsCallback(selectedCards, this));

            }
        }
        String stats = "Actions: " + actions + "    Money: " + money + "   Buys: " + buys;
        GUI.Box(new Rect(5, Screen.height - 40, Screen.width -115, 35), stats);
        if (!buyPhase) {
            if (GUI.Button(new Rect(Screen.width - 105, Screen.height - 40, 100, 35), "Buy cards")) {
                buyPhase = true;
                foreach (Card card in cards) {
                    if (card.Cost > money) {
                        card.GreyOut();
                    }
                }
                cam.ChangeViewTo(Cam.Views.Table);
            }
        }
        else {
            if (GUI.Button(new Rect(Screen.width - 105, Screen.height - 40, 100, 35), "End turn")) {
                buyPhase = false;
                foreach (Card card in cards) {
                    card.UnGreyOut();
                }
                cam.ChangeViewTo(Cam.Views.Hand);
                StartCoroutine(EndCurrentTurn());
            }
            if (!seeOnlyActionCards) {
                if (GUI.Button(new Rect(Screen.width / 2 - 100, 20, 200, 20), "See only action cards.")) {
                    seeOnlyActionCards = true;
                    cam.ChangeViewTo(Cam.Views.ActionCards);
                }
            }
            else {
                if (GUI.Button(new Rect(Screen.width / 2 - 100, 20, 200, 20), "See all cards.")) {
                    seeOnlyActionCards = false;
                    cam.ChangeViewTo(Cam.Views.Table);
                }
            }
        }
    }
}
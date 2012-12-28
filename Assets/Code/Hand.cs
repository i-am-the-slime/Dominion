using UnityEngine;
using System.Collections;

public class Hand : MonoBehaviour, IPlayer{
	public CardStack playedStack;
    public CardStack rubbishStack;
    public CardStack drawStack;
    public CardStack silverStack;

    public CardStack DrawStack
    {
        get { return drawStack; }
        set { drawStack = value; }
    }

    public CardStack SilverStack
    {
        get { return silverStack; }
        set { silverStack = value; }
    }
	
	private IList cards = new ArrayList();
	private const float cardWidth = 0.7f;

    private int money = 0;
    private int buys = 1;
    private int actions = 1;
    private bool buyPhase = false;

    public bool CardPlaying = false;

    public void IncreaseMoney(int by) 
    {
        money += by;
    }

    public void IncreaseActions(int by)
    {
        actions += by;
    }

    public void IncreaseBuys(int by)
    {
        buys += by;
    }

    public IEnumerator DrawCard(Card card){
		cards.Add(card);
		card.CardClicked += OnCardClicked;
		ReorderCards();
		yield return new WaitForSeconds(0.3f);
	}
	
	private void ReorderCards(){
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
        if ((card.Flags & Card.CardFlags.Action) == Card.CardFlags.Action && !buyPhase && actions>0) {
            card.CardClicked -= OnCardClicked;
		    cards.Remove(card);
		    ReorderCards();
            playedStack.Push(card);
            actions--;
            StartCoroutine(card.Play(this));
        }
        else if ((card.Flags & Card.CardFlags.Treasure) == Card.CardFlags.Treasure)
        {
            card.CardClicked -= OnCardClicked;
            cards.Remove(card);
            ReorderCards();
            playedStack.Push(card);
            card.Play(this);
        }
	}
	
	public IEnumerator EndTurn() {
		if (cards.Count == 0) {
			yield break;
		}
		
		while (cards.Count > 0) {
			Card c = cards[0] as Card;
			cards.Remove(c);
			c.CardClicked -= OnCardClicked;
			playedStack.Push(c);
		}
		
		yield return new WaitForSeconds(0.8f);
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
	
	public IEnumerator DrawNewCards(int number) {
		// If there are not enough cards, recycle the rubbish stack
		if (DrawStack.Count < number) {
			int drawable = DrawStack.Count;
			for (int i = 0; i < drawable; i++) {
				yield return StartCoroutine(DrawCard(DrawStack.Pop()));
			}
			
			rubbishStack.MoveAllCardsToStack(DrawStack, false);
			yield return new WaitForSeconds(1.0f);
			yield return StartCoroutine(DrawStack.Shuffle());
			
			for (int i = 0; i < number-drawable; i++) {
				yield return StartCoroutine(DrawCard(DrawStack.Pop()));
			}
		}
		else {
			for(int i=0; i<number; i++){
				yield return StartCoroutine(DrawCard(DrawStack.Pop()));
			}
		}
		
		yield return new WaitForSeconds(0.5f);
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

    public int Money
    {
        get { return money; }
    }

    public void BuyCard(CardStack cardStack)
    {
        Card card = cardStack.Peek();
        if (card.Cost <= money && buys > 0)
        {
            buyPhase = true;
            rubbishStack.Push(cardStack.Pop());
            buys--;
            money -= card.Cost;
        }
    }
    
    public int Buys
    {
        get { return buys; }
    }

    public int Actions
    {
        get { return actions; }
    }

}

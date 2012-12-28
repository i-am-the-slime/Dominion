using UnityEngine;
using System.Collections;

public class Hand : MonoBehaviour, IPlayer{
	public CardStack playedStack;
	public CardStack drawStack;
	public CardStack rubbishStack;
	
	private IList cards = new ArrayList();
	private const float cardWidth = 1.0f;

    private int money = 0;
    private int buys = 1;
    private int actions = 1;

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
        if (card.IsPlayable()) {
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
    }

    public void DrawCards(int number)
    {
        StartCoroutine(DrawNewCards(number));
    }
	
	public IEnumerator DrawNewCards(int number) {
		// If there are not enough cards, recycle the rubbish stack
		if (drawStack.Count < number) {
			int drawable = drawStack.Count;
			for (int i = 0; i < drawable; i++) {
				yield return StartCoroutine(DrawCard(drawStack.Pop()));
			}
			
			rubbishStack.MoveAllCardsToStack(drawStack, false);
			yield return new WaitForSeconds(1.0f);
			yield return StartCoroutine(drawStack.Shuffle());
			
			for (int i = 0; i < number-drawable; i++) {
				yield return StartCoroutine(DrawCard(drawStack.Pop()));
			}
		}
		else {
			for(int i=0; i<number; i++){
				yield return StartCoroutine(DrawCard(drawStack.Pop()));
			}
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
        if (card.GetCost() <= money && buys > 0)
        {
            rubbishStack.Push(cardStack.Pop());
            buys--;
            money -= card.GetCost();
        }
    }
}

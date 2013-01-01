using System;
using UnityEngine;
using System.Collections;

public delegate bool RevealCardsCallback(Card card, Player player);

public class Player : MonoBehaviour
{
    public CardStack playedStack;
    public CardStack rubbishStack;
    public CardStack drawStack;
    public CardStack silverStack;
    public CardStack trashStack;
    public CardStack TrashStack { get { return trashStack; } set { drawStack = value; } }
    public CardStack DrawStack { get { return drawStack; } set { drawStack = value; } }
    public CardStack RubbishStack { get { return rubbishStack; } set { rubbishStack = value; } }
    public CardStack SilverStack { get { return silverStack; } set { silverStack = value; } }
    public Hand hand;
    public GUIManager guiManager;
    public Player leftNeighbour;
    private int money = 0;
    private int buys = 1;
    private int actions = 1;

    public int Money { get { return money; } set { money = value; } }
    public int Buys { get { return buys; } set { buys = value; } }
    public int Actions { get { return actions; } set { actions = value; } }

    public IEnumerator EndTurn()
    {
        if (hand.Cards.Count == 0)
        {
            yield break;
        }

        while (hand.Cards.Count > 0)
        {
            Card c = hand.Cards[0] as Card;
            hand.Cards.Remove(c);
            playedStack.Push(c);
        }

        yield return new WaitForSeconds(0.8f);
    }

    public void ResetStats()
    {
        money = 0;
        actions = 1;
        buys = 1;
    }

	public IEnumerator EndCurrentTurn() {
		yield return StartCoroutine(EndTurn());
		playedStack.MoveAllCardsToStack(rubbishStack, true);
		yield return new WaitForSeconds(1.0f);
		yield return StartCoroutine(hand.DrawNewCards(5));
	    ResetStats();
	}

    public IEnumerator RevealCards(RevealCardsCallback callback)
    {
        yield return hand.HideHand();
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
        yield return hand.ShowHand();
    }


    public void BuyCard(CardStack cardStack)
    {
        Card card = cardStack.Peek();
        Debug.Log("Attempting to buy card " + card.name);
        if (card.Cost <= money && buys > 0)
        {
			guiManager.ChangeMode(new BuyMode(this));
            Debug.Log("Actually buying it.");
            rubbishStack.Push(cardStack.Pop());
            buys--;
            money -= card.Cost;
        }
    }

    public void DrawCards(int i)
    {
        hand.DrawCards(i);
    }

    public IEnumerator DrawCard(Card card)
    {
        yield return hand.DrawCard(card);
    }

    public void ChooseCards(IsSelectedCardSelectable cardCallback, SelectedCardsCallback finalCallback)
    {
        guiManager.ChangeMode(new CardSelectionMode(this, cardCallback, finalCallback));
    }
}
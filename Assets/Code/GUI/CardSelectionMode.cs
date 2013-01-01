using UnityEngine;
using System.Collections;
public delegate bool IsSelectedCardSelectable(Card card, Player player, ArrayList cardList);
public delegate IEnumerator SelectedCardsCallback(ArrayList cardList, Player player);

public class CardSelectionMode : GUIMode
{
    public override string ModeName {
        get { return "CardSelection"; }
    }
    private ArrayList selectedCards = new ArrayList();
    public SelectedCardsCallback chooseCardsCallback;
    public IsSelectedCardSelectable selectedCardValidCallback;
    private Player _p; 
	private ArrayList cardsWithEventListener = new ArrayList();

    public CardSelectionMode(Player player, IsSelectedCardSelectable cardCallback, SelectedCardsCallback finalCallback)
    {
        _p = player;
        selectedCards = new ArrayList();
        chooseCardsCallback = finalCallback;
        selectedCardValidCallback = cardCallback;
        _p.StartCoroutine(Init());
    }

    public IEnumerator Init()
    {
        // Wait until card is played.
        yield return new WaitForSeconds(0.5f);
        foreach (Card card in _p.hand.Cards)
        {
            card.GreyOut();
            card.CardClicked += OnCardClicked;
			cardsWithEventListener.Add(card);
        }
    }

    public override void FinishMode()
    {
        foreach (Card card in _p.hand.Cards)
        {
            card.UnGreyOut();
        }
		foreach (Card card in selectedCards){
			card.CardClicked -= OnCardClicked;
		}
        _p.StartCoroutine(chooseCardsCallback(selectedCards, _p));
    }

    public void OnCardClicked(Card card) {
        Debug.Log("Selection mode click listener");
        if (selectedCards.Contains(card))
        {
            selectedCards.Remove(card);
            _p.hand.Cards.Add(card);
            // Grey card out
            card.GreyOut();
            // Move card back down so that we know that we keep it on our hand.
            iTween.MoveTo(card.gameObject, card.gameObject.transform.position - new Vector3(0, 0.1f, 0), 0.3f);
        }
        else if (selectedCardValidCallback(card, _p, selectedCards)) {
            _p.hand.Cards.Remove(card);
            selectedCards.Add(card);
            // Make card have full brightness
            card.UnGreyOut();
            // Move card up a little so we know that this is selected
            iTween.MoveTo(card.gameObject, card.gameObject.transform.position + new Vector3(0, 0.1f, 0), 0.3f);
        }
    }
}
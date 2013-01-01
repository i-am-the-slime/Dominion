using System;
using UnityEngine;
using System.Collections;

public class ActionMode : GUIMode {
    public override string ModeName {
        get { return "Action"; }
    }

    private Player _p;
    ArrayList cardsWithClickHandler = new ArrayList();

    public ActionMode(Player player) {
        _p = player;
        Debug.Log("Starting Action mode");
        Debug.Log("Player has "+ _p.hand.Cards.Count + ".");
        foreach (Card card in _p.hand.Cards)
        {
            cardsWithClickHandler.Add(card);
            card.CardClicked += OnCardClicked;
        }
    }

    public override void FinishMode() {
      Debug.Log("Finishing Action mode");
        foreach (Card card in cardsWithClickHandler)
        {
            card.CardClicked -= OnCardClicked;
        }
    }

    public void OnCardClicked(Card card) {
        Debug.Log("Action Mode Click Listener");
        if ((card.Flags & Card.CardFlags.Action) == Card.CardFlags.Action && _p.Actions > 0) {
            card.CardClicked -= OnCardClicked;
            _p.playedStack.Push(card);
            _p.hand.Cards.Remove(card);
            _p.hand.ReorderCards();
            _p.Actions--;
            try { _p.StartCoroutine(card.Play(_p)); }
            catch (NullReferenceException) { /*Ignore*/ }
        }
        else if ((card.Flags & Card.CardFlags.Treasure) == Card.CardFlags.Treasure) {
            card.CardClicked -= OnCardClicked;
            _p.hand.Cards.Remove(card);
            _p.hand.ReorderCards();
            _p.playedStack.Push(card);
            try { _p.StartCoroutine(card.Play(_p)); }
            catch (NullReferenceException) { /*Ignore*/ }
        }
    }
}
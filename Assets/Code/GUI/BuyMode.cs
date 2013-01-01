using System;
using UnityEngine;
using System.Collections;

public class BuyMode : GUIMode {
    public override string ModeName {
        get { return "Buy"; }
    }
    private Player _p;

    public BuyMode(Player player) {
        Debug.Log("Starting Buy mode");
        Debug.Log(ModeName);
        _p = player;
        foreach (Card card in _p.hand.Cards) {
            card.CardClicked += OnCardClicked;
        }
    }

    public override void FinishMode()
    {
        Debug.Log("Finishing Buy mode");
        foreach (Card card in _p.hand.Cards) {
            card.CardClicked -= OnCardClicked;
        }
        _p.StartCoroutine(_p.EndCurrentTurn());
    }

    public void OnCardClicked(Card card) {
        Debug.Log("OnCardClicked");
        if ((card.Flags & Card.CardFlags.Action) == Card.CardFlags.Action && _p.Actions > 0) {
            card.CardClicked -= OnCardClicked;
            _p.hand.ReorderCards();
            _p.playedStack.Push(card);
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
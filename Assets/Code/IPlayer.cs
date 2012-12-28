using UnityEngine;
using System.Collections;

public delegate bool RevealCardsCallback(Card card, Hand hand);

public interface IPlayer {
    void IncreaseMoney(int by);
    void IncreaseActions(int by);
    void IncreaseBuys(int by);
    int Money
    {
        get;
    }
    int Buys
    {
        get;
    }
    int Actions
    {
        get;
    }
    void DrawCards(int number);

    IEnumerator RevealCards(RevealCardsCallback callback);
}

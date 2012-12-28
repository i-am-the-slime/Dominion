using UnityEngine;
using System.Collections;

public class Adventurer : Card
{
    public override int Cost { get { return 6; } }
    public override int Score { get { return 0; } }
    public override CardFlags Flags { get { return CardFlags.Action; } }
    private int treasureCardsFound = 0;

    public override void Play(IPlayer player)
    {
        StartCoroutine(player.RevealCards(Callback));
    }

    public bool Callback(Card card, Hand hand)
    {
        if ((card.Flags & Card.CardFlags.Treasure) == Card.CardFlags.Treasure)
        {
            StartCoroutine(hand.DrawCard(card));
            treasureCardsFound++;
        }
        else
        {
            hand.rubbishStack.Push(card);
        }
        if (treasureCardsFound >= 2)
        {
            treasureCardsFound = 0;
            return true;
        }
        return false;
    }
}

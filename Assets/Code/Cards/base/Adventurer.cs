using System.Collections;

public class Adventurer : Card
{
    public override int Cost { get { return 6; } }
    public override int Score { get { return 0; } }
    public override CardFlags Flags { get { return CardFlags.Action; } }
    private int _treasureCardsFound;

    public override IEnumerator Play(Player player)
    {
        _treasureCardsFound = 0;
        StartCoroutine(player.RevealCards(Callback));
        return null;
    }

    public bool Callback(Card card, Player player)
    {
        if ((card.Flags & CardFlags.Treasure) == CardFlags.Treasure)
        {
            StartCoroutine(player.DrawCard(card));
            _treasureCardsFound++;
        }
        else
        {
            player.rubbishStack.Push(card);
        }
        if (_treasureCardsFound >= 2)
        {
            _treasureCardsFound = 0;
            return true;
        }
        return false;
    }
}

using UnityEngine;
using System.Collections;

public class Cellar : Card
{
    public override int Cost { get { return 2; } }
    public override int Score { get { return 0; } }
    public override CardFlags Flags { get { return CardFlags.Action; } }

    public override IEnumerator Play(Player player)
    {
        player.IncreaseActions(1);
        Debug.Log("Calling Choose Cards");
        player.ChooseCards(CardCallback, FinalCallback);
        return null;
    }

    public bool CardCallback(Card card, Player player)
    {
        return true;
    }

    public IEnumerator FinalCallback(ArrayList cardList, Player player)
    {
        foreach (Card card in cardList)
        {
            Debug.Log("Must throw away " + card.name);
        }
        
        player.DiscardSelectedCards(cardList);
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(player.DrawNewCards(cardList.Count));
    }
}

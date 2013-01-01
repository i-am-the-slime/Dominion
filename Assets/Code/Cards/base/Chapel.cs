using UnityEngine;
using System.Collections;

public class Chapel : Card {
    public override int Cost { get { return 2; } }
    public override int Score { get { return 0; } }
    public override CardFlags Flags { get { return CardFlags.Action; } }

    public override IEnumerator Play(Player player) {
        player.Actions++;
        Debug.Log("Calling Choose Cards");
        player.ChooseCards(CardCallback, FinalCallback);
        return null;
    }

    public bool CardCallback(Card card, Player player, ArrayList selectedCardList)
    {
        return selectedCardList.Count < 2;
    }

    public IEnumerator FinalCallback(ArrayList cardList, Player player) {
        foreach (Card card in cardList) {
            Debug.Log("Must throw away " + card.name);
        }

        player.hand.TrashSelectedCards(cardList);
        yield return new WaitForSeconds(0.5f);
    }
}

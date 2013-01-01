using UnityEngine;
using System.Collections;

public class CouncilRoom : Card
{
    public override int Cost { get { return 5; } }
    public override int Score { get { return 0; } }
    public override CardFlags Flags { get { return CardFlags.Action; } }

    public override IEnumerator Play(Player player)
    {
        player.DrawCards(4);
        player.Buys++;
        //TODO: Have each other player draw a card.
        return null;
    }
}
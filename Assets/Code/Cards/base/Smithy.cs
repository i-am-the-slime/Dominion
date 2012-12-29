using UnityEngine;
using System.Collections;

public class Smithy : Card
{
    public override int Cost { get { return 4; } }
    public override int Score { get { return 0; } }
    public override CardFlags Flags { get { return CardFlags.Action; } }

    public override IEnumerator Play(Player player)
    {
        player.DrawCards(3);
        return null;
    }
}
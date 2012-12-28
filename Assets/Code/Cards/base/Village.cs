using UnityEngine;
using System.Collections;

public class Village : Card
{
    public override int Cost { get { return 3; } }
    public override int Score { get { return 0; } }
    public override CardFlags Flags { get { return CardFlags.Action; } }

    public override IEnumerator Play(Player player)
    {
        player.IncreaseActions(2);
        player.DrawCards(1);
        return null;
    } 
}

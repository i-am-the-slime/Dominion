using UnityEngine;
using System.Collections;

public class Festival : Card
{
    public override int Cost { get { return 5; } }
    public override int Score { get { return 0; } }
    public override CardFlags Flags { get { return CardFlags.Action; } }

    public override IEnumerator Play(Player player)
    {
        player.IncreaseActions(2);
        player.IncreaseBuys(1);
        player.IncreaseMoney(2);
        return null;
    }
}
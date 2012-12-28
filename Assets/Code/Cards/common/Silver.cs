using UnityEngine;
using System.Collections;

public class Silver : Card
{
    public override int Cost { get { return 3; } }
    public override int Score { get { return 0; } }
    public override CardFlags Flags { get { return CardFlags.Treasure; } }

    public override IEnumerator Play(IPlayer player)
    {
        player.IncreaseMoney(2);
        return null;
    }
}

using UnityEngine;
using System.Collections;

public class Gold : Card
{
    public override int Cost { get { return 5; } }
    public override int Score { get { return 0; } }
    public override CardFlags Flags { get { return CardFlags.Treasure; } }

    public override IEnumerator Play(Player player)
    {
        player.IncreaseMoney(3);
        return null;
    }
}

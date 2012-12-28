using UnityEngine;
using System.Collections;

public class Copper : Card
{
    public override int Cost { get { return 0; } }
    public override int Score { get { return 0; } }
    public override CardFlags Flags { get { return CardFlags.Treasure; } }

    public override void Play(IPlayer player)
    {
        player.IncreaseMoney(1);
    }
}

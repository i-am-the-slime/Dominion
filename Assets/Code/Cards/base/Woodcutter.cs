using UnityEngine;
using System.Collections;

public class Woodcutter : Card
{
    public override int Cost { get { return 3; } }
    public override int Score { get { return 0; } }
    public override CardFlags Flags { get { return CardFlags.Action; } }

    public override IEnumerator Play(Player player)
    {
        player.Buys++;
        player.Money+=2;
        
        return null;
    }
}
using UnityEngine;
using System.Collections;

public class Festival : Card
{
    public override int Cost { get { return 5; } }
    public override int Score { get { return 0; } }
    public override CardFlags Flags { get { return CardFlags.Action; } }

    public override IEnumerator Play(Player player)
    {
        player.Actions+=2;
        player.Buys+=1;
        player.Money+=2;
        return null;
    }
}
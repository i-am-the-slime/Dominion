using UnityEngine;
using System.Collections;

public class Village : Card
{
    public override int Cost { get { return 3; } }
    public override int Score { get { return 0; } }
    public override CardFlags Flags { get { return CardFlags.Action; } }

    public override void Play(IPlayer player)
    {
        player.IncreaseActions(2);
        player.DrawCards(1);
    } 
}

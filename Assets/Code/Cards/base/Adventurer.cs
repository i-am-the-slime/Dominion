using UnityEngine;
using System.Collections;

public class Adventurer : Card
{
    public override int Cost { get { return 6; } }
    public override int Score { get { return 0; } }
    public override CardFlags Flags { get { return CardFlags.Action; } }

    public override void Play(IPlayer player)
    {
        StartCoroutine(player.HideHand());
    } 
}

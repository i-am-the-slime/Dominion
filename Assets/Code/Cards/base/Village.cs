using UnityEngine;
using System.Collections;

public class Village : Card
{
    public override bool IsPlayable()
    {
        return true;
    }

    public override int GetCost()
    {
        return 3;
    }

    public override void Play(IPlayer player)
    {
        player.IncreaseActions(2);
        player.DrawCards(1);
    }
}

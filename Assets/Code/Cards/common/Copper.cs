using UnityEngine;
using System.Collections;

public class Copper : Card {
    public override void Play(IPlayer player)
    {
        player.IncreaseMoney(1);
    }

    public override bool IsPlayable()
    {
        return true;
    }

    public override int GetCost()
    {
        return 0;
    }
}

using UnityEngine;
using System.Collections;

public class Silver : Card {
    public override void Play(IPlayer player)
    {
        player.IncreaseMoney(2);
    }

    public override bool IsPlayable()
    {
        return true;
    }

    public override int GetCost()
    {
        return 3;
    }
}

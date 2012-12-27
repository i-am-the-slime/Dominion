using UnityEngine;
using System.Collections;

public class Gold : Card {
    public override void Play(IPlayer player)
    {
        player.IncreaseMoney(3);
    }

    public override bool IsPlayable()
    {
        return true;
    }

    public override int GetCost()
    {
        return 6;
    }
}

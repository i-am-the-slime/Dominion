using UnityEngine;
using System.Collections;

public class Curse : Card
{
    public override int GetScore()
    {
        return -1;
    }

    public override bool IsPlayable()
    {
        return false;
    }

    public override int GetCost()
    {
        return 0;
    }
}

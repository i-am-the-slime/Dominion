using UnityEngine;
using System.Collections;

public class Province : Card
{
    public override int GetScore()
    {
        return 6;
    }

    public override bool IsPlayable()
    {
        return false;
    }

    public override int GetCost()
    {
        return 8;
    }
}

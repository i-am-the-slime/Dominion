using UnityEngine;
using System.Collections;

public class Duchy : Card
{
    public override int GetScore()
    {
        return 3;
    }

    public override bool IsPlayable()
    {
        return false;
    }

    public override int GetCost()
    {
        return 5;
    }
}

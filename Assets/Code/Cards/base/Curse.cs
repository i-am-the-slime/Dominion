using UnityEngine;
using System.Collections;

public class Curse : Card
{
    public override int Cost { get { return 0; } }
    public override int Score { get { return -1; } }
    public override CardFlags Flags { get { return CardFlags.Victory; } }
}

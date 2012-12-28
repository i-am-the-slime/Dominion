using UnityEngine;
using System.Collections;

public class Duchy : Card
{
    public override int Cost { get { return 5; } }
    public override int Score { get { return 3; } }
    public override CardFlags Flags { get { return CardFlags.Victory; } }
}
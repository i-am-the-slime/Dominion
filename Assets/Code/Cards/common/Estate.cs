using UnityEngine;
using System.Collections;

public class Estate : Card
{
    public override int Cost { get { return 2; } }
    public override int Score { get { return 1; } }
    public override CardFlags Flags { get { return CardFlags.Victory; } }
}
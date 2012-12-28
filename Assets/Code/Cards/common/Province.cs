using UnityEngine;
using System.Collections;

public class Province : Card
{
    public override int Cost { get { return 8; } }
    public override int Score { get { return 6; } }
    public override CardFlags Flags { get { return CardFlags.Victory; } }
}

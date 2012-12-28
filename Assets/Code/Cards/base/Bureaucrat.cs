using UnityEngine;
using System.Collections;

class Bureaucrat : Card
{
    public override int Cost { get { return 4; } }
    public override int Score { get { return 0; } }
    public override CardFlags Flags { get { return CardFlags.Action | CardFlags.Attack; } }

    public override IEnumerator Play(Player player)
    {
        yield return new WaitForSeconds(1.0f);
        Card card = player.SilverStack.Pop();
        player.DrawStack.Push(card);
        // TODO: Implement the multiplayer part: 
        // Each other player reveals a Victory card from his
        // hand and puts it on his deck (or reveals a hand with no victory cards)
    }
}

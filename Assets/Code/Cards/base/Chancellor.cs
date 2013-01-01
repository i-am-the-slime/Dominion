using UnityEngine;
using System.Collections;

public class Chancellor : Card {
    public override int Cost { get { return 3; } }
    public override int Score { get { return 0; } }
    public override CardFlags Flags { get { return CardFlags.Action; } }
    private Player player;
    public override IEnumerator Play(Player player) {
        player.Money+=2;
        this.player = player;
        GUIManager.modalDialogue = new ModalDialogue("Do you want to put your deck onto your discard pile?", YesCallback, NoCallback);
        return null;
    }

    public void YesCallback(Object[] results)
    {
       player.DrawStack.MoveAllCardsToStack(player.RubbishStack, false); 
       print("Yes"); 
    }

    public void NoCallback(Object[] results)
    {
       print("No"); 
    }
} 

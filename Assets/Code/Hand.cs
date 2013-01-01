using UnityEngine;
using System.Collections;

public class Hand : MonoBehaviour
{
    public Player Player;
    public IList Cards = new ArrayList();
    private const float cardWidth = 0.8f;

    public void DiscardSelectedCards(ArrayList cardList)
    {
        MoveSelectedCardsToStack(cardList, Player.playedStack);
    }

    public void TrashSelectedCards(ArrayList cardList) {
        MoveSelectedCardsToStack(cardList, Player.TrashStack);
    }

    private void MoveSelectedCardsToStack(ArrayList cardList, CardStack stack)
    {
        foreach (Card card in cardList)
        {
            stack.Push(card);
        }
        ReorderCards();
    }

    public IEnumerator ShowHand()
    {
        foreach (Card card in Cards)
        {
            iTween.MoveTo(card.gameObject, card.gameObject.transform.position + new Vector3(0, 1.5f, 0), 0.5f);
        }
        yield return new WaitForSeconds(0.5f);
    }

    public IEnumerator HideHand()
    {
        foreach (Card card in Cards)
        {
            iTween.MoveTo(card.gameObject, card.gameObject.transform.position - new Vector3(0, 1.5f, 0), 0.5f);
        }
        yield return new WaitForSeconds(0.5f);
    }

    public IEnumerator DrawCard(Card card)
    {
        Cards.Add(card);
        //Trying to avoid cards flying back to the hand.
        //yield return new WaitForSeconds(0.2f);
        ReorderCards();
        yield return new WaitForSeconds(0.3f);
    }

    public void DrawCards(int number)
    {
        StartCoroutine(DrawNewCards(number));
    }

    public IEnumerator DrawNewCards(int number)
    {
		Player.guiManager.ChangeMode(new WatchMode(Player));
        // If there are not enough cards, recycle the rubbish stack
        if (Player.DrawStack.Count < number)
        {
            int drawable = Player.DrawStack.Count;
            for (int i = 0; i < drawable; i++)
            {
                yield return StartCoroutine(DrawCard(Player.DrawStack.Pop()));
            }

            Player.RubbishStack.MoveAllCardsToStack(Player.DrawStack, false);
            yield return new WaitForSeconds(1.0f);
            yield return StartCoroutine(Player.DrawStack.Shuffle());

            for (int i = 0; i < number - drawable; i++)
            {
                yield return StartCoroutine(DrawCard(Player.DrawStack.Pop()));
            }
        }
        else
        {
            for (int i = 0; i < number; i++)
            {
                yield return StartCoroutine(DrawCard(Player.DrawStack.Pop()));
            }
        }
        yield return new WaitForSeconds(0.5f);
        Player.guiManager.ChangeMode(new ActionMode(Player));
    }

    public void ReorderCards()
    {
        int index = 0;
        foreach (Card c in Cards)
        {
            iTween.MoveTo(c.gameObject, transform.position + CalcCardPosition(index), 2.0f);
            iTween.RotateTo(c.gameObject, transform.rotation.eulerAngles, 2.0f);
            index++;
        }
    }

    private Vector3 CalcCardPosition(int index)
    {
        float centerIndex = (Cards.Count - 1)/2.0f;
        float xPos = (index - centerIndex)*cardWidth;
        return new Vector3(xPos, 0.0f, 0.0f);
    }

} 


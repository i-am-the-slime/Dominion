using UnityEngine;
using System.Collections;

public interface IPlayer {
    void IncreaseMoney(int by);
    void IncreaseActions(int by);
    void IncreaseBuys(int by);
    int Money
    {
        get;
    }

    void DrawCards(int number);
}

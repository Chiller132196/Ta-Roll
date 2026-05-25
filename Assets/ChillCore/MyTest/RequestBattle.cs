using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestBattle : MonoBehaviour
{
    public void PrepareToBattle()
    {
        ChessKeeper.chessKeeper.PrepareToBattle();
    }
}

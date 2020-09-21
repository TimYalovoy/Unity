using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Field : MonoBehaviour
{
    public abstract void SetUp();

    public abstract bool MatchesAt(int column, int row, GameObject element);
    public abstract void DestroyMatchesAt(int column, int row);
    public abstract void DestroyMatches();
    public abstract IEnumerator DecreaseRowCo();
    public abstract void RefillBoard();
    public abstract bool MatchesOnBoard();
    public abstract IEnumerator FillBoardCo();
    public abstract void ChangeGravityAt(int column, int row);
}

using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public enum GameState
{
    wait,
    move
}

public class Board : Field
{
    [Header("Board State")]
    public GameState currentState = GameState.move;
    [Header("Properties of Board")]
    public int width;
    public int height;
    public int offSet;
    public bool isAntiGravity;

    public GameObject[,] allElements;
    [Header("Current touched element")]
    public Element currentElement;
    private FindMatches findMatches;
    private PoolObject poolObject;
    
    void Start()
    {
        isAntiGravity = false;
        findMatches = FindObjectOfType<FindMatches>();
        poolObject = FindObjectOfType<PoolObject>();
        allElements = new GameObject[width, height];
        SetUp();
    }

    public override void SetUp()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                poolObject.FillBoardOnStart(i, j);
            }
        }
    }

    public override bool MatchesAt(int column, int row, GameObject element)
    {
        if (column > 1 && row > 1)
        {
            if (allElements[column - 1, row] != null && allElements[column - 2, row] != null)
            {
                if (element.CompareTag(allElements[column - 1, row].tag) && element.CompareTag(allElements[column - 2, row].tag))
                {
                    return true;
                }
            }
            if (allElements[column, row - 1] != null && allElements[column, row - 2] != null)
            {
                if (element.CompareTag(allElements[column, row - 1].tag) && element.CompareTag(allElements[column, row - 2].tag))
                {
                    return true;
                }
            }
        }
        else if (column <= 1 || row <= 1)
        {
            if (row > 1)
            {
                if (allElements[column, row - 1] != null && allElements[column, row - 2] != null)
                {
                    if (element.CompareTag(allElements[column, row - 1].tag) && element.CompareTag(allElements[column, row - 2].tag))
                    {
                        return true;
                    }
                }
            }
            if (column > 1)
            {
                if (allElements[column - 1, row] != null && allElements[column - 2, row] != null)
                {
                    if (element.CompareTag(allElements[column - 1, row].tag) && element.CompareTag(allElements[column - 2, row].tag))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public override void DestroyMatchesAt(int column, int row)
    {
        if (allElements[column, row].GetComponent<Element>().isMatched)
        {
            if (findMatches.currentMatches.Count == 4 || findMatches.currentMatches.Count == 5 || findMatches.currentMatches.Count == 7)
            {
                if (currentElement != null)
                {
                    currentElement.CheckPowerUp();
                }
            }
            
            ChangeGravityAt(column, row);

            poolObject.BackToPool(allElements[column, row]);
        }
    }

    public override void DestroyMatches()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allElements[i, j] != null)
                {
                    DestroyMatchesAt(i, j);
                }
            }
        }
        findMatches.currentMatches.Clear();
        StartCoroutine(DecreaseRowCo());
    }

    public override IEnumerator DecreaseRowCo()
    {
        int nullCount = 0;
        if (isAntiGravity)
        {
            for (int i = width - 1; i >= 0 ; i--)
            {
                for (int j = height - 1; j >= 0; j--)
                {
                    if (allElements[i, j].activeInHierarchy == false || allElements[i, j] == null)
                    {
                        nullCount++;
                    }
                    else if (nullCount > 0)
                    {
                        allElements[i, j].GetComponent<Element>().row += nullCount;
                        allElements[i, j] = null;
                    }
                }
                nullCount = 0;
            }
        }
        else
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (allElements[i, j].activeInHierarchy == false || allElements[i, j] == null)
                    {
                        nullCount++;
                    }
                    else if (nullCount > 0)
                    {
                        allElements[i, j].GetComponent<Element>().row -= nullCount;
                        allElements[i, j] = null;
                    }
                }
                nullCount = 0;
            }
        }
        yield return new WaitForSeconds(.4f);
        StartCoroutine(FillBoardCo());
    }

    public override void RefillBoard()
    {
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                if (allElements[i, j] == null || allElements[i, j].activeInHierarchy == false)
                {
                    if (isAntiGravity)
                    {
                        poolObject.CallFromPool(i, j, (j - (offSet - 2)));
                    } else
                    {
                        poolObject.CallFromPool(i, j, (j + offSet));
                    }
                }
            }
        }
    }

    public override bool MatchesOnBoard()
    {
        for(int i=0; i < width; i++)
        {
            for(int j=0; j < height; j++)
            {
                if (allElements[i, j] != null)
                {
                    if (allElements[i, j].GetComponent<Element>().isMatched)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public override IEnumerator FillBoardCo()
    {
        RefillBoard();
        yield return new WaitForSeconds(.5f);

        while (MatchesOnBoard())
        {
            currentState = GameState.wait;
            yield return new WaitForSeconds(.28f);
            DestroyMatches();
        }
        findMatches.currentMatches.Clear();
        currentElement = null;
        yield return new WaitForSeconds(.5f);
        currentState = GameState.move;
    }

    public override void ChangeGravityAt(int column, int row)
    {
        if (allElements[column, row].GetComponent<Element>().isAntiGravity)
        {
            this.isAntiGravity = !this.isAntiGravity;
        }
    }
}

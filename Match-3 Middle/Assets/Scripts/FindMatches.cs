using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class FindMatches : MonoBehaviour
{
    private Board board;
    public List<GameObject> currentMatches = new List<GameObject>();

    void Start()
    {
        board = FindObjectOfType<Board>();
    }

    public void FindAllMatches()
    {
        StartCoroutine(FindAllMatchesCo());
    }

    private void AddToListAndMatch(GameObject element)
    {
        if (!currentMatches.Contains(element))
        {
            currentMatches.Add(element);
        }
        element.GetComponent<Element>().isMatched = true;
    }

    public void GetNearbyElements(GameObject element1, GameObject element2, GameObject element3)
    {
        AddToListAndMatch(element1);
        AddToListAndMatch(element2);
        AddToListAndMatch(element3);
    }

    private IEnumerator FindAllMatchesCo()
    {
        yield return new WaitForSeconds(.2f);
        GameObject leftElement;
        GameObject rightElement;
        GameObject upElement;
        GameObject downElement;
        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                GameObject currentElement = board.allElements[i, j];
                if (currentElement != null)
                {
                    if (i > 0 && i < board.width - 1)
                    {
                        leftElement = board.allElements[i - 1, j];
                        rightElement = board.allElements[i + 1, j];
                        if (leftElement != null && rightElement != null)
                        {
                            if (currentElement.CompareTag(leftElement.tag) && currentElement.CompareTag(rightElement.tag))
                            {
                                GetNearbyElements(leftElement, currentElement, rightElement);
                            }
                        }
                    }

                    if (j > 0 && j < board.height - 1)
                    {
                        upElement = board.allElements[i, j + 1];
                        downElement = board.allElements[i, j - 1];
                        if (upElement != null && downElement != null)
                        {
                            if (currentElement.CompareTag(upElement.tag) && currentElement.CompareTag(downElement.tag))
                            {
                                GetNearbyElements(upElement, currentElement, downElement);
                            }
                        }
                    }
                }
            }
        }
    }
}

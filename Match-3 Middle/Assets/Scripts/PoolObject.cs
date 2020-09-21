using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
    public GameObject[] elements;
    
    public Board board;

    public Queue<GameObject> queueOfObjects = new Queue<GameObject>();

    private PoolObject() { }

    private static PoolObject _instance;

    public static PoolObject GetInstance()
    {
        if (_instance == null)
        {
            _instance = new PoolObject();
        }
        return _instance;
    }

    public void FillBoardOnStart(int i, int j)
    {
        Vector2 tempPosition = new Vector2(i, j);
        int elementToUse = Random.Range(0, elements.Length);


        if (i == board.width - 1 && j == board.height - 1)
        {
            for (int _i = 0; _i < 8; _i++)
            {
                for (int _j = 0; _j < 15; _j++)
                {
                    int _elementToUse = Random.Range(0, elements.Length);
                    GameObject _element = Instantiate(elements[_elementToUse], tempPosition, Quaternion.identity);
                    _element.GetComponent<Element>().row = _j;
                    _element.GetComponent<Element>().column = _i;
                    _element.transform.parent = board.transform;
                    _element.SetActive(false);
                    queueOfObjects.Enqueue(_element);
                }
            }
        }


        int maxIterations = 0;
        while (board.MatchesAt(i, j, elements[elementToUse]) && maxIterations < 100)
        {
            elementToUse = Random.Range(0, elements.Length);
            maxIterations++;
        }
        maxIterations = 0;

        GameObject element = Instantiate(elements[elementToUse], tempPosition, Quaternion.identity);
        element.GetComponent<Element>().row = j;
        element.GetComponent<Element>().column = i;
        element.transform.parent = board.transform;
        element.name = $"({i}, {j}){element.tag}";

        element.SetActive(true);

        board.allElements[i, j] = element;
    }

    public void CallFromPool(int i, int j, int offSet)
    {
        Vector2 tempPosition = new Vector2(i, offSet);

        GameObject element = queueOfObjects.Dequeue();
        element.transform.position = tempPosition;

        element.GetComponent<Element>().row = j;
        element.GetComponent<Element>().column = i;
        element.transform.parent = board.transform;
        element.name = $"({i}, {j}){element.tag}";
        board.allElements[i, j] = element;

        element.SetActive(true);
    }

    public void BackToPool(GameObject allElements)
    {
        if(allElements != null)
        {
            allElements.GetComponent<Element>().isMatched = false;
            allElements.GetComponent<Element>().isAntiGravity = false;
            allElements.GetComponent<Element>().otherElement = null;

            if (allElements.transform.childCount > 0)
            {
                allElements.transform.GetChild(0).gameObject.SetActive(false);
            }
            allElements.SetActive(false);
            queueOfObjects.Enqueue(allElements);
        } else
        {
            Debug.Log("allElements is null");
        }
    }
}

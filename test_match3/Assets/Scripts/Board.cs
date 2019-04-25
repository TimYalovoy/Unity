using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int width; //ширина доски
    public int height; //высота доски
    public int offSet; 
    public GameObject[] elements; //создаём массив(одномерный) плиток
    public GameObject tilePrefab; //объявляем перемунную типа GameObject.
                                  //В эту переменную пихаем спрайт фоновой плитки

    public GameObject[,] allElements; //объявляем двумерный массив плиток
    private BackgroundTile[,] allTiles; //объявляем двумерный массив фоновых плиток

    [SerializeField] private TextMesh scoreLabel;
    private int _score = 0;

    void Start()
    {
        allTiles = new BackgroundTile[width, height]; //инициализируем двумерный массив
        allElements = new GameObject[width, height]; 
        SetUp(); // вызываем метод заполнения доски плитками
    }

    void Update()
    {
        
    }

    private void SetUp() // метода установки доски
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2 tempPosition = new Vector2(i, j + offSet);
                GameObject backgroundTile = Instantiate(tilePrefab, tempPosition, Quaternion.identity) as GameObject;
                //для удобства отладки и разгрузки Иерархии в Юнити создаём игровой объект в котором будут все плитки доски
                backgroundTile.transform.parent = this.transform;
                backgroundTile.name = "( " + i + ", " + j + " )"; // называю каждую плитку её координатми относительно доски(для удобства отладки)

                int elementToUse = Random.Range(0, elements.Length);
                int maxInterations = 0;
                while (MathcesAt(i, j, elements[elementToUse]) && maxInterations < 100)
                {
                    elementToUse = Random.Range(0, elements.Length);
                    maxInterations++;
                    Debug.Log(maxInterations);
                }
                maxInterations = 0;

                GameObject element = Instantiate(elements[elementToUse], tempPosition, Quaternion.identity);
                element.GetComponent<Element>().row = j;
                element.GetComponent<Element>().column = i;
                element.transform.parent = this.transform;
                element.name = "( " + i + ", " + j + " )";
                allElements[i, j] = element;
            }
        }
    }

    private bool MathcesAt(int column, int row, GameObject figure)
    {//изначально figure назывался element, но во избежании путаницы и для удобства чтения кода, переименовал
        if (column > 1 && row > 1)
        {
            if (allElements[column - 1, row].tag == figure.tag && allElements[column - 2, row].tag == figure.tag)
            {
                return true;
            }
            if (allElements[column, row - 1].tag == figure.tag && allElements[column, row - 2].tag == figure.tag)
            {
                return true;
            }
        }else if(column <=1 || row <=1)
        {
            if(row > 1)
            {
                if (allElements[column, row - 1].tag == figure.tag && allElements[column, row - 2].tag == figure.tag)
                {
                    return true;
                }
            }
            if (column > 1)
            {
                if (allElements[column - 1, row].tag == figure.tag && allElements[column - 2, row].tag == figure.tag)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void DestroyMatchesAt(int column, int row)
    {
        if (allElements[column, row].GetComponent<Element>().isMatched)
        {
            Destroy(allElements[column, row]);
            allElements[column, row] = null;
            _score++;
            scoreLabel.text = "Score: " + _score;
        }
    }

    public void DestroyMathes()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if(allElements[i,j] != null)
                { 
                    DestroyMatchesAt(i, j);
                }
            }
        }
        StartCoroutine(DecreaseRowCoroutine());
    }

    private IEnumerator DecreaseRowCoroutine()//реализация "падения" элементов после три-в-ряд //является сопрограммой
    {
        int nullCount = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allElements[i, j] == null)
                {
                    nullCount++;
                } else if(nullCount > 0)
                {
                    allElements[i, j].GetComponent<Element>().row -= nullCount;
                    allElements[i, j] = null;
                }
            }
            nullCount = 0;
        }
        yield return new WaitForSeconds(.4f);
        StartCoroutine(FillBoardCoroutine());
    }

    private void RefillBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for(int j=0; j < height; j++)
            {
                if (allElements[i, j] == null)
                {
                    Vector2 tempPosition = new Vector2(i, j + offSet);
                    int elementToUse = Random.Range(0, elements.Length);
                    GameObject figure = Instantiate(elements[elementToUse], tempPosition, Quaternion.identity);
                    allElements[i, j] = figure;
                    figure.GetComponent<Element>().row = j;
                    figure.GetComponent<Element>().column = i;
                }
            }
        }
    }

    private bool MatchesOnBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if(allElements[i, j] != null)
                {
                    if(allElements[i, j].GetComponent<Element>().isMatched)
                    {
                        _score++;
                        scoreLabel.text = "Score: " + _score;
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private IEnumerator FillBoardCoroutine()
    {
        RefillBoard();
        yield return new WaitForSeconds(.5f);

        while (MatchesOnBoard())
        {
            yield return new WaitForSeconds(.5f);
            DestroyMathes();
        }
    }

    public void Restart()
    {
        Application.LoadLevel("SampleScene");
    }
}

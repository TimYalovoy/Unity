using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour
{
    [Header("Board Variables")]
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    private Vector2 tempPosition;
    private GameObject otherElement;
    private Board board;

    public int column;
    public int row;
    public int prevColumn;
    public int prevRow;
    public int targetX;
    public int targetY;
    public float swipeAngle = 0;
    public float swipeResist = 1f;
    public bool isMatched = false;

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        //targetX = (int)transform.position.x;
        //targetY = (int)transform.position.y;
        //column = targetX;
        //row = targetY;
        //prevRow = row;
        //prevColumn = column;
    }

    // Update is called once per frame
    void Update()
    {
        FindMatches();
        if (isMatched)
        {
            SpriteRenderer mySprite = GetComponent<SpriteRenderer>(); // для отладки правильности алгоритма
            mySprite.color = new Color(1f, 1f, 1f, .2f); // вместо закрашивание добавить метод уничтожения совпавших элементов
        }

        targetX = column;
        targetY = row;
        if(Mathf.Abs(targetX - transform.position.x) > .1)
        {
            //движение к цели
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .7f);
            if(board.allElements[column, row] != this.gameObject)
            {
                board.allElements[column, row] = this.gameObject;//предотвращение наложения спрайтов друг на друга
            }
        } else
        {
            //установить позицию
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;
        }
        if (Mathf.Abs(targetY - transform.position.y) > .1)
        {
            //движение к цели
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .7f);
            if (board.allElements[column, row] != this.gameObject)
            {
                board.allElements[column, row] = this.gameObject;//предотвращение наложения спрайтов друг на друга
            }
        }
        else
        {
            //установить позицию
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;
        }
    }

    public IEnumerator CheckMoveCoroutine() //сопрограмма проверки правильности хода
    {
        yield return new WaitForSeconds(.5f);
        if (otherElement != null)
        {
            if(!isMatched && !otherElement.GetComponent<Element>().isMatched)
            {
                otherElement.GetComponent<Element>().row = row;
                otherElement.GetComponent<Element>().column = column;
                row = prevRow;
                column = prevColumn;
            }else
            {
                board.DestroyMathes();
            }

            otherElement = null;
        }
        
    }

    private void OnMouseDown()
    {
        firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log(firstTouchPosition);
    }

    private void OnMouseUp()
    {
        finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalculateAngle();
    }

    void CalculateAngle()
    {
        if (Mathf.Abs(finalTouchPosition.y - firstTouchPosition.y) > swipeResist || Mathf.Abs(finalTouchPosition.x - firstTouchPosition.x) > swipeResist)
        {//исключаем возможность случайного нажатия(касания)
            swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * (180 / Mathf.PI);
            moveElement();
        }
    }

    void moveElement()
    {
        if(swipeAngle > -45 && swipeAngle <= 45 && column < board.width - 1)//Right swipe
        {
            otherElement = board.allElements[column + 1, row];
            prevRow = row;
            prevColumn = column;
            otherElement.GetComponent<Element>().column -= 1;
            column += 1;
        }
        else
        if (swipeAngle > 45 && swipeAngle <= 135 && row < board.height - 1)//Up swipe
        {
            otherElement = board.allElements[column, row + 1];
            prevRow = row;
            prevColumn = column;
            otherElement.GetComponent<Element>().row -= 1;
            row += 1;
        }
        else
        if ((swipeAngle > 135 || swipeAngle <= -135) && column > 0)//Left swipe
        {
            otherElement = board.allElements[column - 1, row];
            prevRow = row;
            prevColumn = column;
            otherElement.GetComponent<Element>().column += 1;
            column -= 1;
        }
        else
        if (swipeAngle < -45 && swipeAngle >= -135 && row > 0)//Down swipe
        {
            otherElement = board.allElements[column, row - 1];
            prevRow = row;
            prevColumn = column;
            otherElement.GetComponent<Element>().row += 1;
            row -= 1;
        }
        StartCoroutine(CheckMoveCoroutine());
    }

    void FindMatches()
    {
        if(column > 0 && column < board.width - 1)
        {
            GameObject leftElement1 = board.allElements[column - 1, row];
            GameObject rightElement1 = board.allElements[column + 1, row];
            if (leftElement1 != null && rightElement1 != null)
            {
                if (leftElement1.tag == this.gameObject.tag && rightElement1.tag == this.gameObject.tag)
                {
                    leftElement1.GetComponent<Element>().isMatched = true;
                    rightElement1.GetComponent<Element>().isMatched = true;
                    isMatched = true;

                }
            }
        }
        if (row > 0 && row < board.height - 1)
        {
            GameObject upElement1 = board.allElements[column, row + 1];
            GameObject downElement1 = board.allElements[column, row - 1];
            if(upElement1 != null && downElement1 != null)
            {
                if (upElement1.tag == this.gameObject.tag && downElement1.tag == this.gameObject.tag)
                {
                    upElement1.GetComponent<Element>().isMatched = true;
                    downElement1.GetComponent<Element>().isMatched = true;
                    isMatched = true;
                   
                }
            }
        }
    }
}

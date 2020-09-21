using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Permissions;
using UnityEngine;

public class Element : Member
{
    [Header("Board Variables")]
    public int column;
    public int row;
    public int previousColumn;
    public int previousRow;
    public int targetX;
    public int targetY;
    public bool isMatched;

    private FindMatches findMatches;
    private Board board;
    public GameObject otherElement;

    private Vector2 tempPosition;
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    [Header("Swipe Variables")]
    public double swipeAngle = 0;
    public double swipeResist = 1f;

    [Header("PowerUp Staff")]
    public bool isAntiGravity;
    public GameObject antiGravityPowerUp;

    void Start()
    {
        isMatched = false;
        isAntiGravity = false;
        board = FindObjectOfType<Board>();
        findMatches = FindObjectOfType<FindMatches>();
    }

    void Update()
    {
        targetX = column;
        targetY = row;

        if(Mathf.Abs(targetX - transform.position.x) > .1)
        {
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .6f);
            if (board.allElements[column, row] != this.gameObject)
            {
                board.allElements[column, row] = this.gameObject;
            }
            findMatches.FindAllMatches();
        } else
        {
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;
        }
        if (Mathf.Abs(targetY - transform.position.y) > .1)
        {
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .6f);
            if (board.allElements[column, row] != this.gameObject)
            {
                board.allElements[column, row] = this.gameObject;
            }
            findMatches.FindAllMatches();
        }
        else
        {
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;
        }
    }

    public override IEnumerator CheckMoveCo()
    {
        yield return new WaitForSeconds(.5f);
        if (otherElement != null)
        {
            if (!isMatched && !otherElement.GetComponent<Element>().isMatched)
            {
                otherElement.GetComponent<Element>().column = column;
                otherElement.GetComponent<Element>().row = row;
                column = previousColumn;
                row = previousRow;
                yield return new WaitForSeconds(.5f);
                board.currentElement = null;
                board.currentState = GameState.move;
            }
            else
            {
                board.DestroyMatches();
            }
        }
    }

    private void OnMouseDown()
    {
        if (board.currentState == GameState.move)
        {
            firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    private void OnMouseUp()
    {
        if (board.currentState == GameState.move) {
            finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CalculateAngle();
        }
    }

    public override void CalculateAngle()
    {
        if (Mathf.Abs(finalTouchPosition.y - firstTouchPosition.y) > swipeResist || Mathf.Abs(finalTouchPosition.x - firstTouchPosition.x) > swipeResist)
        {
            board.currentState = GameState.wait;
            swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * (180/Math.PI);
            MoveElements();
            board.currentElement = this;
        } else
        {
            board.currentState = GameState.move;
        }
    }

    void MoveElementsActual(Vector2 direction)
    {
        otherElement = board.allElements[column + (int)direction.x, row + (int)direction.y];
        previousColumn = column;
        previousRow = row;
        if (otherElement != null)
        {
            otherElement.GetComponent<Element>().column += -1 * (int)direction.x;
            otherElement.GetComponent<Element>().row += -1 * (int)direction.y;
            column += (int)direction.x;
            row += (int)direction.y;
            StartCoroutine(CheckMoveCo());
        }
        else
        {
            board.currentState = GameState.move;
        }
    }

    public override void MoveElements()
    {
        if(swipeAngle > -45 && swipeAngle <= 45 && column < board.width - 1)
        {
            MoveElementsActual(Vector2.right);
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < board.height - 1)
        {
            MoveElementsActual(Vector2.up);
        }
        else if ((swipeAngle > 135 || swipeAngle <= -135) && column > 0)
        {
            MoveElementsActual(Vector2.left);
        }
        else if (swipeAngle > -135 || swipeAngle <= -45 && row > 0)
        {
            MoveElementsActual(Vector2.down);
        } else
        {
            board.currentState = GameState.move;
        }
    }

    public override void CheckPowerUp()
    {
        if (board.currentElement != null)
        {
            if (board.currentElement.isMatched)
            {
                board.currentElement.isMatched = false;
                board.currentElement.MakePowerUp_ChangeGravity();
            }
            else
            if (board.currentElement.otherElement != null)
            {
                Element otherElement = board.currentElement.otherElement.GetComponent<Element>();
                if (otherElement.isMatched)
                {
                    otherElement.isMatched = false;
                    otherElement.MakePowerUp_ChangeGravity();
                }
            }
        }
    }

    public override void MakePowerUp_ChangeGravity()
    {
        isAntiGravity = true;
        if (this.transform.childCount == 0)
        {
            GameObject powerUp = Instantiate(antiGravityPowerUp, transform.position, Quaternion.identity);
            powerUp.transform.parent = this.transform;
        } else
        {
            this.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}

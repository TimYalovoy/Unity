using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float posX;
    public float posY;
    bool isMove = false;
    private Rigidbody2D rb;

    Vector2 begPos;
    Vector2 endPos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        endPos = rb.position;
        posX = rb.position.x;
        posY = rb.position.y;
    }

    void Update()
    {
        // LEFT
        if (Input.GetKeyDown(KeyCode.A))
        {
            isMove = true;
            posX--;
        }
        if (isMove)
        {
            begPos = rb.position;
            endPos = new Vector3(Mathf.Lerp(transform.position.x, posX, 1f), rb.position.y, 1f);
            
            transform.position = endPos;
            //если дошел  isMove = false;
            if(endPos != begPos)
            {
                isMove = false;
            }
        }
        //RIGHT
        if (Input.GetKeyDown(KeyCode.D))
        {
            isMove = true;
            posX++;
        }
        if (isMove)
        {
            begPos = rb.position;
            endPos = new Vector3(Mathf.Lerp(transform.position.x, posX, 1f), rb.position.y, -1f);
            
            transform.position = endPos;
            if (endPos != begPos)
            {
                isMove = false;
            }
        }
        //UP
        if (Input.GetKeyDown(KeyCode.W))
        {
            isMove = true;
            posY++;
        }
        if (isMove)
        {
            begPos = rb.position;
            endPos = new Vector3(rb.position.x, Mathf.Lerp(transform.position.y, posY, 1f), 1f);
            
            transform.position = endPos;
            if (endPos != begPos)
            {
                isMove = false;
            }
            
        }
        //DOWN
        if (Input.GetKeyDown(KeyCode.S))
        {
            isMove = true;
            posY--;
        }
        if (isMove)
        {
            begPos = rb.position;
            endPos = new Vector3(rb.position.x, Mathf.Lerp(transform.position.y, posY, 1f), -1f);
            
            transform.position = endPos;
            if (endPos != begPos)
            {
                isMove = false;
            }
            
        }
    }
}

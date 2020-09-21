using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class CameraScaler : MonoBehaviour
{
    private Board board;
    public float aspectRatio = 0.5625f;
    public float padding = 2;

    void Start()
    {
        board = FindObjectOfType<Board>();
        if(board != null)
        {
            RepositionCamera(board.width -1 , board.height -1);
        }
    }

    void RepositionCamera(float x, float y)
    {
        Vector3 tempPosition = new Vector3(x / 2, y / 2, -10f);
        transform.position = tempPosition;
        if(board.width >= board.height)
        {
            Camera.main.orthographicSize = ((board.width / 2) + padding) / aspectRatio;

        } else
        {
            Camera.main.orthographicSize = (board.height / 2) + padding;
        }
    }
}

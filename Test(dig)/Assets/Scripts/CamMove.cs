using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMove : MonoBehaviour
{
    public GameObject player;

    void Start()
    {
        
    }

    void Update()
    {
        if (player.transform.position.x == 7 || player.transform.position.x <= 7.15 )
        {
            if (player.transform.position.y == 5 || player.transform.position.y >= 5.5)
            {
                transform.position = new Vector3(7.15f, 5.5f, -100f);
            }
            else transform.position = new Vector3(7.15f, player.transform.position.y, -100f);
        }
        else if (player.transform.position.x == 21 || player.transform.position.x >= 21.85)
        {
            if (player.transform.position.y == 5 || player.transform.position.y >= 5.5)
            {
                transform.position = new Vector3(21.85f, 5.5f, -100f);
            }
            else transform.position = new Vector3(21.85f, player.transform.position.y, -100f);
        }
        else if (player.transform.position.y == 5 || player.transform.position.y >= 5.5)
        {
            transform.position = new Vector3(player.transform.position.x, 5.5f, -100f);
        }
        else transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -100f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    [SerializeField] private TextMesh coinsLabel;
    private int _coins = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Diamond" || collision.gameObject.tag == "Dirt")
        {
            Destroy(collision.gameObject);
            _coins++;
            coinsLabel.text = "Coins: " + _coins;
        }
    }
}

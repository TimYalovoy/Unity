using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryCard : MonoBehaviour
{
    [SerializeField] private GameObject cardback;
    [SerializeField] private SceneController controller;

    private int _id;
    public int id
    {
        get { //функция чтения(идиома)
            return _id;
        }
    }

    public void SetCard(int id, Sprite image)
    {
        _id = id;
        GetComponent<SpriteRenderer>().sprite = image;
    }

    public void OnMouseDown()
    {
        if (cardback.activeSelf || controller.canReveal)
        {
            cardback.SetActive(false);
            controller.CardRevealed(this);
        }
    }

    public void Unreveal()
    {
        cardback.SetActive(true);
    }

    void Start()
    {

    }

    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBtn : MonoBehaviour
{
    [SerializeField]
    private GameObject towerObject;
    [SerializeField]
    private Sprite dragSprite;
    [SerializeField]
    private int towerPrice;

    public GameObject TowerObject
    {
        get
        {
            return towerObject;
        }
    }

    public int TowerPrice
    {
        get
        {
            return towerPrice;
        }
        set
        {
            towerPrice = value;
        }
    }

    public Sprite DragSprite
    {
        get
        {
            return dragSprite;
        }
    }
    
}

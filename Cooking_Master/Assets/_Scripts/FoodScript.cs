using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodScript : MonoBehaviour
{
    public GameObject UnprocessedFood;
    public GameObject ProcessedFood;
    public bool prepared;

    public bool isServed;

    void Update()
    {
        //when the food is served is the only time the item will not be updating its position
        if (!isServed)
        {
            this.gameObject.transform.position = this.gameObject.transform.parent.gameObject.transform.position;
        }
    }
}

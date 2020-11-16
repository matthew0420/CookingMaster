using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodScript : MonoBehaviour
{
    public GameObject UnprocessedFood;
    public GameObject ProcessedFood;

    void Update()
    {
        this.gameObject.transform.position = this.gameObject.transform.parent.gameObject.transform.position;
    }
}

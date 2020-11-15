using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodScript : MonoBehaviour
{
    void Update()
    {
        this.gameObject.transform.position = this.gameObject.transform.parent.gameObject.transform.position;
    }
}

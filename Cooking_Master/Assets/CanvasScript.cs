using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour
{
    private Canvas thisCanvas;
    private GameObject thisSlider;
    private GameObject customer;

    void Start()
    {
        thisCanvas = this.gameObject.GetComponent<Canvas>();
        thisCanvas.worldCamera = GameObject.FindGameObjectWithTag("MainCamera").gameObject.GetComponent<Camera>();
        thisSlider = thisCanvas.transform.GetChild(0).gameObject;
        customer = thisCanvas.transform.parent.gameObject;
        thisSlider.transform.position = new Vector3(customer.transform.position.x, customer.transform.position.y - 2.25f, customer.transform.position.z);
    }
}


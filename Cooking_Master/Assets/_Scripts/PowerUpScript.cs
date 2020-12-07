using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpScript : MonoBehaviour
{
    public GameObject player;
    public string powerUpType;

    public Sprite speedSprite;
    public Sprite timeSprite;
    public Sprite scoreSprite;

    void Start()
    {
        transform.parent = GameObject.FindGameObjectWithTag("MainCanvas").transform;
        //var randNum = Random.Range(0, 3);
        var randNum = 2;
        switch (randNum)
        {
            case 0:
                powerUpType = "Speed";
                GetComponent<SpriteRenderer>().sprite = speedSprite;
                break;

            case 1:
                powerUpType = "Time";
                GetComponent<SpriteRenderer>().sprite = timeSprite;
                break;

            case 2:
                powerUpType = "Score";
                GetComponent<SpriteRenderer>().sprite = scoreSprite;
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppingScript : MonoBehaviour
{
    public List<GameObject> ItemsOnBoard;

    private GameObject playerObject;
    private PlayerScript playerScript;

    private GameObject choppingBar;
    private float choppingTimer;
    public float maxChoppingTime;

    public bool readyToChop;
    public bool isChopping;

    private void Start()
    {
        //assign maxchoppingtime as the max number shown on the ui progress bar for when an item is chopped
    }

    private void FixedUpdate()
    {
        if (isChopping && choppingTimer < maxChoppingTime)
        {
            choppingTimer += Time.deltaTime;
        }
        if (isChopping && choppingTimer >= maxChoppingTime)
        {
            isChopping = false;

        }
    }

    public void AddToBoard(GameObject ItemToChop)
    {
        playerScript = ItemToChop.GetComponentInParent<PlayerScript>();
        playerObject = ItemToChop.transform.parent.gameObject;
        ItemToChop.transform.SetParent(this.gameObject.transform);
        ItemsOnBoard.Add(ItemToChop);
        readyToChop = true;
    }

    public void StartChopping()
    {
        readyToChop = false;
        isChopping = true;
    }
    //add a switch statement to look through names of currently placed chopping food item to turn into chopped version
    //run a timer in fixedupdate that runs down how much time is left to chop (show progress with a status bar under the chopping board
    //chopped version of ingredients shows up on chopping board
    //parent all of the chopping board items together so that when one is picked up, they are all picked up as a pile of ingredients

    //PLAYER SHOULD NOT BE ABLE TO MOVE WHILE CHOPPING
}

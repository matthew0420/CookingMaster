using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public Slider ChoppingBoardSlider;

    private void Start()
    {
        ChoppingBoardSlider.maxValue = maxChoppingTime;
    }

    private void FixedUpdate()
    {
        if (isChopping && choppingTimer < maxChoppingTime)
        {
            choppingTimer += Time.deltaTime;
            ChoppingBoardSlider.value = choppingTimer;
        }
        if (isChopping && choppingTimer >= maxChoppingTime)
        {
            isChopping = false;
            ChoppingBoardSlider.value = 0;
            choppingTimer = 0;
            if (ItemsOnBoard.Count > 0)
            {
                ProcessChoppedFood(ItemsOnBoard[ItemsOnBoard.Count - 1]);
            }
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

    private void ProcessChoppedFood(GameObject ItemToProcess)
    {
        var myProcessedFood = Instantiate(ItemToProcess.GetComponent<FoodScript>().ProcessedFood, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        myProcessedFood.transform.parent = this.gameObject.transform;
        myProcessedFood.transform.position = new Vector3(0, 0, 0);
        myProcessedFood.transform.localScale = new Vector3(1.3f, 1.8f, 1.3f);

        var OldFoodItem = ItemsOnBoard[ItemsOnBoard.Count - 1];
        ItemsOnBoard.Remove(ItemsOnBoard[ItemsOnBoard.Count - 1]);
        Destroy(OldFoodItem);

        ItemsOnBoard.Add(myProcessedFood);
    }
    //add a switch statement to look through names of currently placed chopping food item to turn into chopped version
    //run a timer in fixedupdate that runs down how much time is left to chop (show progress with a status bar under the chopping board
    //chopped version of ingredients shows up on chopping board
    //parent all of the chopping board items together so that when one is picked up, they are all picked up as a pile of ingredients

    //PLAYER SHOULD NOT BE ABLE TO MOVE WHILE CHOPPING
    //IF READY TO CHOP IS FALSE (NO INGREDIENT TO BE CHOPPED AND IF A PROCESSED FOOD IS ON THE CUTTING BOARD) PRESS CTRL TO
    //PICK UP THE CONTENTS OF THE BOARD (THIS WILL RUN A LOOP AND PICK UP AND REMOVE EACH ITEM FROM THE ITEMSONBOARD LIST)
}

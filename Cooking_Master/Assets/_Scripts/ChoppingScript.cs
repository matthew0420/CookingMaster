using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoppingScript : MonoBehaviour
{
    public List<GameObject> ItemsOnBoard;

    public GameObject playerObject;
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
            playerScript.canMove = true;
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
        ItemToChop.transform.SetParent(this.gameObject.transform);
        ItemsOnBoard.Add(ItemToChop);
        readyToChop = true;
    }

    public void StartChopping(GameObject player)
    {
        playerScript = player.GetComponent<PlayerScript>();
        playerObject = player;

        readyToChop = false;
        isChopping = true;
    }

    private void ProcessChoppedFood(GameObject ItemToProcess)
    {
        var myProcessedFood = Instantiate(ItemToProcess.GetComponent<FoodScript>().ProcessedFood, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        myProcessedFood.transform.parent = this.gameObject.transform;
        myProcessedFood.transform.position = new Vector3(0, 0, 0);
        myProcessedFood.transform.localScale = new Vector3(1.3f, 1.8f, 1.3f);
        myProcessedFood.GetComponent<FoodScript>().prepared = true;

        var OldFoodItem = ItemsOnBoard[ItemsOnBoard.Count - 1];
        ItemsOnBoard.Remove(ItemsOnBoard[ItemsOnBoard.Count - 1]);
        Destroy(OldFoodItem);
        
        ItemsOnBoard.Add(myProcessedFood);
    }

    public void PickUpChoppedFood(GameObject player)
    {
        var choppedIngredients = new GameObject("choppedIngredients");
        choppedIngredients.tag = "ChoppedFood";
        choppedIngredients.AddComponent<FoodScript>();
        choppedIngredients.GetComponent<FoodScript>().prepared = true;
        choppedIngredients.transform.parent = player.transform.GetChild(player.GetComponent<PlayerScript>().Inventory.Count).gameObject.transform;
        choppedIngredients.transform.localPosition = new Vector3(0, 0, 0);


        for (int i = 0; i < ItemsOnBoard.Count; i++)
        {
            ItemsOnBoard[i].gameObject.transform.parent = choppedIngredients.transform;
            ItemsOnBoard[i].gameObject.transform.position = new Vector3(0, 0, 0);
        }
        ItemsOnBoard.Clear();
        player.GetComponent<PlayerScript>().Inventory.Add(choppedIngredients);
    }
    //check if inventory of chopping board is greater than 0 while ready to chop is false
    //IF READY TO CHOP IS FALSE (NO INGREDIENT TO BE CHOPPED AND IF A PROCESSED FOOD IS ON THE CUTTING BOARD) PRESS CTRL TO
    //PICK UP THE CONTENTS OF THE BOARD (THIS WILL RUN A LOOP AND PICK UP AND REMOVE EACH ITEM FROM THE ITEMSONBOARD LIST)
}

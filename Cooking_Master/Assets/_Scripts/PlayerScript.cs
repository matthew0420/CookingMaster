using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerScript : MonoBehaviour
{
    public float speed = 2f;
    public List<GameObject> Inventory;
    public List<GameObject> FoodItems;

    public bool isPlayer1;
    public bool pressingShift;
    public bool pressingControl;
    public bool canMove = true;

    Rigidbody2D rigidBody;
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!canMove)
        {
            rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
        } else
        {
            rigidBody.constraints = RigidbodyConstraints2D.None;
            rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        switch (isPlayer1)
        {
            case true:
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    pressingShift = true;
                }
                if (Input.GetKeyUp(KeyCode.LeftShift))
                {
                    pressingShift = false;
                }
                if (Input.GetKeyDown(KeyCode.LeftControl))
                {
                    pressingControl = true;
                }
                if (Input.GetKeyUp(KeyCode.LeftControl))
                {
                    pressingControl = false;
                }
                break;
            case false:
                if (Input.GetKeyDown(KeyCode.RightShift))
                {
                    pressingShift = true;
                }
                if (Input.GetKeyUp(KeyCode.RightShift))
                {
                    pressingShift = false;
                }
                if (Input.GetKeyDown(KeyCode.RightControl))
                {
                    pressingControl = true;
                }
                if (Input.GetKeyUp(KeyCode.RightControl))
                {
                    pressingControl = false;
                }
                break;
        }
    }

    void FixedUpdate()
    {
        Vector3 input = Vector3.zero;
        if (isPlayer1)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");
        }
        else
        {
            input.x = Input.GetAxisRaw("Horizontal2");
            input.y = Input.GetAxisRaw("Vertical2");
        }

        Vector3 direction = input.normalized;
        Vector3 movement = direction * speed * Time.fixedDeltaTime;
        rigidBody.MovePosition(transform.position + movement);    
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Inventory.Count < 2)
        {
            if (collision.gameObject.tag == "FoodItem" 
                && pressingShift)
            {
                Debug.Log("pressing!");
                pressingShift = false;
                InstantiateFoodItem(collision.gameObject.name);
                return;
            }
        }

        if (Inventory.Count > 0)
        {
            //runs if the player does not have items on the board that are 'ready to chop' 
            if (collision.gameObject.tag == "CuttingBoard" 
                && pressingShift
                && collision.gameObject.GetComponent<ChoppingScript>().readyToChop == false
                && Inventory[0].GetComponent<FoodScript>().prepared == false)
            {
                pressingShift = false;
                collision.GetComponent<ChoppingScript>().AddToBoard(Inventory[0]);
                Inventory.Remove(Inventory[0]);
                if(Inventory.Count > 0)
                {
                    Inventory[0].transform.parent = gameObject.transform.GetChild(Inventory.Count - 1).gameObject.transform;
                    return;
                }
            }
        }

        //runs if the player has placed a 'ready to chop' item on the chopping board
        if (collision.gameObject.tag == "CuttingBoard" 
            && pressingControl
            && collision.gameObject.GetComponent<ChoppingScript>().readyToChop == true)
        {
            pressingControl = false;
            canMove = false;
            collision.gameObject.GetComponent<ChoppingScript>().StartChopping(this.gameObject);
        }
        //if itemsonboard.count is greater than 0 but ready to chop is false, this means there must be processed food on the cutting board
        //pressing control at this point will gather all of the chopped ingredients on the board and pick them up
        if (collision.gameObject.tag == "CuttingBoard" 
            && pressingControl
            && collision.gameObject.GetComponent<ChoppingScript>().readyToChop == false 
            && collision.gameObject.GetComponent<ChoppingScript>().ItemsOnBoard.Count > 0)
        {
            pressingControl = false;
            collision.gameObject.GetComponent<ChoppingScript>().PickUpChoppedFood(this.gameObject);
        }

        if(collision.gameObject.tag == "TrashCan" 
            && pressingShift 
            && Inventory.Count > 0)
        {
            pressingShift = false;
            Destroy(Inventory[0].gameObject);
            Inventory.Remove(Inventory[0]);
            if (Inventory.Count > 0)
            {
                Inventory[0].transform.parent = gameObject.transform.GetChild(Inventory.Count - 1).gameObject.transform;
                return;
            }
        }

        if (collision.gameObject.tag == "Plate" && pressingShift 
            && Inventory[0].GetComponent<FoodScript>().prepared == false
            && collision.gameObject.GetComponent<PlateScript>().isItemOnPlate == false)
        {
            pressingShift = false;
            collision.gameObject.GetComponent<PlateScript>().AddToPlate(Inventory[0]);
            Inventory.Remove(Inventory[0]);
            if (Inventory.Count > 0)
            {
                Inventory[0].transform.parent = gameObject.transform.GetChild(Inventory.Count - 1).gameObject.transform;
                return;
            }
        }
        if (collision.gameObject.tag == "Plate" && pressingShift
            && Inventory.Count < 2
            && collision.gameObject.GetComponent<PlateScript>().isItemOnPlate == true)
        {
            pressingShift = false;
            collision.gameObject.GetComponent<PlateScript>().RemoveFromPlate(this.gameObject);
        }
    }

    private void InstantiateFoodItem(string FoodToSpawn)
    {
        for (int i = 0; i < FoodItems.Count; i++)
        {
            if (FoodItems[i].name == FoodToSpawn)
            {
                var myItemToInstantiate = Instantiate(FoodItems[i].gameObject, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                myItemToInstantiate.transform.parent = gameObject.transform.GetChild(Inventory.Count).gameObject.transform;
                myItemToInstantiate.transform.position = new Vector3(0, 0, 0);
                myItemToInstantiate.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
                Inventory.Add(myItemToInstantiate);
                break;
            }
        }
    }
}


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

    Rigidbody2D rigidBody;
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    //USE LEFT AND RIGHT CTRL FOR CHOPPING, RIGHT AND LEFT SHIFT FOR PICKING UP
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift) && isPlayer1)
        {
            pressingShift = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            pressingShift = false;
        }
        if (Input.GetKeyDown(KeyCode.RightShift) && !isPlayer1)
        {
            pressingShift = true;
        }
        if (Input.GetKeyUp(KeyCode.RightShift) && !isPlayer1)
        {
            pressingShift = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && isPlayer1)
        {
            pressingControl = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            pressingControl = false;
        }
        if (Input.GetKeyDown(KeyCode.RightControl) && !isPlayer1)
        {
            pressingControl = true;
        }
        if (Input.GetKeyUp(KeyCode.RightControl) && !isPlayer1)
        {
            pressingControl = false;
        }
    }

    void FixedUpdate()
    {
        Vector3 input = Vector3.zero;
        if (isPlayer1)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");
        } else
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
            if (collision.gameObject.tag == "FoodItem" && pressingShift)
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
            if (collision.gameObject.tag == "CuttingBoard" && pressingShift
                && collision.gameObject.GetComponent<ChoppingScript>().readyToChop == false)
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
        if (collision.gameObject.tag == "CuttingBoard" && pressingControl
            && collision.gameObject.GetComponent<ChoppingScript>().readyToChop == true)
        {
            pressingControl = false;
            collision.gameObject.GetComponent<ChoppingScript>().StartChopping();
        }
    }

    private void InstantiateFoodItem(string FoodToSpawn)
    {
        for (int i = 0; i < FoodItems.Count; i++)
        {
            if (FoodItems[i].name == FoodToSpawn)
            {
                var myItemToInstantiate = Instantiate(FoodItems[i].gameObject, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity); ;
                myItemToInstantiate.transform.parent = gameObject.transform.GetChild(Inventory.Count).gameObject.transform;
                myItemToInstantiate.transform.position = new Vector3(0, 0, 0);
                myItemToInstantiate.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
                Inventory.Add(myItemToInstantiate);
                break;
            }
        }
    }
}


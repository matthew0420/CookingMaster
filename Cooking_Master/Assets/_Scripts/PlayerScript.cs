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

    Rigidbody2D rigidBody;
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
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
            if (collision.gameObject.tag == "FoodItem" && Input.GetKeyDown(KeyCode.LeftShift) && isPlayer1)
            {
                Debug.Log("pressing!");
                InstantiateFoodItem(collision.gameObject.name);
            }
            else if (collision.gameObject.tag == "FoodItem" && Input.GetKeyDown(KeyCode.RightShift) && !isPlayer1)
            {
                InstantiateFoodItem(collision.gameObject.name);
            }
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


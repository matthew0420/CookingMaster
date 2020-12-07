using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerScript : MonoBehaviour
{
    public CustomerSpawner customerSpawner;

    //the objects of the prepared ingredients
    public List<GameObject> PreparedIngredients;

    //has the same items as Prepared Ingredients but each time one 
    //is chosen randomly that list item will be removed
    public List<GameObject> ChoosingIngredients;

    //the items the customer wants for their order
    public List<GameObject> DesiredIngredients;
    public List<GameObject> SaveDesiredIngredients;
    //a list of the ingredients delivered to the customer
    public List<GameObject> DeliveredIngredients;

    //Holds the spots where ingredients to server the customer will appear
    public List<GameObject> IngredientHUDSpots;

    public float maxWaitTime;
    public float currentWaitTime;

    public Slider waitTimeSlider;

    private bool customerAngry;
    //Players who deliver the wrong order to the customer will be added to this list
    //this list is to be cleared if the order is completed
    public List<GameObject> AngryAt;

    private GameObject currentPlayer;

    public ScoreScript scoreScript;

    public bool outOfTimeBool;

    void Start()
    {
        scoreScript = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreScript>();
        waitTimeSlider = gameObject.GetComponentInChildren<Slider>();
        //number of ingredients desired by the customer
        var numberOfIngredients = Random.Range(2, 5);

        for (int i = 0; i < numberOfIngredients; i++)
        {
            var numberChosen = ChoosingIngredients[Random.Range(0, ChoosingIngredients.Count)];
            DesiredIngredients.Add(numberChosen);
            ChoosingIngredients.Remove(numberChosen);

            var IngredientHUD = Instantiate(DesiredIngredients[i].gameObject, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            IngredientHUD.transform.parent = IngredientHUDSpots[i].transform;
            IngredientHUD.transform.localPosition = new Vector3(0, 0.22f, 0);
            IngredientHUD.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 150);
        }

        foreach (GameObject ingredient in DesiredIngredients)
        {
            SaveDesiredIngredients.Add(ingredient);
        }

        switch (DesiredIngredients.Count)
        {
            case 1:
                waitTimeSlider.maxValue = 40;
                break;
            case 2:
                waitTimeSlider.maxValue = 60;
                break;
            case 3:
                waitTimeSlider.maxValue = 80;
                break;
            case 4:
                waitTimeSlider.maxValue = 100;
                break;
            default:
                Debug.Log("something went wrong with generating the max wait time");
                break;
        }

        maxWaitTime = waitTimeSlider.maxValue;
        waitTimeSlider.value = maxWaitTime;
        currentWaitTime = maxWaitTime;
    }

    private void FixedUpdate()
    {
        if (currentWaitTime > 0)
        {
            if (!customerAngry)
            {
                currentWaitTime -= Time.deltaTime;
            }
            else
            {
                currentWaitTime -= Time.deltaTime + Time.deltaTime * 0.5f;
            }
            waitTimeSlider.value = currentWaitTime;
        }
        else
        {
            if (!outOfTimeBool)
            {
                outOfTimeBool = true;
                OutOfTime();
                //leave angry, remove score
            }
        }
    }

    public void RecieveOrder(GameObject order, GameObject player)
    {
        currentPlayer = player;
        order.GetComponent<FoodScript>().isServed = true;
        order.transform.SetParent(this.gameObject.transform);
        order.gameObject.transform.localPosition = new Vector3(0, this.gameObject.transform.localPosition.y - 0.4f, 0);

        //we use DesiredIngredients here and check the listItem against the items in DesiredIngredients so that we can remove them from the list to prevent
        //submitting the same ingredient multiple times, while still accurately gauging if all the ingredients were delivered
        foreach (Transform child in order.transform)
        {
            foreach (GameObject listItem in DesiredIngredients)
            {
                if (listItem.name == child.gameObject.name && SaveDesiredIngredients.Contains(listItem))
                {
                    DeliveredIngredients.Add(child.gameObject);
                    SaveDesiredIngredients.Remove(listItem);
                }
            }
        }

        if (DeliveredIngredients.Count != DesiredIngredients.Count)
        {
            //the order must be wrong, act angry!
            if (!AngryAt.Contains(player))
            {
                AngryAt.Add(player);
            }
            IncorrectOrder();
        }
        if (DeliveredIngredients.Count == DesiredIngredients.Count)
        {
            //This order is correct!
            CorrectOrder();

            //if delivery is before 70% of waiting time is up, generate bonus for player
            if(currentWaitTime >= (maxWaitTime * 0.70f))
            {
                scoreScript.GenerateBonus(player);
            }
        }
    }

    private void CorrectOrder()
    {
        scoreScript.AddScore(currentPlayer, DesiredIngredients.Count);
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color32(97, 154, 60, 255);
        Invoke("CustomerLeave", 0.2f);
    }

    private void IncorrectOrder()
    {
        //remake the SaveDesiredIngredients list to be checked again
        SaveDesiredIngredients.Clear();
        foreach (GameObject ingredient in DesiredIngredients)
        {
            SaveDesiredIngredients.Add(ingredient);
        }
        customerAngry = true;
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color32(120, 72, 72, 255);
        //Destroys the last child in the customer object, which will always be the choppedIngredients
        Destroy(GetComponent<Transform>().GetChild(this.transform.childCount - 1).gameObject);
    }

    private void OutOfTime()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color32(120, 72, 72, 255);
        Debug.Log("Out of time!");
        if (customerAngry)
        {
            foreach(GameObject player in AngryAt)
            {
                //only the player(s) who angered the customer lose points when they leave
                scoreScript.AngryLeaveDeduction(player, DesiredIngredients.Count);
            }
            AngryAt.Clear();
            customerAngry = false;
        } else
        {
            //both players lose points
            scoreScript.LeaveDeduction(DesiredIngredients.Count);
        }
        Invoke("CustomerLeave", 0.2f);
    }

    private void CustomerLeave()
    {
        customerSpawner.CustomerSpots.Add(this.transform.parent.gameObject);
        Destroy(this.gameObject);
    }
}

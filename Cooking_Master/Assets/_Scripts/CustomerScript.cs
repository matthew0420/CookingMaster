using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerScript : MonoBehaviour
{
    public CustomerSpawner customerSpawner;
    //the objects of the prepared ingredients
    public List<GameObject> PreparedIngredients;

    //has the same items as Prepared Ingredients but each time one is chosen randomly
    //that list item will be removed
    public List<GameObject> ChoosingIngredients;

    //the items the customer wants for their order
    public List<GameObject> DesiredIngredients;

    void Start()
    {
        //number of ingredients desired by the customer (5 - 1)
        var numberOfIngredients = Random.Range(2, 5);
            //PreparedIngredients.Count);
    
        for(int i = 0; i < numberOfIngredients; i++)
        {
            var numberChosen = ChoosingIngredients[Random.Range(1, ChoosingIngredients.Count)];
            DesiredIngredients.Add(numberChosen);
            ChoosingIngredients.Remove(numberChosen);
        }
    }

    private void FixedUpdate()
    {
        //timer for patience bar will go here
    }

    public void RecieveOrder(GameObject order)
    {

    }

    //testing function******************
    public void Die()
    {
        customerSpawner.CustomerSpots.Add(this.transform.parent.gameObject);
        Destroy(this.gameObject);
    }
    //**********************************
}

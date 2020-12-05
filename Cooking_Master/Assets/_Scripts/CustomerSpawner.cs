using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{

    public GameObject CustomerPrefab;

    public float TimeUntilSpawn;
    public float CurrentTime;

    //customers will be spawned in to an empty CustomerSpot, and then added as a child of the spot
    //when spawning new customers, we check each customerspot to ensure that it does not have a child
    public List<GameObject> CustomerSpots;
    public List<GameObject> CustomerSpotObjects;

    private void FixedUpdate()
    {
        CurrentTime += Time.deltaTime;

        if (CurrentTime >= TimeUntilSpawn)
        {
            CurrentTime = 0f;
            SpawnCustomer();
        }
    }

    private void SpawnCustomer()
    {
        for (int i = 0; i < CustomerSpots.Count; i++)
        {
            if (CustomerSpots[i].gameObject.transform.childCount > 0)
            {
                CustomerSpots.Remove(CustomerSpots[i]);
            }
        }
        
        //if CustomerSpots returns empty, then the spots are all full
        if (CustomerSpots.Count == 0)
        {
            Debug.Log("No space!");
            return;
        }

        var spotToSpawn = Random.Range(0, CustomerSpots.Count);

        var newCustomer = Instantiate(CustomerPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        newCustomer.transform.parent = CustomerSpots[spotToSpawn].transform;
        newCustomer.transform.localPosition = new Vector3(0, 0.26f, 0);
        newCustomer.GetComponent<CustomerScript>().customerSpawner = this.gameObject.GetComponent<CustomerSpawner>();
        return;
    }
}

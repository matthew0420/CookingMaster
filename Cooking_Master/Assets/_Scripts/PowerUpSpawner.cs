using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject playArea;
    public GameObject PowerUpObject;

    private bool powerUpSpawned;

    public void SpawnPower(GameObject player)
    {
        powerUpSpawned = false;
        var canSpawn = true;
        while (!powerUpSpawned)
        {
            Vector3 powerUpPosition = new Vector3(Random.Range(-10f, 10f), Random.Range(-3.5f, 3.5f), 0f);
            var players = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject playerObject in players)
            {
                if ((powerUpPosition - playerObject.transform.position).magnitude < 5)
                {
                    canSpawn = false;
                }
            }
            if (!canSpawn)
            {
                //returns to beggining of while loop
                continue;
            }
            else
            {
                var powerUp = Instantiate(PowerUpObject, powerUpPosition, Quaternion.identity);
                powerUp.GetComponent<PowerUpScript>().player = player;

                if (player.GetComponent<PlayerScript>().isPlayer1)
                {
                    powerUp.GetComponent<SpriteRenderer>().color = new Color32(52, 67, 255, 255);
                } else
                {
                    powerUp.GetComponent<SpriteRenderer>().color = new Color32(231, 27, 37, 255);
                }
                powerUpSpawned = true;
            }
        }
    }
}

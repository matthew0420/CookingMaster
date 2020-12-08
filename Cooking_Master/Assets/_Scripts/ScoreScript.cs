using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    public PowerUpSpawner powerUpSpawner;

    public GameObject player1;
    public GameObject player2;

    public bool player1GameOver;
    public bool player2GameOver;
    public bool gameOver;

    public Text Timer1;
    public float Timer1Amount = 120f;
    public Text Timer2;
    public float Timer2Amount = 120f;


    public Text Score1;
    public int Score1Amount;
    public Text Score2;
    public int Score2Amount;

    public GameObject gameOverMenu;

    void FixedUpdate()
    {
        if(player1GameOver && player2GameOver)
        {
            gameOverMenu.SetActive(true);
            gameOverMenu.GetComponent<GameOverScript>().GameOver();
            Time.timeScale = 0f;
        }

        if (!player1GameOver)
        {
            if (Timer1Amount > 0)
            {
                Timer1Amount -= Time.deltaTime;
                Timer1.text = "Time: " + Mathf.RoundToInt(Timer1Amount);
            }
            else
            {
                Timer1.text = "Time: " + 0;
                player1.GetComponent<PlayerScript>().canMove = false;
                player1.transform.position = new Vector3(500, 500, 0);
                player1GameOver = true;
            }
        }

        if (!player2GameOver)
        {
            if (Timer2Amount > 0)
            {
                Timer2Amount -= Time.deltaTime;
                Timer2.text = "Time: " + Mathf.RoundToInt(Timer2Amount);
            }
            else
            {
                Timer2.text = "Time: " + 0;
                player2.GetComponent<PlayerScript>().canMove = false;
                player2.transform.position = new Vector3(500, 500, 0);
                player2GameOver = true;
            }
        }
    }

    public void AddScore(GameObject player, int scoreToAdd)
    {
        if (player.gameObject == player1.gameObject)
        {
            Score1Amount += scoreToAdd;
            Score1.text = "Score: " + Score1Amount;
        }
        else
        {
            Score2Amount += scoreToAdd;
            Score2.text = "Score: " + Score2Amount;
        }
    }

    public void GenerateBonus(GameObject player)
    {
        powerUpSpawner.SpawnPower(player);
    }

    public void AngryLeaveDeduction(GameObject player, int scoreToRemove)
    {
        if (player.gameObject == player1.gameObject)
        {
            Score1Amount -= scoreToRemove;
            Score1.text = "Score: " + Score1Amount;
        }
        else
        {
            Score2Amount -= scoreToRemove;
            Score2.text = "Score: " + Score2Amount;
        }
    }

    public void LeaveDeduction(int scoreToRemove)
    {
        Score1Amount -= scoreToRemove;
        Score1.text = "Score: " + Score1Amount;

        Score2Amount -= scoreToRemove;
        Score2.text = "Score: " + Score2Amount;
    }
}

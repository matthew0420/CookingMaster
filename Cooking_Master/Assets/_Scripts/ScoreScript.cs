using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;

    public Text Timer1;
    private float Timer1Amount = 120f;
    public Text Timer2;
    private float Timer2Amount = 120f;


    public Text Score1;
    private int Score1Amount;
    public Text Score2;
    private int Score2Amount;

    void Update()
    {
        Timer1Amount -= Time.deltaTime;
        Timer1.text = "Time: " + Mathf.RoundToInt(Timer1Amount);

        Timer2Amount -= Time.deltaTime;
        Timer2.text = "Time: " + Mathf.RoundToInt(Timer2Amount);
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

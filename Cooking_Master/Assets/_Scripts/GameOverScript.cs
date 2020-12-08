using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour
{
    public Text winText;
    public Text score1;
    public Text score2;

    public ScoreScript scoreScript;

    public string[] HighScores;

    public string HighestScore;

    public List<GameObject> HighScoreSpaces;

    public void GameOver()
    {
        //HighScores = new string[10];
        if (PlayerPrefs.GetInt("FirstGameOver") == 0)
        {
            for (int i = 0; i < 10; ++i)
            {
                PlayerPrefs.SetString("HighScore" + i, "0");
            }
            PlayerPrefs.SetInt("FirstGameOver", 1);
        }

        for (int i = 0; i < 10; ++i)
        {
            HighScores[i] = "0";
        }

        if (scoreScript.Score1Amount > scoreScript.Score2Amount)
        {
            winText.text = "Player 1 wins!";
            HighestScore = scoreScript.Score1Amount.ToString();
        }

        if (scoreScript.Score2Amount > scoreScript.Score1Amount)
        {
            winText.text = "Player 2 wins!";
            HighestScore = scoreScript.Score2Amount.ToString();
        }

        if (scoreScript.Score1Amount == scoreScript.Score2Amount)
        {
            winText.text = "It's a tie!";
            HighestScore = scoreScript.Score1Amount.ToString();
        }

        score1.text = "Player 1 score: " + scoreScript.Score1Amount;
        score2.text = "Player 2 score: " + scoreScript.Score2Amount;

        for (int i = 1; i < HighScores.Length; ++i)
        {
            if (Convert.ToInt32(HighestScore) > Convert.ToInt32(PlayerPrefs.GetString("HighScore" + i)))
            {
                PlayerPrefs.SetString("HighScore" + i, HighestScore.ToString());
                break;
            }
        }

        for (int i = HighScores.Length; i > 0; --i)
        {
            HighScores[i - 1] = PlayerPrefs.GetString("HighScore" + i);
        }

        SortScores();

        for (int i = 0; i < HighScoreSpaces.Count; i++)
        {
            HighScoreSpaces[i].GetComponent<Text>().text = $"{i + 1}: " + HighScores[i].ToString();
        }
        HighScoreSpaces[0].GetComponent<Text>().text = "1: " + HighScores[0].ToString();

        for (int i = 0; i < HighScoreSpaces.Count; i++)
        {
            PlayerPrefs.SetString("HighScore" + i, HighScores[i]);
        }

    }

    void SortScores()
    {
        bool swapped;
        for (int i = HighScores.Length - 1; i > 0; --i)
        {
            swapped = false;
            for (int j = 0; j < i; ++j)
            {
                if (Convert.ToInt32(HighScores[j]) < Convert.ToInt32(HighScores[j + 1]))
                {
                    Swap<string>(ref HighScores[j], ref HighScores[j + 1]);
                    swapped = true;
                }
            }
            if (!swapped)
            {
                break;
            }
        }
    }

    public static void Swap<T>(ref T lhs, ref T rhs)
    {
        T temp = lhs;
        lhs = rhs;
        rhs = temp;
    }
    //update does not run when time is frozen
}

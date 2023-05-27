using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

//https://www.youtube.com/watch?v=K4uOjb5p3Io&ab_channel=CocoCode

public class GameOver : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    public void Setup(int score)
    {
        gameObject.SetActive(true);
        scoreText.text = score.ToString() + " POINTS";
    }

    public void NewGameButton()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ExitButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

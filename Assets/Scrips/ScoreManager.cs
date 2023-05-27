using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// https://learn.unity.com/tutorial/lesson-5-2-keeping-score#5ce6151aedbc2a0076e7401a
public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public AudioSource scoreAdded;

    public int score;

    void Start()
    {
        score = 0;
        UpdateScore(0);
    }

    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "" + score;

        StartCoroutine(AnimateFontSize(scoreText.fontSize + 10));

        scoreAdded.Play();
    }

    private IEnumerator AnimateFontSize(float targetSize)
    {
        float startSize = 36;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / 0.25f;
            scoreText.fontSize = Mathf.Lerp(startSize, targetSize, t);
            yield return null;
        }

        scoreText.fontSize = startSize;
    }
}

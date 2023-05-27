using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// https://docs.unity3d.com/ScriptReference/Cursor-lockState.html

public class PlayerHealth : MonoBehaviour
{

    // Damage Indicator
    [Header("Damage Indicator")]
    public Image damageImage;
    public float fadeDuration = 0.5f;
    public float fadeDelay = 0.2f;

    [Header("Audio")]
    public AudioSource hurtSound;
    public AudioSource walkingSound;
    public AudioSource sprintSound;

    [Header("Health")]
    public float maxHealth = 100f;
    public float currentHealth;

    public TextMeshProUGUI healthText;

    [Header("Death")]
    public GameOver GameOver;
    public ScoreManager ScoreManager;
    private StarterAssets.FirstPersonController FPSControllerScript;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        damageImage.canvasRenderer.SetAlpha(0.0f);
        FPSControllerScript = GetComponent<StarterAssets.FirstPersonController>();

        currentHealth = maxHealth;
        healthText.text = "" + currentHealth;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                walkingSound.enabled = false;
                sprintSound.enabled = true;
            }
            else
            {
                walkingSound.enabled = true;
                sprintSound.enabled = false;
            }
        }

        else
        {
            walkingSound.enabled = false;
            sprintSound.enabled = false;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        healthText.text = "" + currentHealth;

        StartCoroutine(DamageScreen());
        hurtSound.Play();

        // Turns "off" the game when player died
        if (currentHealth <= 0f)
        {
            Time.timeScale = 0;
            FPSControllerScript.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            GameOver.Setup(ScoreManager.score);
        }
    }

    // Heals the Player back to full health
    public void healPlayer()
    {
        Debug.Log("Player Healed");
        currentHealth = maxHealth;
        healthText.text = "" + currentHealth;
    }

    // Increases the player maxhealth
    public void increaseMaxHealth(float newMaxHealth)
    {
        Debug.Log("Player Health Increased");
        maxHealth = newMaxHealth;
        currentHealth = maxHealth;
        healthText.text = "" + currentHealth;
    }

    IEnumerator DamageScreen()
    {
        // Fade in
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / fadeDuration)
        {
            damageImage.canvasRenderer.SetAlpha(t);
            yield return null;
        }

        // Fade out
        for (float t = 1.0f; t > 0.0f; t -= Time.deltaTime / fadeDuration)
        {
            damageImage.canvasRenderer.SetAlpha(t);
            yield return null;
        }

        damageImage.canvasRenderer.SetAlpha(0.0f);
    }
}

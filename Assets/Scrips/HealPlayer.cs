using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HealPlayer : MonoBehaviour
{

    public GameObject healthObject;
    public float maxHealth;

    public float distance = 0.3f;
    public float duration = 1f;

    private void Start()
    {
        StartCoroutine(MoveUpDown());
        StartCoroutine(Spin());
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            PlayerHealth playerHealth = col.GetComponent<PlayerHealth>();

            if (healthObject.CompareTag("ResetMaxHealth"))
            {
                if (playerHealth.currentHealth < playerHealth.maxHealth)
                {
                    playerHealth.healPlayer();
                    Destroy(gameObject);
                }
                return;
            }

            if (healthObject.CompareTag("IncreaseMaxHealth"))
            {
                playerHealth.increaseMaxHealth(maxHealth);
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator MoveUpDown()
    {
        while (true)
        {
            // Move up
            yield return transform.DOMoveY(transform.position.y + distance, duration).WaitForCompletion();

            // Move down
            yield return transform.DOMoveY(transform.position.y - distance, duration).WaitForCompletion();
        }
    }
    private IEnumerator Spin()
    {
        while (true)
        {
            // Rotate around Y axis
            yield return transform.DORotate(new Vector3(0f, 360f, 0f), duration, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .WaitForCompletion();
        }
    }


}

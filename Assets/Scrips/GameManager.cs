using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public StarterAssets.FirstPersonController FPSControllerScript;

    void Start()
    {
        FPSControllerScript = GetComponent<StarterAssets.FirstPersonController>();

        Time.timeScale = 1.0f;
        // FPSControllerScript.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // General Lighting
    // https://vintay.medium.com/creating-light-source-fog-in-unity-hdrp-6510cda9b387
}

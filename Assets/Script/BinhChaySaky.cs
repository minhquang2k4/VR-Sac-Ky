using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BinhChaySacky : MonoBehaviour
{

    [SerializeField] MeshRenderer rend;
    [SerializeField] Canvas countdownCanvas;
    [SerializeField] Image countdownImage;
    [SerializeField] Sprite[] countdownSprites;

    [Header("Testing")]
    [SerializeField] private KeyCode testKey = KeyCode.T;

    private bool isCountdownActive = false;


    public void Start()
    {
        if (countdownCanvas != null)
        {
            countdownCanvas.enabled = false;
        }
    }

    // Test
    void Update()
    {
        if (Input.GetKeyDown(testKey))
        {
            Debug.Log("[BinhChaySacky] Test key pressed - starting countdown");
            ActiveLiquid();
        }
    }

    void LateUpdate()
    {
        if (countdownCanvas != null && countdownCanvas.enabled && Camera.main != null)
        {
            countdownCanvas.transform.LookAt(Camera.main.transform);
            countdownCanvas.transform.Rotate(0, 180f, 0);
        }

    }

    public void ActiveLiquid()
    {
        if (!isCountdownActive)
        {
            StartCoroutine(CountdownAndActivate());
        }
    }

    private IEnumerator CountdownAndActivate()
    {
        isCountdownActive = true;
        rend.enabled = true;

        if (countdownCanvas != null)
        {
            countdownCanvas.enabled = true;
        }

        float countdown = 10f;

        while (countdown > 0)
        {
            int currentNumber = Mathf.CeilToInt(countdown);
            if (countdownImage != null && countdownSprites != null && countdownSprites.Length >= currentNumber)
            {
                countdownImage.sprite = countdownSprites[10 - currentNumber];
            }

            yield return null;
            countdown -= Time.deltaTime;
        }

        if (countdownCanvas != null)
        {
            countdownCanvas.enabled = false;
        }

        isCountdownActive = false;
    }
}
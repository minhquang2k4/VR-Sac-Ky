using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaySacKy : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private MeshRenderer left;
    [SerializeField] private MeshRenderer right;
    [SerializeField] private Canvas canvasEnd;
    
    private float alpha = 0.05f;
    private float alphaUV = 0.3f;
    private bool  check = false;
    
    void OnTriggerEnter(Collider other) {
        if( other.gameObject.CompareTag("BinhSacKy"))
        {
            check = true;
            StartCoroutine(SacKy());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (check && other.gameObject.CompareTag("UV"))
        {
            canvasGroup.alpha = alphaUV;
            canvasEnd.enabled = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (check && other.gameObject.CompareTag("UV"))
        {
            canvasGroup.alpha = alpha;
        }
    }

    IEnumerator SacKy()
    {
        yield return new WaitForSeconds(20);
        left.enabled = false;
        right.enabled = false;
        while (canvasGroup.alpha < alpha)
        {
            canvasGroup.alpha += Time.deltaTime / 10;
            yield return null;
        }
    }
}

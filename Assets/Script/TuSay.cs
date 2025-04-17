using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuSay : MonoBehaviour
{ 
    [SerializeField] private Canvas canvas;

    private void Start()
    {
        canvas.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BanMong"))
        {
            canvas.enabled = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BanMong"))
        {
            canvas.enabled = false;
        }
    }
}

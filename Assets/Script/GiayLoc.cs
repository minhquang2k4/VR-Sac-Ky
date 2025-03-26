using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiayLoc : MonoBehaviour
{
    [SerializeField] private GameObject bot;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PositionOngNghiem"))
        {
            bot.SetActive(true);
        }
    }
}

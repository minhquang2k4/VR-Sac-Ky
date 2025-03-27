using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaoDan : MonoBehaviour
{
    
    [SerializeField] private GameObject dungdich;
    private MeshRenderer _rend;
    private void Start()
    {
        _rend = dungdich.GetComponent<MeshRenderer>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CocVang"))
        {
            _rend.enabled = true;

        }
    }
}

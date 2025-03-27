using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NhoGiot : MonoBehaviour
{
    MeshRenderer _rend;
    void Awake()
    {
        _rend = GetComponent<MeshRenderer>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("MaoDan"))
        {
            if (other.transform.GetChild(0).GetComponent<MeshRenderer>().enabled)
            {
                _rend.enabled = true;
                other.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }

}

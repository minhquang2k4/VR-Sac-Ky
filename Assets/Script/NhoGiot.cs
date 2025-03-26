using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NhoGiot : MonoBehaviour
{
    // Start is called before the first frame update
    MeshRenderer _rend;
    public MaoDan MaoDan;
    private bool _isEmpty;

    private void Update()
    {
        _isEmpty = MaoDan.isEmpty;
    }

    void Awake()
    {
        _rend = GetComponent<MeshRenderer>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!_isEmpty && other.gameObject.CompareTag("MaoDan"))
        {
            if (_rend != null)
            {
                Debug.Log("rend not Null");
                _rend.enabled = true;
            }
            else
            {
                Debug.Log("rend is Null");
            }
        }
        else
        {
            Debug.Log("IsEmpty is true");
        }
    }

}

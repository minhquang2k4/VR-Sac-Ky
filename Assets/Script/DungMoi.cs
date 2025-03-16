using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DungMoi : MonoBehaviour
{
    public bool _isEmpty;
    MeshRenderer rend;
    void Awake()
    {
        rend = this.transform.GetChild(2).GetComponent<MeshRenderer>();
    }

    void Update()
    {
        _isEmpty = !rend.enabled;
    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("BinhSacKy"))
        {
            if (!_isEmpty)
            {
                MeshRenderer otherRend = other.GetComponent<MeshRenderer>();
                otherRend.enabled = true;
                rend.enabled = false;
            }
        }
    }
}

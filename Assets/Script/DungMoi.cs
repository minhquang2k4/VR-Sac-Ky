using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DungMoi : MonoBehaviour
{
    public bool _isEmpty;
    [SerializeField ]MeshRenderer rend;

    void Update()
    {
        _isEmpty = !rend.enabled;
    }

    public void ActiveLiquid()
    {
        rend.enabled = true;    
    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PositionBinhChaySacKy"))
        {
            if (!_isEmpty)
            {
               other.gameObject.GetComponent<BinhChaySacky>().ActiveLiquid();
                rend.enabled = false;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pheu : MonoBehaviour
{
    Vector3 _position;
    public bool check = true;
    public void OnTriggerStay(Collider other)
    {

        if (check && other.gameObject.CompareTag("PositionOngNghiem"))
        {
            this.transform.SetParent(other.transform);
            check = false;
        }

    }
    public void OnTriggerExit(Collider other)
    {
        this.transform.SetParent(null);
        check = true;
    }
}
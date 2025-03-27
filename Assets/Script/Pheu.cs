using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pheu : MonoBehaviour
{
    // Vector3 _position;
    public bool check = true;
    [SerializeField] Transform positionGiayLoc;
    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("PositionOngNghiem"))
        {
            // if (check && (this.transform.position == other.transform.GetChild(0).position))
            // {
            //     check = false;
            // }
            if (this.transform.parent == null || this.transform.parent == other.transform)
            {
                this.transform.position = other.transform.GetChild(0).position;
                this.transform.rotation = other.transform.GetChild(0).rotation;
                this.transform.SetParent(other.transform);
            }
        }
        if (other.gameObject.CompareTag("GiayLoc"))
        {
            other.transform.position = positionGiayLoc.position;
            other.transform.rotation = positionGiayLoc.rotation;
            other.transform.SetParent(this.transform);
        }

    }
    public void OnTriggerExit(Collider other)
    {
        if ( other.gameObject.CompareTag("PositionOngNghiem"))
        {
            this.transform.SetParent(null);
            // check = true;
        }
        if (other.gameObject.CompareTag("GiayLoc"))
        {
            other.transform.SetParent(null);
        }
    }
}
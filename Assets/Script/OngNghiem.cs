using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OngNghiem : MonoBehaviour
{
    
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("GiaDo"))
        {
            this.transform.position = other.transform.position;
            this.transform.rotation = other.transform.rotation;
        }
    }
}

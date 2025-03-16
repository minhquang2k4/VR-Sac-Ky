using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OngNghiem : MonoBehaviour
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
        if (other.CompareTag("GiaDo"))
        {
            this.transform.position = other.transform.position;
            this.transform.rotation = other.transform.rotation;
        }
        if (other.CompareTag("Pheu"))
        {
            if (this.transform == other.transform.parent)
            {
                return;
            }
            else
            {
                if (!_isEmpty)
                {
                    Transform target = other.transform.parent.GetChild(2);
                    target.gameObject.GetComponent<MeshRenderer>().enabled = true;
                    target.localScale = new Vector3(0.90793f, 0.3059987f, 1f);
                    rend.enabled = false;
                }
            }
        }
        if (other.CompareTag("CocVang"))
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NhoGiot : MonoBehaviour
{
    // Start is called before the first frame update
    MeshRenderer rend;

    void Awake()
    {
        rend = GetComponent<MeshRenderer>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("OngHut"))
        {
            if (rend != null)
            {
                rend.enabled = true;
                Debug.Log("Nho giot ra khoi ong hut");
            }
            else
            {
                Debug.LogError("MeshRenderer chưa được khởi tạo.");
            }
        }
    }

}

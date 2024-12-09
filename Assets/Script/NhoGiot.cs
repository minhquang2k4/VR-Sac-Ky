using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NhoGiot : MonoBehaviour
{
    // Start is called before the first frame update
    MeshRenderer rend;
    public OngNhoGiot ongNhoGiot;
    bool isEmpty;
    bool check;

    void Awake()
    {
        rend = GetComponent<MeshRenderer>();
    }
    void Update()
    {
        isEmpty = ongNhoGiot.IsEmpty;
        check = ongNhoGiot.Check;
    }
    void OnTriggerEnter(Collider other)
    {
        if (!isEmpty && other.gameObject.CompareTag("OngNhoGiot"))
        {
            if (rend != null)
            {
                Debug.Log("rend not Null");
                rend.enabled = true;
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

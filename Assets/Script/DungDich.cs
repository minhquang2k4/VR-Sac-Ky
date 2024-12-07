using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungDich : MonoBehaviour
{
    public OngNhoGiot _ongNhoGiot;
    MeshRenderer rend;
    int cnt = 0;
    float scale = 0.1f;
    bool check;

    void Awake()
    {
        rend = GetComponent<MeshRenderer>();
    }
    void Update()
    {
        check = _ongNhoGiot.IsEmpty;
    
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("OngNhoGiot"))
        {
            if (check == false)
            {
                rend.enabled = true;
                cnt++;
                if (cnt <= 5)
                {
                    this.transform.localScale = new Vector3(1, this.transform.localScale.y + scale, 1);
                }
            }
            else
            {
                Debug.LogWarning("OngNhoGiot component không tồn tại hoặc _isEmpty là false.");
            }
        }
    }
}
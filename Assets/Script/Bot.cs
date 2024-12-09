using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour
{
    public Thia _thia;
    MeshRenderer rend;
    int cnt = 0;
    float scale = 0.01f;
    bool check;

    void Awake()
    {
        rend = GetComponent<MeshRenderer>();
    }
    void Update()
    {
        check = _thia.IsEmpty;
    
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Thia"))
        {
            if (check == false)
            {
                rend.enabled = true;
                cnt++;
                if (cnt <= 5)
                {
                    this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y + scale, this.transform.localScale.z);
                }
            }
            else
            {
                Debug.LogWarning("thia component không tồn tại hoặc _isEmpty là false.");
            }
        }
    }
}
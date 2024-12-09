using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungDich : MonoBehaviour
{
    public OngNhoGiot _ongNhoGiot;
    public Thia _thia;
    MeshRenderer rend;
    int cnt = 0;
    float scale = 0.1f;
    bool checkOng;
    bool checkThia;

    bool checkCocVang;
    void Awake()
    {
        rend = GetComponent<MeshRenderer>();
    }
    void Update()
    {
        checkCocVang = _ongNhoGiot.Check;
        checkOng = _ongNhoGiot.IsEmpty;
        checkThia = _thia.IsEmpty;

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("OngNhoGiot") || other.gameObject.CompareTag("Thia"))
        {
            if (checkCocVang == false)
            {
                if (checkOng == false || checkThia == false)
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

}
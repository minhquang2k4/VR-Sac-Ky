using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OngNhoGiot : MonoBehaviour
{
    [SerializeField] GameObject xanh;
    [SerializeField] GameObject vang;
    public bool _isEmpty = true;
    public bool check;
    public bool Check
    {
        get { return check; }
    }
    public bool IsEmpty
    {
        get { return _isEmpty; }
    }


    public void OnTriggerEnter(Collider other)
    {
        if (_isEmpty)
        {
            if (other.gameObject.CompareTag("Xanh"))
            {
                check = false;
                xanh.SetActive(true);
                _isEmpty = false;
            }
            if (other.gameObject.CompareTag("CocVang"))
            {
                check = true;
                vang.SetActive(true);
                _isEmpty = false;
            }
        }
        else
        {
            if (check == false && other.gameObject.CompareTag("CocXanh"))
            {
                xanh.SetActive(false);
                vang.SetActive(false);

            }
            if (check == false && other.gameObject.CompareTag("CocVang"))
            {
                xanh.SetActive(false);
                vang.SetActive(false);
            }
            if (check)
            {
                if (other.gameObject.CompareTag("BanMong"))
                {
                    vang.SetActive(false);
                    check = false;
                    _isEmpty = true;
                }
            }

        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (check == false)
        {
            if (other.gameObject.CompareTag("CocXanh") || other.gameObject.CompareTag("CocVang"))
            {
                _isEmpty = true;
            }
        }
        else
        {
            return;
        }

    }
}

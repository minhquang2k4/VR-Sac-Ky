using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OngHutDungMoi : MonoBehaviour
{
    [SerializeField] GameObject acid;
    [SerializeField] GameObject butanol;
    [SerializeField] GameObject water;
    
    private bool _isEmpty = true;

    public void OnTriggerEnter(Collider other)
    {
        if (_isEmpty)
        {
            if (other.gameObject.CompareTag("Acid"))
            {
                acid.SetActive(true);
                _isEmpty = false;
            }
            if (other.gameObject.CompareTag("Butanol"))
            {
                butanol.SetActive(true);
                _isEmpty = false;
            }
            if (other.gameObject.CompareTag("NuocCat"))
            {
                water.SetActive(true);
                _isEmpty = false;
            }
        } else
        {
            if (other.gameObject.CompareTag("BinhNon"))
            {
                acid.SetActive(false);
                butanol.SetActive(false);
                water.SetActive(false);
                _isEmpty = true;
            }
        }
    }
}

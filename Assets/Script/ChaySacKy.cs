using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaySacKy : MonoBehaviour
{

    [SerializeField] private Canvas _canvas;

    void OnTriggerEnter(Collider other) {
        if( other.gameObject.CompareTag("BinhSacKy"))
        {
            StartCoroutine(SacKy());
        }
    }
    IEnumerator SacKy()
    {
        yield return new WaitForSeconds(20);
        _canvas.gameObject.SetActive(true);
    }

}

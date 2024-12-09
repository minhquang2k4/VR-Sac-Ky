using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thia : MonoBehaviour
{
    [SerializeField] GameObject _bot;
    public bool _isEmpty = true;
        public bool IsEmpty
    {
        get { return _isEmpty; }
    }

    void OnTriggerEnter(Collider other)
    {
        if (_isEmpty)
        {
            if (other.gameObject.CompareTag("Bot"))
            {
                _bot.SetActive(true);
                _isEmpty = false;
            }
        }
        else
        {
            if (other.gameObject.CompareTag("CocVang"))
            {
                _bot.SetActive(false);
                _isEmpty = true;
            }
        }

    }
}

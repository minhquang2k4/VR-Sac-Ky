using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DongNap : MonoBehaviour
{
    [SerializeField] Transform _transform;
    [SerializeField] float _valueY;
    [SerializeField] string _tag;    

    Vector3 _position;

    void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.CompareTag(_tag))
        {
            _position = new Vector3(_transform.position.x, _transform.position.y - _valueY, _transform.position.z);
            this.transform.position = _position;
            this.transform.rotation = _transform.rotation;
            this.transform.SetParent(collision.transform);
        }
    }
    void OnTriggerExit(Collider collision)
    {
      
      
            this.transform.SetParent(null);

    }
}
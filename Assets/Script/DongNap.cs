using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DongNap : MonoBehaviour
{
    [SerializeField] Transform _transform;
    Vector3 _position;

    void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.CompareTag("PositionNapBinh"))
        {
            _position = new Vector3(_transform.position.x, _transform.position.y - 0.008f, _transform.position.z);
            this.transform.position = _position;
            this.transform.rotation = _transform.rotation; // Gán trực tiếp Quaternion
            Debug.Log("Da cham vao nap binh");
        }
    }
}
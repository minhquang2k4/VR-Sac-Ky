using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MaintainColliderSize : MonoBehaviour
{
    private Vector3 originalScale; // Lưu scale ban đầu của GameObject
    private Vector3 originalBoxColliderSize; // Lưu kích thước ban đầu của BoxCollider
    private Vector3 originalCapsuleColliderSize; // Lưu kích thước ban đầu của CapsuleCollider
    private Vector3 originalColliderCenter; // Lưu vị trí ban đầu của collider
    private Collider col; // Biến để lưu collider

    void Start()
    {
        // Lưu scale ban đầu của GameObject
        originalScale = transform.localScale;
        // Lấy collider từ GameObject
        col = GetComponent<Collider>();

        // Kiểm tra loại collider và lưu kích thước và vị trí ban đầu
        if (col is BoxCollider box)
        {
            originalBoxColliderSize = box.size;
            originalColliderCenter = box.center;
        }
        else if (col is CapsuleCollider capsule)
        {
            originalCapsuleColliderSize = new Vector3(capsule.radius, capsule.height, capsule.radius);
            originalColliderCenter = capsule.center;
        }
    }

    void Update()
    {
        // Tính toán scale factor dựa trên scale hiện tại và scale ban đầu
        Vector3 scaleFactor = new Vector3(
            originalScale.x / transform.localScale.x,
            originalScale.y / transform.localScale.y,
            originalScale.z / transform.localScale.z
        );

        // Điều chỉnh kích thước và vị trí của collider dựa trên scale factor
        if (col is BoxCollider box)
        {
            box.size = Vector3.Scale(originalBoxColliderSize, scaleFactor);
            box.center = Vector3.Scale(originalColliderCenter, scaleFactor);
        }
        else if (col is CapsuleCollider capsule)
        {
            capsule.radius = originalCapsuleColliderSize.x * scaleFactor.x;
            capsule.height = originalCapsuleColliderSize.y * scaleFactor.y;
            capsule.center = Vector3.Scale(originalColliderCenter, scaleFactor);
        }
    }
}
using System;
using UnityEngine;

public abstract class ThuLai : MonoBehaviour
{
    public event Action<ThuLai> LamSai;
    private Vector3 storedPos;
    private Quaternion storedRot;
    private bool isStored;

    public void MemorizePos()
    {
        storedPos = transform.position;
        storedRot = transform.rotation;
        isStored = true;
    }

    public void RevertPos()
    {
        if (isStored)
        {
            transform.SetPositionAndRotation(storedPos, storedRot);
        }
    }
    protected void RaiseFailed()
    {
        LamSai?.Invoke(this);
    }
}
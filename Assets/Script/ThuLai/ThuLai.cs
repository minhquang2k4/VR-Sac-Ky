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
    public void RaiseFailed()
    {
        LamSai?.Invoke(this);
    }

    public void ResetAllSpheres()
    {
        var spheres = GetComponentsInChildren<NhoGiot>(true);
        foreach (var sphere in spheres)
        {
            sphere.ResetSphere();
        }
    }
}
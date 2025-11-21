using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BinhChaySacky : MonoBehaviour
{

    [SerializeField ]MeshRenderer rend;
    

    public void ActiveLiquid()
    {
        rend.enabled = true;   
    }
}
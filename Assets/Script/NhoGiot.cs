using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NhoGiot : MonoBehaviour
{
    [Header("Paired Large Sphere")]
    [SerializeField] private GameObject largeSphere;

    [Header("Fail Setting")]
    [SerializeField] [Range (0f, 1f)] private float failChance = 0.2f;

    [Header("Test")]
    [SerializeField] private KeyCode testFailKey = KeyCode.F;
    [SerializeField] private KeyCode testSuccessKey = KeyCode.G;

    private MeshRenderer _rend;
    private bool hasAttempted = false;

    void Awake()
    {
        _rend = GetComponent<MeshRenderer>();

        if (largeSphere != null)
        {
            largeSphere.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(testFailKey))
        {
            Debug.Log($"[NhoGiot:{gameObject.name}] Test FAIL triggered");
            hasAttempted = false;
            SpawnLargeSphere();
            TriggerFail();
        }

        if (Input.GetKeyDown(testSuccessKey))
        {
            Debug.Log($"[NhoGiot:{gameObject.name}] Test SUCCESS triggered");
            hasAttempted = false;
            ShowNormalSphere();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("MaoDan"))
        {
            Transform childTransform = other.transform.GetChild(0);
            MeshRenderer childRend = childTransform?.GetComponent<MeshRenderer>();

            if (childRend != null && childRend.enabled && !hasAttempted)
            {
                hasAttempted = true;

                float roll = UnityEngine.Random.value;
                Debug.Log($"[NhoGiot:{gameObject.name}] Random roll: {roll}, Fail chance: {failChance}");

                if (roll < failChance)
                {
                    SpawnLargeSphere();
                    TriggerFail();
                }
                else
                {
                    _rend.enabled = true;
                    Debug.Log($"[NhoGiot:{gameObject.name}] Success: NhoGiot activated.");
                }

                childRend.enabled = false;
            }
        }
    }

    private void SpawnLargeSphere()
    {
        _rend.enabled = false;

        if (largeSphere != null)
        {
            largeSphere.SetActive(true);
            Debug.Log($"[NhoGiot:{gameObject.name}] Large sphere spawned.");
        }
        else
        {
            Debug.LogError($"[NhoGiot:{gameObject.name}] Large sphere reference is missing.");
        }
    }

    private void ShowNormalSphere()
    {
        if (largeSphere != null)
        {
            largeSphere.SetActive(false);
        }

        _rend.enabled = true;
        Debug.Log($"[NhoGiot:{gameObject.name}] Normal sphere activated.");
    }

    private void TriggerFail()
    {
        FailOverlayManager.ShowOverlay();

        var thuLai = GetComponentInParent<ThuLai>();
        if (thuLai != null)
        {
            thuLai.RaiseFailed();
            Debug.Log($"[NhoGiot:{gameObject.name}] ThuLai triggered.");
        }
        else
        {
            Debug.LogWarning($"[NhoGiot:{gameObject.name}] ThuLai component not found in parent.");
        }
    }

    public void ResetSphere()
    {
        hasAttempted = false;

        if (largeSphere != null)
        {
            largeSphere.SetActive(false);
        }

        _rend.enabled = false;

        Debug.Log($"[NhoGiot:{gameObject.name}] Sphere reset.");
    }

}

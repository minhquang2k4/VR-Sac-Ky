using System.Collections.Generic;
using UnityEngine;

public class ThuLaiUI : MonoBehaviour
{
    [SerializeField] private float distanceFromCamera = 1f;
    [SerializeField] private bool faceCamera = true;
    [SerializeField] private bool followCameraWhileVisible = false;
    [SerializeField] private Camera overrideCamera;

    [Header("Debug")]
    [SerializeField] private bool debugLogs = true;

    private ThuLai currentFailedSubject;
    private Camera cam;
    private readonly HashSet<ThuLai> subscribed = new HashSet<ThuLai>();

    private void Awake()
    {
        cam = overrideCamera != null ? overrideCamera : Camera.main;

        // IMPORTANT: Do NOT disable yourself here (was causing missed subscriptions).
        // Use Hide() AFTER subscribing (in Start) instead.
        InitialScan();
    }

    private void Start()
    {
        // Now hide UI after we know we are subscribed.
        Hide();
    }

    private void OnEnable()
    {
        // If re-enabled later, rescan.
        RescanNewSubjects();
    }

    private void OnDisable()
    {
        UnsubscribeAll();
    }

    private void LateUpdate()
    {
        if (followCameraWhileVisible && gameObject.activeSelf)
        {
            if (cam == null) cam = overrideCamera != null ? overrideCamera : Camera.main;
            if (cam == null) return;

            transform.position = cam.transform.position + cam.transform.forward * distanceFromCamera;
            if (faceCamera)
            {
                transform.rotation = Quaternion.LookRotation(cam.transform.forward, cam.transform.up);
            }
        }
    }

    private void InitialScan()
    {
        foreach (var subj in FindObjectsOfType<ThuLai>())
        {
            Subscribe(subj);
        }
    }

    public void RegisterSubject(ThuLai subj)
    {
        Subscribe(subj);
    }

    private void RescanNewSubjects()
    {
        foreach (var subj in FindObjectsOfType<ThuLai>())
        {
            Subscribe(subj);
        }
    }

    private void Subscribe(ThuLai subj)
    {
        if (subj == null || subscribed.Contains(subj)) return;
        subj.LamSai += OnSubjectFailed;
        subscribed.Add(subj);
        if (debugLogs) Debug.Log($"[ThuLaiUI] Subscribed to {subj.name}");
    }

    private void UnsubscribeAll()
    {
        foreach (var subj in subscribed)
        {
            if (subj != null)
            {
                subj.LamSai -= OnSubjectFailed;
            }
        }
        subscribed.Clear();
    }

    private void OnSubjectFailed(ThuLai subj)
    {
        currentFailedSubject = subj;
        if (debugLogs) Debug.Log($"[ThuLaiUI] Failure received from {subj.name}");
        ShowOnce();
    }

    private void ShowOnce()
    {
        if (cam == null) cam = overrideCamera != null ? overrideCamera : Camera.main;
        if (cam == null)
        {
            if (debugLogs) Debug.LogWarning("[ThuLaiUI] No camera found.");
            return;
        }

        transform.position = cam.transform.position + cam.transform.forward * distanceFromCamera;

        if (faceCamera)
        {
            transform.rotation = Quaternion.LookRotation(cam.transform.forward, cam.transform.up);
        }

        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        if (debugLogs) Debug.Log("[ThuLaiUI] Panel shown.");
    }

    public void OnTryAgain()
    {
        if (currentFailedSubject != null)
        {
            currentFailedSubject.RevertPos();
            // Optional: if specific logic needed after retry (e.g., reset flags) cast and call.
            // if (currentFailedSubject is ChaySacKy csk) csk.ResetAfterRetry();
        }
        Hide();
    }

    public void OnCancel()
    {
        Hide();
    }

    private void Hide()
    {
        // Keep object active so subscriptions persist; just move it out of sight instead of disabling.
        // If you truly want to disable: gameObject.SetActive(false); but then you must resubscribe.
        transform.position = new Vector3(9999, 9999, 9999);
        currentFailedSubject = null;
        if (debugLogs) Debug.Log("[ThuLaiUI] Panel hidden.");
    }

    // For manual debug invocation from inspector
    [ContextMenu("Force Show Test")]
    private void ForceShowTest()
    {
        ShowOnce();
    }
}
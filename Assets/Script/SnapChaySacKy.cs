using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SnapChaySacKy : MonoBehaviour
{
    [Header("Anchor")]
    [SerializeField] private Transform anchor;

    [Header("Trigger Conditions")]
    [SerializeField] private string triggerTag = "BinhSacKy";
    [SerializeField] private bool snapOnEnter = false;
    [SerializeField] private bool snapOnStay = true;

    [Header("Snap Options")]
    [SerializeField] private bool allowResnapAfterExit = false;
    [SerializeField] private bool smoothSnap = false;
    [SerializeField] private float positionLerpSpeed = 8f;
    [SerializeField] private float rotationLerpSpeed = 12f;

    [SerializeField] private Vector3 positionOffset = Vector3.zero;
    [SerializeField] private Vector3 eulerOffset = Vector3.zero;

    [SerializeField] private bool freezeRigidbodyAfterSnap = true;
    [SerializeField] private bool applyInLateUpdate = true;

    [Header("Fail Pose (optional)")]
    [SerializeField] private bool enableFailPose = false;
    [SerializeField] private Vector3 failEulerOffset = Vector3.zero;
    [SerializeField] private Vector3 failPositionOffset = Vector3.zero;

    [Header("Debug")]
    [SerializeField] private bool logDebug = false;

    [Header("Grab Integration")]
    [SerializeField] private bool releaseAndDisableGrabOnSnap = true; // release current grab and disable few frames
    [SerializeField] private float grabDisableSeconds = 0.25f;

    private bool snapped;
    private Coroutine smoothRoutine;
    private bool pendingLateSnap;
    private Rigidbody rb;

    // fail pose flag for *next* snap
    private bool applyFailOnNextSnap;

    // Oculus Interaction components
    private GrabInteractable grabInteractable;
    private Grabbable grabbable;
    private HandGrabInteractable handGrabInteractable;
    private PhysicsGrabbable physicsGrabbable;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<GrabInteractable>();
        grabbable = GetComponent<Grabbable>();
        handGrabInteractable = GetComponent<HandGrabInteractable>();
        physicsGrabbable = GetComponent<PhysicsGrabbable>();

        if (anchor == null) Debug.LogWarning("[SnapChaySacKy] Anchor not assigned.", this);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!snapOnEnter || !IsValid(other)) return;
        TrySnap();
    }

    void OnTriggerStay(Collider other)
    {
        if (!snapOnStay || !IsValid(other)) return;
        if (!snapped) TrySnap();
    }

    void OnTriggerExit(Collider other)
    {
        if (!IsValid(other)) return;
        if (allowResnapAfterExit)
        {
            if (smoothRoutine != null) StopCoroutine(smoothRoutine);
            smoothRoutine = null;
            snapped = false;
            applyFailOnNextSnap = false;
        }
    }

    void LateUpdate()
    {
        if (pendingLateSnap)
        {
            pendingLateSnap = false;
            ApplyInstantPose();
        }
    }

    private bool IsValid(Collider other)
    {
        return anchor != null && (string.IsNullOrEmpty(triggerTag) || other.CompareTag(triggerTag));
    }

    public void SnapNow() => TrySnap(true);

    /// <summary>
    /// Mark that the next snap should use the fail pose offsets.
    /// </summary>
    public void SetFailPoseForNextSnap()
    {
        if (!enableFailPose)
        {
            if (logDebug) Debug.Log("[SnapChaySacKy] Fail pose requested but enableFailPose is false.", this);
            return;
        }

        applyFailOnNextSnap = true;
        if (logDebug) Debug.Log("[SnapChaySacKy] Fail pose armed for next snap.", this);
    }

    private void TrySnap(bool force = false)
    {
        if (anchor == null) return;
        if (snapped && !force) return;

        if (freezeRigidbodyAfterSnap && rb)
        {
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (releaseAndDisableGrabOnSnap && (grabInteractable != null || grabbable != null || handGrabInteractable != null || physicsGrabbable != null))
        {
            StartCoroutine(DisableGrabTemporarily());
        }

        if (smoothSnap)
        {
            if (smoothRoutine != null) StopCoroutine(smoothRoutine);
            smoothRoutine = StartCoroutine(SmoothSnapRoutine());
        }
        else
        {
            if (applyInLateUpdate) pendingLateSnap = true;
            else ApplyInstantPose();
        }

        snapped = true;
        if (logDebug) Debug.Log("[SnapChaySacKy] Snapped.", this);
    }

    private IEnumerator DisableGrabTemporarily()
    {
        if (grabInteractable != null)
        {
            foreach (var interactor in grabInteractable.SelectingInteractors)
            {
                grabInteractable.RemoveInteractorByIdentifier(interactor.Identifier);
            }
        }

        bool giWasEnabled = grabInteractable ? grabInteractable.enabled : false;
        bool gWasEnabled = grabbable ? grabbable.enabled : false;
        bool hgiWasEnabled = handGrabInteractable ? handGrabInteractable.enabled : false;
        bool pgWasEnabled = physicsGrabbable ? physicsGrabbable.enabled : false;

        if (grabInteractable) grabInteractable.enabled = false;
        if (grabbable) grabbable.enabled = false;
        if (handGrabInteractable) handGrabInteractable.enabled = false;
        if (physicsGrabbable) physicsGrabbable.enabled = false;

        pendingLateSnap = true;

        if (rb)
        {
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        float elapsed = 0f;
        while (elapsed < grabDisableSeconds)
        {
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        if (grabbable) grabbable.enabled = gWasEnabled;
        if (grabInteractable) grabInteractable.enabled = giWasEnabled;
        if (handGrabInteractable) handGrabInteractable.enabled = hgiWasEnabled;
        if (physicsGrabbable) physicsGrabbable.enabled = pgWasEnabled;
    }

    private void ApplyInstantPose()
    {
        // base snap pose
        Quaternion targetRot = anchor.rotation * Quaternion.Euler(eulerOffset);
        Vector3 targetPos = anchor.position + (anchor.rotation * positionOffset);

        // apply optional fail offsets
        if (enableFailPose && applyFailOnNextSnap)
        {
            targetRot = targetRot * Quaternion.Euler(failEulerOffset);
            if (failPositionOffset != Vector3.zero)
            {
                targetPos += targetRot * failPositionOffset;
            }
            applyFailOnNextSnap = false;
        }

        transform.SetPositionAndRotation(targetPos, targetRot);
    }

    private IEnumerator SmoothSnapRoutine()
    {
        const float posThresholdSqr = 0.00005f;
        const float rotThresholdDeg = 0.15f;
        int safety = 0;

        while (true)
        {
            Quaternion targetRot = anchor.rotation * Quaternion.Euler(eulerOffset);
            Vector3 targetPos = anchor.position + (anchor.rotation * positionOffset);

            if (enableFailPose && applyFailOnNextSnap)
            {
                targetRot = targetRot * Quaternion.Euler(failEulerOffset);
                if (failPositionOffset != Vector3.zero)
                {
                    targetPos += targetRot * failPositionOffset;
                }
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotationLerpSpeed);
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * positionLerpSpeed);

            bool done =
                (Vector3.SqrMagnitude(transform.position - targetPos) < posThresholdSqr) &&
                (Quaternion.Angle(transform.rotation, targetRot) < rotThresholdDeg);

            if (done || (++safety > 600))
            {
                // clear fail flag once pose is fully applied
                applyFailOnNextSnap = false;
                transform.SetPositionAndRotation(targetPos, targetRot);
                smoothRoutine = null;
                yield break;
            }

            yield return null;
        }
    }
}
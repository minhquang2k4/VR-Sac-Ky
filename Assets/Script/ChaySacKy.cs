using System.Collections;
using UnityEngine;

public class ChaySacKy : ThuLai
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private MeshRenderer left;
    [SerializeField] private MeshRenderer right;
    [SerializeField] private Canvas canvasEnd;

    [Header("Failure Chance")]
    [SerializeField][Range(0f, 1f)] private float failureChance = 0.2f;
    [SerializeField] private Vector3 failureEulerOffset = new Vector3(0f, 45f, 0f);
    [SerializeField] private Vector3 failurePosOffset = Vector3.zero;

    private float alpha = 0.05f;
    private float alphaUV = 0.3f;

    private bool runningSacKy = false;
    private bool attemptedSacKy = false;


    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("BinhSacKy"))
            return;

        if (attemptedSacKy) return;

        if (other.gameObject.CompareTag("BinhSacKy"))
        {
            attemptedSacKy = true;
            MemorizePos();
            float roll = Random.value;
            Debug.Log($"[ChaySacKy] Enter trigger. failureChance={failureChance:F3}, roll={roll:F3}");

            //That bai -> Thu lai
            if (roll < failureChance)
            {
                ApplyFailPose();
                runningSacKy = false;
                FailOverlayManager.ShowOverlay();
                RaiseFailed();
                return;
            }

            //Thanh cong -> Chay Sac Ky
            runningSacKy = true;
            StartCoroutine(SacKy());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (runningSacKy && other.gameObject.CompareTag("UV"))
        {
            canvasGroup.alpha = alphaUV;
            canvasEnd.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (runningSacKy && other.gameObject.CompareTag("UV"))
        {
            canvasGroup.alpha = alpha;
        }
    }

    IEnumerator SacKy()
    {
        yield return new WaitForSeconds(30);
        left.enabled = false;
        right.enabled = false;
        while (canvasGroup.alpha < alpha)
        {
            canvasGroup.alpha += Time.deltaTime / 10;
            yield return null;
        }
    }

    private void ApplyFailPose()
    {
        //Quay lech so voi huong ban dau
        transform.rotation = transform.rotation * Quaternion.Euler(failureEulerOffset);
        if (failurePosOffset != Vector3.zero)
        {
            transform.position += transform.rotation * failurePosOffset;
        }
    }
}

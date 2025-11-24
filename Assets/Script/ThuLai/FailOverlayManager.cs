using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FailOverlayManager : MonoBehaviour
{
    private static FailOverlayManager _instance;

    [Header("World Space Overlay")]
    [SerializeField] private Camera vrCamera;
    [SerializeField] private float distance = 0.6f;
    [SerializeField] private float width = 1.2f;
    [SerializeField] private float height = 0.8f;
    [SerializeField] private Color overlayColor = new Color(1f, 0f, 0f, 0.35f);
    [SerializeField] private bool pauseOnShow = false;

    [Header("Auto Scene Reset")]
    [SerializeField] private bool autoResetOnShow = true;  
    [SerializeField] private float resetDelaySeconds = 5f;  
    private Coroutine resetRoutine;

    private Canvas canvas;
    private Image img;
    private bool shown;
    private float prevTimeScale = 1f;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
        BuildCanvas();
        HideOverlayImmediate();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (vrCamera == null) vrCamera = Camera.main;
        HideOverlayImmediate();
    }

    private void BuildCanvas()
    {
        if (vrCamera == null) vrCamera = Camera.main;

        GameObject cGO = new GameObject("FailOverlayCanvas");
        cGO.transform.SetParent(transform, false);
        canvas = cGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = vrCamera;
        canvas.sortingOrder = 5000;

        var scaler = cGO.AddComponent<CanvasScaler>();
        scaler.scaleFactor = 1f;
        scaler.dynamicPixelsPerUnit = 100f;

        cGO.AddComponent<GraphicRaycaster>();

        GameObject imgGO = new GameObject("OverlayImage");
        imgGO.transform.SetParent(canvas.transform, false);
        img = imgGO.AddComponent<Image>();
        img.color = overlayColor;

        RectTransform rt = img.rectTransform;
        rt.sizeDelta = new Vector2(width, height);
    }

    void LateUpdate()
    {
        if (shown && vrCamera != null)
        {
            Transform t = canvas.transform;
            t.position = vrCamera.transform.position + vrCamera.transform.forward * distance;
            t.rotation = Quaternion.LookRotation(vrCamera.transform.forward, vrCamera.transform.up);
        }
    }

    public static void ShowOverlay()
    {
        if (_instance == null) return;
        if (_instance.shown) return;

        _instance.shown = true;
        _instance.canvas.gameObject.SetActive(true);

        if (_instance.pauseOnShow)
        {
            _instance.prevTimeScale = Time.timeScale;
            Time.timeScale = 0f;
        }

        if (_instance.autoResetOnShow)
        {
            if (_instance.resetRoutine != null)
            {
                _instance.StopCoroutine(_instance.resetRoutine);
            }
            _instance.resetRoutine = _instance.StartCoroutine(_instance.ResetAfterDelay());
        }
    }

    public static void HideOverlay()
    {
        if (_instance == null) return;
        if (!_instance.shown) return;

        _instance.shown = false;
        _instance.canvas.gameObject.SetActive(false);

        if (_instance.pauseOnShow)
        {
            Time.timeScale = _instance.prevTimeScale;
        }

        if (_instance.resetRoutine != null)
        {
            _instance.StopCoroutine(_instance.resetRoutine);
            _instance.resetRoutine = null;
        }
    }

    public static void CancelPendingReset()
    {
        if (_instance == null) return;
        if (_instance.resetRoutine != null)
        {
            _instance.StopCoroutine(_instance.resetRoutine);
            _instance.resetRoutine = null;
        }
    }

    private IEnumerator ResetAfterDelay()
    {
        float elapsed = 0f;
        while (elapsed < resetDelaySeconds)
        {
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        if (pauseOnShow)
        {
            Time.timeScale = prevTimeScale;
        }

        var currentIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentIndex);
    }

    private void HideOverlayImmediate()
    {
        shown = false;
        if (canvas != null) canvas.gameObject.SetActive(false);
        if (resetRoutine != null)
        {
            StopCoroutine(resetRoutine);
            resetRoutine = null;
        }
    }
}
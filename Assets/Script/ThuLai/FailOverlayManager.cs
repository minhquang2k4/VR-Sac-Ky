using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [Header("Fail Message Card")]
    // The tint sits right on the camera so nothing in the lab can occlude it, but text is
    // unreadable that close, so the card is pushed further out along the same view ray.
    // Raise this if the card feels uncomfortably close - it changes depth, not apparent size.
    [SerializeField] private float cardDistance = 0.6f;
    // How much of the player's view the card covers, in degrees. Driving the card from an
    // angle rather than a width in metres keeps it looking identical at any cardDistance.
    [Range(8f, 60f)][SerializeField] private float cardFieldOfView = 26f;
    [SerializeField] private Color cardColor = new Color(0.06f, 0.06f, 0.08f, 0.9f);
    [SerializeField] private Color cardBorderColor = new Color(1f, 0.35f, 0.3f, 1f);

    [Header("Fail Message Text")]
    [SerializeField] private TMP_FontAsset messageFont;
    [SerializeField] private Color messageColor = Color.white;
    [SerializeField] private string titleMessage = "THẤT BẠI";
    [SerializeField] private string countdownLabelFormat = "Thử lại sau {0} giây...";

    // The card is laid out in a fixed reference space and then scaled as a whole to hit
    // cardFieldOfView, so every proportion below stays put no matter how the card is tuned.
    // Only X and Y are scaled: a non-uniform scale would stretch the glyphs, and squashing Z
    // serves no purpose on flat UI.
    private const float CardRefWidth = 1800f;
    private const float CardRefHeight = 1000f;
    private const float BorderRefThickness = 25f;
    private const float TitleRefSize = 160f;
    private const float ReasonRefSize = 130f;
    private const float CountdownRefSize = 140f;

    // Static so the 1024x1024 atlas survives the scene reload each failure triggers.
    private static TMP_FontAsset runtimeFont;

    private Canvas canvas;
    private Image img;
    private GameObject cardRoot;
    private TextMeshProUGUI titleText;
    private TextMeshProUGUI reasonText;
    private TextMeshProUGUI countdownText;
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
        RepairFontAtlasMaterials();
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
        // The previous scene's camera was destroyed along with it, so re-point the canvas.
        if (canvas != null) canvas.worldCamera = vrCamera;
        HideOverlayImmediate();
    }

    // This project's TMP font assets are broken on disk: both "LiberationSans SDF" and
    // "LiberationSans SDF - Fallback" serialize their material's _MainTex as null.
    // TMP_MaterialManager.GetFallbackMaterial dereferences that texture without a null check,
    // so the first character resolving through the fallback - every Vietnamese diacritic,
    // since the static atlas has none - throws and the label renders nothing. Walk the whole
    // chain once, repair any material missing its atlas, and grab a usable source TTF on the
    // way past so the overlay can build a font asset that does not depend on any of this.
    private Font sourceFont;

    private void RepairFontAtlasMaterials()
    {
        var visited = new HashSet<int>();
        WalkFontAsset(messageFont, visited);
        WalkFontAsset(TMP_Settings.defaultFontAsset, visited);

        var globalFallbacks = TMP_Settings.fallbackFontAssets;
        if (globalFallbacks == null) return;
        for (int i = 0; i < globalFallbacks.Count; i++)
        {
            WalkFontAsset(globalFallbacks[i], visited);
        }
    }

    private void WalkFontAsset(TMP_FontAsset fontAsset, HashSet<int> visited)
    {
        if (fontAsset == null) return;
        // Fallback tables can reference each other, so only walk each asset once.
        if (!visited.Add(fontAsset.GetInstanceID())) return;

        // Static assets drop their source TTF reference, so only a dynamic one supplies this.
        if (sourceFont == null && fontAsset.sourceFontFile != null)
        {
            sourceFont = fontAsset.sourceFontFile;
        }

        Material mat = fontAsset.material;
        bool hasAtlas = fontAsset.atlasTextures != null && fontAsset.atlasTextures.Length > 0;
        if (mat != null && hasAtlas && mat.HasProperty("_MainTex") && mat.GetTexture("_MainTex") == null)
        {
            mat.SetTexture("_MainTex", fontAsset.atlasTexture);
            Debug.Log($"[FailOverlayManager] Repaired missing atlas texture on font asset '{fontAsset.name}'.");
        }

        var fallbacks = fontAsset.fallbackFontAssetTable;
        if (fallbacks == null) return;
        for (int i = 0; i < fallbacks.Count; i++)
        {
            WalkFontAsset(fallbacks[i], visited);
        }
    }

    // Built from the TTF rather than reused from the project, so the overlay owns a freshly
    // created atlas and material and rasterizes Vietnamese on demand. That sidesteps both the
    // broken _MainTex references and the fallback lookup that was throwing.
    private TMP_FontAsset ResolveFont()
    {
        if (messageFont != null) return messageFont;
        if (runtimeFont != null) return runtimeFont;

        if (sourceFont != null)
        {
            runtimeFont = TMP_FontAsset.CreateFontAsset(sourceFont);
            if (runtimeFont != null)
            {
                runtimeFont.name = "FailOverlay Runtime Font";
                return runtimeFont;
            }
            Debug.LogWarning($"[FailOverlayManager] Could not build a font asset from '{sourceFont.name}'.");
        }
        else
        {
            Debug.LogWarning("[FailOverlayManager] No source font file found; Vietnamese may not render.");
        }

        return TMP_Settings.defaultFontAsset;
    }

    private void BuildCanvas()
    {
        if (vrCamera == null) vrCamera = Camera.main;

        GameObject cGO = new GameObject("FailOverlayCanvas");
        cGO.transform.SetParent(transform, false);
        // Hidden before anything else is built, so a failure while building the card can
        // never strand a full-screen red overlay on the player's view.
        cGO.SetActive(false);

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

        BuildFailCard();
    }

    private void BuildFailCard()
    {
        // The canvas is billboarded so its +Z runs along the camera's forward axis; offsetting
        // the card along it parks the card further from the eye than the tint. Being a later
        // sibling it still draws over the tint, since UI never writes depth.
        cardRoot = new GameObject("FailCard", typeof(RectTransform));
        cardRoot.transform.SetParent(canvas.transform, false);
        RectTransform cardRootRT = cardRoot.GetComponent<RectTransform>();
        CenterOnParent(cardRootRT);
        cardRootRT.sizeDelta = new Vector2(CardRefWidth, CardRefHeight);

        // Width the card must have at cardDistance to subtend cardFieldOfView degrees.
        float cardWorldWidth = 2f * cardDistance * Mathf.Tan(cardFieldOfView * 0.5f * Mathf.Deg2Rad);
        float cardScale = cardWorldWidth / CardRefWidth;
        cardRootRT.localScale = new Vector3(cardScale, cardScale, 1f);
        cardRootRT.anchoredPosition3D = new Vector3(0f, 0f, cardDistance - distance);

        // Border first, then background on top of it - uGUI draws later siblings last, so the
        // background covers the middle of the border and only its rim shows through.
        BuildCardPanel("CardBorder", cardBorderColor,
            new Vector2(CardRefWidth + BorderRefThickness * 2f,
                        CardRefHeight + BorderRefThickness * 2f));
        BuildCardPanel("CardBackground", cardColor,
            new Vector2(CardRefWidth, CardRefHeight));

        float textWidth = CardRefWidth * 0.88f;

        titleText = BuildLabel("TitleText", TitleRefSize,
            new Vector2(textWidth, CardRefHeight * 0.3f),
            new Vector2(0f, CardRefHeight * 0.32f));
        titleText.text = titleMessage;
        titleText.fontStyle = FontStyles.Bold;
        titleText.gameObject.SetActive(!string.IsNullOrWhiteSpace(titleMessage));

        reasonText = BuildLabel("ReasonText", ReasonRefSize,
            new Vector2(textWidth, CardRefHeight * 0.4f),
            Vector2.zero);
        reasonText.enableWordWrapping = true;

        countdownText = BuildLabel("CountdownText", CountdownRefSize,
            new Vector2(textWidth, CardRefHeight * 0.3f),
            new Vector2(0f, -CardRefHeight * 0.32f));
    }

    private void BuildCardPanel(string panelName, Color color, Vector2 size)
    {
        GameObject panelGO = new GameObject(panelName);
        panelGO.transform.SetParent(cardRoot.transform, false);
        Image panel = panelGO.AddComponent<Image>();
        panel.color = color;
        CenterOnParent(panel.rectTransform);
        panel.rectTransform.sizeDelta = size;
    }

    private TextMeshProUGUI BuildLabel(string labelName, float textSize, Vector2 size, Vector2 position)
    {
        GameObject labelGO = new GameObject(labelName);
        labelGO.transform.SetParent(cardRoot.transform, false);
        TextMeshProUGUI label = labelGO.AddComponent<TextMeshProUGUI>();

        TMP_FontAsset font = ResolveFont();
        if (font != null) label.font = font;

        label.fontSize = textSize;
        label.alignment = TextAlignmentOptions.Center;
        label.color = messageColor;
        label.enableWordWrapping = false;
        label.overflowMode = TextOverflowModes.Overflow;

        // The configured size is a ceiling: a long reason string or a wide countdown line
        // shrinks to stay inside the card instead of spilling past its edge.
        label.enableAutoSizing = true;
        label.fontSizeMin = textSize * 0.5f;
        label.fontSizeMax = textSize;

        CenterOnParent(label.rectTransform);
        label.rectTransform.sizeDelta = size;
        label.rectTransform.anchoredPosition = position;

        return label;
    }

    private static void CenterOnParent(RectTransform rt)
    {
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = Vector2.zero;
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

    public static void ShowOverlay(string reasonMessage = "")
    {
        if (_instance == null) return;
        if (_instance.shown) return;

        _instance.shown = true;
        _instance.canvas.gameObject.SetActive(true);

        // With no reason to show, fall back to the bare red tint instead of an empty card.
        bool hasReason = !string.IsNullOrWhiteSpace(reasonMessage);
        if (_instance.cardRoot != null)
        {
            _instance.cardRoot.SetActive(hasReason);
        }

        if (_instance.reasonText != null)
        {
            _instance.reasonText.text = reasonMessage;
        }

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
        else if (_instance.countdownText != null)
        {
            _instance.countdownText.text = "";
        }
    }

    public static void HideOverlay()
    {
        if (_instance == null) return;
        if (!_instance.shown) return;

        _instance.shown = false;
        _instance.canvas.gameObject.SetActive(false);

        if (_instance.reasonText != null) _instance.reasonText.text = "";
        if (_instance.countdownText != null) _instance.countdownText.text = "";

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
            if (countdownText != null)
            {
                int secondsLeft = Mathf.Max(Mathf.CeilToInt(resetDelaySeconds - elapsed), 0);
                countdownText.text = string.Format(countdownLabelFormat, secondsLeft);
            }

            // Unscaled, because pauseOnShow freezes timeScale while the overlay is up.
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        if (countdownText != null)
        {
            countdownText.text = string.Format(countdownLabelFormat, 0);
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
        if (reasonText != null) reasonText.text = "";
        if (countdownText != null) countdownText.text = "";
        if (resetRoutine != null)
        {
            StopCoroutine(resetRoutine);
            resetRoutine = null;
        }
    }
}

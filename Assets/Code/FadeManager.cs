using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;

    [Header("Fade Settings")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private Color fadeColor = Color.black;

    private Coroutine currentFadeCoroutine;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SetupFadeImage();
    }

    private void SetupFadeImage()
    {
        if (fadeImage == null)
        {
            Canvas canvas = GetComponentInChildren<Canvas>();
            if (canvas == null)
            {
                GameObject canvasObj = new GameObject("FadeCanvas");
                canvasObj.transform.SetParent(transform);
                canvas = canvasObj.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.sortingOrder = 9999; // Ensure it's on top

                CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = new Vector2(1920, 1080);

                canvasObj.AddComponent<GraphicRaycaster>();
            }

            GameObject fadeObj = new GameObject("FadeImage");
            fadeObj.transform.SetParent(canvas.transform);
            fadeImage = fadeObj.AddComponent<Image>();
            
            RectTransform rect = fadeImage.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.sizeDelta = Vector2.zero;
            rect.anchoredPosition = Vector2.zero;
        }

        fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 0f);
        fadeImage.raycastTarget = false;
    }

    public void FadeTransition(System.Action onFadeComplete)
    {
        if (currentFadeCoroutine != null)
        {
            StopCoroutine(currentFadeCoroutine);
        }
        currentFadeCoroutine = StartCoroutine(FadeTransitionCoroutine(onFadeComplete));
    }

    private IEnumerator FadeTransitionCoroutine(System.Action onFadeComplete)
    {
        yield return StartCoroutine(Fade(0f, 1f, fadeDuration));
        
        onFadeComplete?.Invoke();
        
        yield return new WaitForSeconds(0.1f);
        
        yield return StartCoroutine(Fade(1f, 0f, fadeDuration));

        currentFadeCoroutine = null;
    }

    
    private IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha);
            yield return null;
        }

        fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, endAlpha);
    }

    
    public void FadeToBlack(float duration = -1f)
    {
        if (duration < 0) duration = fadeDuration;
        if (currentFadeCoroutine != null) StopCoroutine(currentFadeCoroutine);
        currentFadeCoroutine = StartCoroutine(Fade(fadeImage.color.a, 1f, duration));
    }

    public void FadeToClear(float duration = -1f)
    {
        if (duration < 0) duration = fadeDuration;
        if (currentFadeCoroutine != null) StopCoroutine(currentFadeCoroutine);
        currentFadeCoroutine = StartCoroutine(Fade(fadeImage.color.a, 0f, duration));
    }
}
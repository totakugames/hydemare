using UnityEngine;

public class SanityVignetteFollower : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform target; // <- player
    [SerializeField] private Material vignetteMaterial;
    [SerializeField] private Camera cam;

    [Header("Smoothing")]
    [SerializeField] private float followSpeed = 10f;
    [SerializeField] private bool useSmoothing = true;

    private Vector2 currentCenter = new Vector2(0.5f, 0.5f);
    private static readonly int VignetteCenterID = Shader.PropertyToID("_VignetteCenter");

    private void Start()
    {
        if (cam == null)
            cam = Camera.main;
    }

    private void Update()
    {
        if (target == null || vignetteMaterial == null || cam == null) return;

        Vector3 screenPos = cam.WorldToViewportPoint(target.position);
        Vector2 targetCenter = new Vector2(screenPos.x, screenPos.y);

        targetCenter.x = Mathf.Clamp01(targetCenter.x);
        targetCenter.y = Mathf.Clamp01(targetCenter.y);

        if (useSmoothing)
        {
            currentCenter = Vector2.Lerp(currentCenter, targetCenter, followSpeed * Time.deltaTime);
        }
        else
        {
            currentCenter = targetCenter;
        }

        vignetteMaterial.SetVector(VignetteCenterID, new Vector4(currentCenter.x, currentCenter.y, 0, 0));
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void ResetToCenter()
    {
        currentCenter = new Vector2(0.5f, 0.5f);
        if (vignetteMaterial != null)
        {
            vignetteMaterial.SetVector(VignetteCenterID, new Vector4(0.5f, 0.5f, 0, 0));
        }
    }

    private void OnDisable()
    {
        ResetToCenter();
    }
}

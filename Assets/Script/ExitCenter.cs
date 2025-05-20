using UnityEngine;

[RequireComponent(typeof(RectTransform))]
[ExecuteAlways]
public class WindowCentering : MonoBehaviour
{
    public enum UpdateMode
    {
        OnlyOnEnable,
        Continuous
    }

    [Header("Camera Settings")]
    [Tooltip("Target camera (uses Canvas camera if null)")]
    [SerializeField] private Camera targetCamera;
    
    [Header("Update Settings")]
    [SerializeField] private UpdateMode updateMode = UpdateMode.OnlyOnEnable;
    [SerializeField] private Vector2 offset = Vector2.zero;
    [SerializeField] private bool centerOnEnable = true;

    private RectTransform _rectTransform;
    private Canvas _parentCanvas;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _parentCanvas = GetComponentInParent<Canvas>();
    }

    private void OnEnable()
    {
        if (centerOnEnable) CenterWindow();
    }

    private void Update()
    {
        if (updateMode == UpdateMode.Continuous)
            CenterWindow();
    }

    public void CenterWindow()
    {
        if (_rectTransform == null || _parentCanvas == null) return;
        
        Camera cam = GetTargetCamera();
        if (cam == null) return;

        switch (_parentCanvas.renderMode)
        {
            case RenderMode.ScreenSpaceCamera:
                CenterScreenSpaceCamera(cam);
                break;
                
            case RenderMode.ScreenSpaceOverlay:
                CenterScreenSpaceOverlay();
                break;
                
            case RenderMode.WorldSpace:
                CenterWorldSpace(cam);
                break;
        }
    }

    private Camera GetTargetCamera()
    {
        // Если камера указана вручную - используем её
        if (targetCamera != null) 
            return targetCamera;
        
        // Используем камеру Canvas для ScreenSpaceCamera
        if (_parentCanvas.renderMode == RenderMode.ScreenSpaceCamera)
            return _parentCanvas.worldCamera;
        
        // По умолчанию - основная камера
        return Camera.main;
    }

    private void CenterScreenSpaceCamera(Camera cam)
    {
        if (cam != null)
        {
            // Для ScreenSpaceCamera используем Viewport
            Vector3 center = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, GetPlaneDistance()));
            _rectTransform.position = center + (Vector3)offset;
        }
        else
        {
            // Fallback: обычное центрирование
            _rectTransform.anchoredPosition = offset;
        }
    }

    private float GetPlaneDistance()
    {
        // В новых версиях Unity planeDistance хранится в Canvas
        return _parentCanvas.planeDistance;
    }

    private void CenterScreenSpaceOverlay()
    {
        // Для Overlay просто центрируем
        _rectTransform.anchoredPosition = offset;
    }

    private void CenterWorldSpace(Camera cam)
    {
        if (cam != null)
        {
            Vector3 screenCenter = new Vector3(0.5f, 0.5f, 10f);
            Vector3 worldPos = cam.ViewportToWorldPoint(screenCenter);
            _rectTransform.position = worldPos + (Vector3)offset;
        }
    }

    #if UNITY_EDITOR
    private void OnValidate()
    {
        if (!Application.isPlaying && _rectTransform == null)
            _rectTransform = GetComponent<RectTransform>();
    }
    #endif
}
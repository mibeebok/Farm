using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public FarmGrid farmGrid;
    [Range(0.1f, 10f)] public float cameraFollowSpeed = 5f;
    [Range(0f, 1f)] public float edgeSoftness = 0.5f; // Мягкость у краёв

    private Vector3 offset;
    private Vector2 gridBounds;
    private Camera mainCamera;
    private float cameraHeight;
    private float cameraWidth;

    private void Start()
    {
        mainCamera = Camera.main;
        offset = transform.position - player.position;
        gridBounds = farmGrid.GetGridBounds();
        CalculateCameraSize();
    }

    private void CalculateCameraSize()
    {
        cameraHeight = 2f * mainCamera.orthographicSize;
        cameraWidth = cameraHeight * mainCamera.aspect;
    }

    private void LateUpdate()
    {
        Vector3 targetPosition = player.position + offset;
        
        // Мягкие границы
        float softX = Mathf.Lerp(
            -gridBounds.x + cameraWidth/2,
            -gridBounds.x + cameraWidth * edgeSoftness,
            Mathf.InverseLerp(-gridBounds.x, -gridBounds.x + cameraWidth/2, player.position.x)
        );
        
        float hardX = gridBounds.x - cameraWidth/2;
        
        targetPosition.x = player.position.x < 0 ? 
            Mathf.Clamp(targetPosition.x, softX, hardX) :
            Mathf.Clamp(targetPosition.x, -hardX, -softX);

        // Аналогично для оси Y
        float softY = Mathf.Lerp(
            -gridBounds.y + cameraHeight/2,
            -gridBounds.y + cameraHeight * edgeSoftness,
            Mathf.InverseLerp(-gridBounds.y, -gridBounds.y + cameraHeight/2, player.position.y)
        );
        
        float hardY = gridBounds.y - cameraHeight/2;
        
        targetPosition.y = player.position.y < 0 ? 
            Mathf.Clamp(targetPosition.y, softY, hardY) :
            Mathf.Clamp(targetPosition.y, -hardY, -softY);

        // Плавное движение с учетом deltaTime
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            cameraFollowSpeed * Time.deltaTime
        );
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(gridBounds.x*2, gridBounds.y*2, 0));
        
        Gizmos.color = new Color(1, 0.5f, 0, 0.7f);
        Gizmos.DrawWireCube(Vector3.zero, 
            new Vector3(
                gridBounds.x*2 - cameraWidth * edgeSoftness*2,
                gridBounds.y*2 - cameraHeight * edgeSoftness*2,
                0
            )
        );
    }
}
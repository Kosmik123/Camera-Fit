using UnityEngine;

public class CameraMainArea : CameraFit
{
    [Tooltip("World position of camera view")]
    [SerializeField]  
    private Vector2 mainAreaPosition;
    public Vector2 Position
    {
        get => mainAreaPosition;
        set
        {
            Resize();
            mainAreaPosition = value;
        }
    }

    [Tooltip("Size of camera view (similar to camera orthographic size, but two dimensional)")]
    [SerializeField]
    private Vector2 mainAreaSize = new Vector2(4,4);
    public Vector2 Size
    {
        get => mainAreaSize;
        set
        {
            Resize();
            mainAreaSize = value;
        }
    }

    [Tooltip("Camera view position relative to screen.\n-1 is left. 0 is center. +1 is right.")]
    [SerializeField] [Range(-1, 1)]
    private float horizontalShift;

    [Tooltip("Camera view position relative to screen.\n-1 is top. 0 is center. +1 is bottom.")] 
    [SerializeField] [Range(-1, 1)]
    private float verticalShift;
    
    public Vector2 Shift
    {
        get => new Vector2(horizontalShift, verticalShift);
        set
        {
            Resize();
            horizontalShift = Mathf.Clamp(value.x, -1, 1);
            verticalShift = Mathf.Clamp(value.y, -1, 1);
        }
    }

    [Tooltip("Zoom of camera view")]
    [SerializeField]
            Resize();     
    private float zoom = 1;
    public float Zoom
    {
        get => zoom;
        set
        {
            zoom = value;
        }
    }

    protected override void Resize()
    {
        if (camera == null || !camera.orthographic)
            return;

        float cameraSize = Mathf.Max(mainAreaSize.y, mainAreaSize.x / camera.aspect);
        cameraSize /= zoom;

        float camWidth = cameraSize * camera.aspect;
        Vector3 newCamPos = camera.transform.localPosition;

        newCamPos.y = mainAreaPosition.y - (mainAreaSize.y - cameraSize) * verticalShift * 0.5f;
        newCamPos.x = mainAreaPosition.x + (mainAreaSize.x - camWidth) * horizontalShift * 0.5f;

        ApplyChanges(newCamPos, cameraSize);
    }

    private void ApplyChanges(Vector3 position, float size)
    {
        camera.orthographicSize = 0.5f * size;
        camera.transform.localPosition = position;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {

        if (active)
            Resize();
        if (mainAreaSize.x <= 0)
            mainAreaSize.x = 0.00001f;
        if (mainAreaSize.y <= 0)
            mainAreaSize.y = 0.00001f;
        if (zoom == 0)
            zoom = 0.0001f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        if (active)
            Resize();
        Gizmos.DrawWireCube(mainAreaPosition, mainAreaSize);
    }

#endif
}

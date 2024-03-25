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
            mainAreaPosition = value;
            Refresh();
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
            mainAreaSize = value;
            Refresh();
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
            horizontalShift = Mathf.Clamp(value.x, -1, 1);
            verticalShift = Mathf.Clamp(value.y, -1, 1);
            Refresh();
        }
    }

    [Tooltip("Zoom of camera view")]
    [SerializeField]
    private float zoom = 1;
    public float Zoom
    {
        get => zoom;
        set
        {
            zoom = value;
            Refresh();
        }
    }

    protected override void Resize()
    {
        if (_camera == null || !_camera.orthographic)
            return;

        float cameraSize = Mathf.Max(mainAreaSize.y, mainAreaSize.x / _camera.aspect);
        cameraSize /= zoom;

        float camWidth = cameraSize * _camera.aspect;
        Vector3 newCamPos = _camera.transform.localPosition;

        newCamPos.y = mainAreaPosition.y - (mainAreaSize.y - cameraSize) * verticalShift * 0.5f;
        newCamPos.x = mainAreaPosition.x + (mainAreaSize.x - camWidth) * horizontalShift * 0.5f;

        ApplyChanges(newCamPos, cameraSize);
    }

    private void ApplyChanges(Vector3 position, float size)
    {
        _camera.orthographicSize = 0.5f * size;
        _camera.transform.localPosition = position;
    }

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        if (mainAreaSize.x <= 0)
            mainAreaSize.x = 0.00001f;
        if (mainAreaSize.y <= 0)
            mainAreaSize.y = 0.00001f;
        if (zoom == 0)
            zoom = 0.0001f;
        base.OnValidate();
    }

    protected override void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(mainAreaPosition, mainAreaSize);
        base.OnDrawGizmos();
    }
#endif
}

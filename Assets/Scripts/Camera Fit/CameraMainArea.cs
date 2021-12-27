using UnityEngine;

public class CameraMainArea : CameraFit
{
    public new Camera camera;

    [SerializeField]
    private Vector2 _mainAreaPosition;
    public Vector2 Position
    {
        get { return _mainAreaPosition; }
        set
        {
            _mainAreaPosition = value;
            Resize();
        }
    }

    [SerializeField]
    private Vector2 _mainAreaSize = new Vector2(4,4);
    public Vector2 Size
    {
        get { return _mainAreaSize; }
        set
        {
            _mainAreaSize = value;
            Resize();
        }
    }


    [SerializeField] [Range(-1, 1)]
    private float _horizontalShift, _verticalShift;
    public Vector2 Shift
    {
        get { return new Vector2(_horizontalShift, _verticalShift); }
        set
        {
            _horizontalShift = Mathf.Clamp(value.x, -1, 1);
            _verticalShift = Mathf.Clamp(value.y, -1, 1);
            Resize();
        }
    }

    [SerializeField]
    private float _zoom = 1;
    public float Zoom { 
        get { return _zoom; } 
        set 
        {
            _zoom = value;
            Resize();     
        } 
    }

    private void Awake()
    {
        Resize();
    }

    protected override void Resize()
    {
        if (camera == null || !camera.orthographic)
            return;

        float cameraSize = _mainAreaSize.y / _zoom;
        float camWidth = cameraSize * camera.aspect;
        Vector3 newCamPos = camera.transform.localPosition;
        newCamPos.x = _mainAreaPosition.x + (_mainAreaSize.x - camWidth) * _horizontalShift * 0.5f;
        newCamPos.y = _mainAreaPosition.y;
        if (camWidth < _mainAreaSize.x)
        {
            cameraSize = _mainAreaSize.x / (camera.aspect * _zoom);
            newCamPos.y = _mainAreaPosition.y - (_mainAreaSize.y - cameraSize) * _verticalShift * 0.5f;
            newCamPos.x = _mainAreaPosition.x;
        }

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
        if (_mainAreaSize.x <= 0)
            _mainAreaSize.x = 0.00001f;
        if (_mainAreaSize.y <= 0)
            _mainAreaSize.y = 0.00001f;
        if (_zoom == 0)
            _zoom = 0.0001f;

        if (active)
            Resize();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(_mainAreaPosition, _mainAreaSize);

        if (active)
            Resize();
    }

#endif
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class CameraFit : MonoBehaviour
{
    public bool active = true;
    private float lastAspect;
    protected Camera cam;

    public event EventHandler OnCameraResize;

    protected abstract void Resize();

    protected void Refresh()
    {
        if (cam == null)
            cam = GetComponent<Camera>();
        if (Mathf.Abs(cam.aspect - lastAspect) > 0.01f)
        {
            Resize();
            OnCameraResize?.Invoke(this, EventArgs.Empty);
        }
    }
}


public class CameraOrthographicSize : CameraFit
{
    [Range(0, 1)]
    public float horizontalFit;
    public float size;

    private void Awake()
    {
        Resize();
    }


    protected override void Resize()
    {
        if (cam == null)
            cam = GetComponent<Camera>();
        if (!cam.orthographic)
            return;

        cam.orthographicSize = size * ((1 - horizontalFit) + horizontalFit / cam.aspect);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (active)
            Resize();
    }

    private void OnValidate()
    {
        if (active)
            Resize();
    }
#endif
}

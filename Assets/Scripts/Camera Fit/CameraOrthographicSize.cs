using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class CameraFit : MonoBehaviour
{
    public event Action OnCameraResized;

    [Tooltip("Enable/disable camera changes")]
    public bool active = true;
    private float lastAspect;
    
    [Tooltip("Camera to resize")]
    public new Camera camera;

    protected virtual void Awake()
    {
        Refresh();
    }

    protected abstract void Resize();

    protected void Refresh()
    {
        if (camera == null)
            return;
        if (Mathf.Abs(camera.aspect - lastAspect) > 0.01f)
        {
            Resize();
            OnCameraResized?.Invoke();
        }
    }

#if UNITY_EDITOR
    protected virtual void OnDrawGizmos()
    {
        if (active)
            Resize();
    }

    protected virtual void OnValidate()
    {
        if (active)
            Resize();
    }
#endif
}

public class CameraOrthographicSize : CameraFit
{
    [Tooltip("Dimensions which are affected by size changes.\n" +
        "0 - vertical (same as orthographic size change in Camera)\n" +
        "1 - horizontal (size affects horizontal dimension of camera orthographic size)\n" +
        "Anything between (size affect partially both dimensions)")]
    [Range(0, 1)]
    public float horizontalFit;

    [Tooltip("Size of orthographic camera (similar to Camera orthographic size")] 
    public float size;

    protected override void Resize()
    {
        if (camera == null)
            camera = GetComponent<Camera>();
        if (!camera.orthographic)
            return;

        camera.orthographicSize = size * ((1 - horizontalFit) + horizontalFit / camera.aspect);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class LineRendererWidth : MonoBehaviour
{
    public float width = 100;
    private LineRenderer _lineRenderer;

    private void OnEnable()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.widthMultiplier = width;
    }
}

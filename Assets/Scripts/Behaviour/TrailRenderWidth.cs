using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class TrailRenderWidth : MonoBehaviour
{
    public float width = 100;
    private TrailRenderer trailRenderer;

    private void OnEnable()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        trailRenderer.widthMultiplier = width;
    }
}

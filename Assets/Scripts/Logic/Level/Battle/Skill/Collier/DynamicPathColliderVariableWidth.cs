using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(LineRenderer), typeof(PolygonCollider2D))]
public class DynamicPathColliderVariableWidth : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private PolygonCollider2D polygonCollider;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.widthMultiplier = 1000.0f;
        polygonCollider = GetComponent<PolygonCollider2D>();
    }

    void Update()
    {
        //UpdateColliderPath();
    }

    
    public void Dump()
    {
        List<float> distances = new List<float>(lineRenderer.positionCount);
        float distance = 0;
        for(int i = 0; i < lineRenderer.positionCount-1; i++)
        {
            Vector2 p1 = lineRenderer.GetPosition(i);
            Vector2 p2 = lineRenderer.GetPosition(i + 1);
            distance += Vector2.Distance(p1, p2);
            distances.Add(0);
            for (int j = 0; j < i; j++)
            {
                distances[i] += distances[j];
            }
            distances[i] += Vector2.Distance(p1, p2);
        }
        distances.Insert(0, 0);
        for(int i = 0; i < distances.Count; i++)
        {
            distances[i] = distances[i] / distance;
        }

        for (int i = 0; i < distances.Count; i++)
        {
            Debug.Log(distances[i] + " " + lineRenderer.widthCurve.Evaluate(distances[i]));
        }
    }

    // TODO 需要优化
    float GetPointWidth(int index)
    {
        List<float> distances = new List<float>(lineRenderer.positionCount);
        float distance = 0;
        for (int i = 0; i < lineRenderer.positionCount - 1; i++)
        {
            Vector2 p1 = lineRenderer.GetPosition(i);
            Vector2 p2 = lineRenderer.GetPosition(i + 1);
            distance += Vector2.Distance(p1, p2);
            distances.Add(0);
            for (int j = 0; j < i; j++)
            {
                distances[i] += distances[j];
            }
            distances[i] += Vector2.Distance(p1, p2);
        }
        distances.Insert(0, 0);
        for (int i = 0; i < distances.Count; i++)
        {
            distances[i] = distances[i] / distance;
        }

        

        for (int i = 0; i < distances.Count; i++)
        {
            distances[i] = lineRenderer.widthCurve.Evaluate(distances[i]);
        //Debug.Log(distances[i] + " " + lineRenderer.widthCurve.Evaluate(distances[i]));
        }

        return distances[index] * lineRenderer.widthMultiplier;
    }

    [ContextMenu("Generate")]
    public void UpdateColliderPath()
    {
        int pointCount = lineRenderer.positionCount;
        Vector3[] linePoints = new Vector3[pointCount];
        lineRenderer.GetPositions(linePoints);
        List<Vector2> colliderPathPoints = new List<Vector2>();

        // 计算上边缘的点
        for (int i = 0; i < pointCount; i++)
        {

            Vector2 normal = Vector2.up;
            if (i == 0)
            {
                normal = (linePoints[i + 1] - linePoints[i]);
            }
            else if(i < pointCount - 1)
            {
                normal = (linePoints[i + 1] - linePoints[i]).normalized + (linePoints[i] - linePoints[i-1]).normalized;
            }
            else
            {
                normal = (linePoints[i] - linePoints[i-1]);
            }
            normal = normal.normalized;
            normal = new Vector2(-normal.y, normal.x).normalized;

            Vector2 pointOffset = normal * GetPointWidth(i) * 0.5f;// * widthAtPoint.x;
            colliderPathPoints.Add((Vector2)linePoints[i] + pointOffset);
        }

        // 计算下边缘的点（反向）
        for (int i = pointCount - 1; i >= 0; i--)
        {
            Vector2 normal = Vector2.up;
            if (i == 0)
            {
                normal = (linePoints[i + 1] - linePoints[i]);
            }
            else if (i < pointCount - 1)
            {
                normal = (linePoints[i + 1] - linePoints[i]).normalized + (linePoints[i] - linePoints[i - 1]).normalized;
            }
            else
            {
                normal = (linePoints[i] - linePoints[i - 1]);
            }
            normal = normal.normalized;
            normal = new Vector2(normal.y, -normal.x).normalized;

            Vector2 pointOffset = normal * GetPointWidth(i) * 0.5f;// * widthAtPoint.x;
            colliderPathPoints.Add((Vector2)linePoints[i] + pointOffset);
        }

        // 更新PolygonCollider的路径
        polygonCollider.SetPath(0, colliderPathPoints.ToArray());
    }
}

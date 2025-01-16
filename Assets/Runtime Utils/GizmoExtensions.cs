using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GizmoExtensions
{
    static int circleResolution = 32;

    static public void DrawWireCircle(Vector3 position, Vector3 normal, float radius)
    {
        normal.Normalize();

        Vector3 orthogonalVector = normal == Vector3.up ? Vector3.right : Vector3.Cross(normal, Vector3.up);

        DrawArc(position, normal, radius, orthogonalVector, 360f);
    }

    static public void DrawArc(Vector3 position, Vector3 normal, float radius, Vector3 direction, float angle, float offset = -0.5f)
    {
        DrawArc2(position, normal, radius, direction, angle*offset, angle*(1+offset));
    }

    static public void DrawArc2(Vector3 position, Vector3 normal, float radius, Vector3 direction, float angle1, float angle2)
    {
        angle2 = angle2 < angle1 ? angle2 + 360f : angle2;
        Vector3[] points = new Vector3[Mathf.CeilToInt(circleResolution * Mathf.Abs(angle2-angle1) / 360)+1];
        for (int i = 0; i < points.Length; ++i)
        {
            points[i] = position + radius * (Quaternion.AngleAxis(Mathf.Lerp(angle1, angle2, (float)i/(points.Length-1)), normal) * direction);
        }
        Gizmos.DrawLineStrip(points, false);
    }

    static public void DrawWireWedge (Vector3 position, Vector3 normal, float radius, Vector3 direction, float angle, float offset = -0.5f)
    {
        DrawWireWedge2(position, normal, radius, direction, angle*offset, angle*(1+offset));
    }

    static public void DrawWireWedge2(Vector3 position, Vector3 normal, float radius, Vector3 direction, float angle1, float angle2)
    {
        DrawArc2(position, normal, radius, direction, angle1, angle2);
        Gizmos.DrawLine(position, position + radius * (Quaternion.AngleAxis(angle1, normal) * direction));
        Gizmos.DrawLine(position, position + radius * (Quaternion.AngleAxis(angle2, normal) * direction));
    }
}

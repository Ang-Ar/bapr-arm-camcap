using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

[ExecuteAlways]
public class Cable : MonoBehaviour
{
    [SerializeField] Transform anchorStart;
    [SerializeField] float startWeight = 0f;
    [SerializeField] Transform anchorEnd;
    [SerializeField] float endWeight = 0f;
    [SerializeField] float weightAlignmentFactor = 1f;
    public int resolution; // resolution zero means no subdivisions, i.e. a straight line between anchors
    Vector3[] lineSegments = new Vector3[0];

    [SerializeField] bool useMidpoint = false;

#if UNITY_EDITOR
    [SerializeField] bool drawDebugGizmos;
#endif

    #region cache

    // control points auto-configured using anchor points & weights (see UpdateSegments())
    Vector3 controlStart;
    Vector3 controlEnd;
    Vector3[] splinePoints = new Vector3[0];
    
    #endregion

    private void OnValidate()
    {
        resolution = resolution < 0 ? 0 : resolution;
        weightAlignmentFactor = weightAlignmentFactor < 0f ? 0f : weightAlignmentFactor;
        UpdateSegments(ref lineSegments, resolution);
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateSegments(ref lineSegments, resolution);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSegments(ref lineSegments, resolution);
    }

    public void UpdateSegments(ref Vector3[] segments, int resolution)
    {
        // alignment is based on similarity of (a) tangent given by control point and (b) direction to other end.
        // range (0, 1) from total misalignment to total alignment
        float startAlignment = Mathf.InverseLerp(-1f, 1f, Vector3.Dot(anchorStart.up, (anchorEnd.position - anchorStart.position).normalized));
        float endAlignment = Mathf.InverseLerp(-1f, 1f, Vector3.Dot(anchorEnd.up, (anchorStart.position - anchorEnd.position).normalized));

        // compute control point positions
        // the more misaligned, the more compensation
        controlStart = anchorStart.position + anchorStart.up * startWeight * Mathf.Lerp(weightAlignmentFactor, 1, startAlignment);
        controlEnd = anchorEnd.position + anchorEnd.up * endWeight * Mathf.Lerp(weightAlignmentFactor, 1, endAlignment);

        // compute spline points (first and last point are on-curve)
        if (splinePoints.Length != 4) splinePoints = new Vector3[] {anchorStart.position, controlStart, controlEnd, anchorEnd.position };

        // midpoint to be used for jiggle etc
        if (useMidpoint)
        {
            splinePoints = new Vector3[] { anchorStart.position, controlStart, EvaluateAt(0.5f), controlEnd, anchorEnd.position };
        }

        if (segments.Length-2 != resolution)
        {
            segments = new Vector3[resolution+2];
        }

        for (int i=0; i<resolution+2; i++)
        {
            float curveParameter = Mathf.InverseLerp(0, resolution+1, i);
            segments[i] = EvaluateAt(curveParameter);
        }
    }

    public Vector3 EvaluateAt(float parameter)
    {
        return interpolateSegment(splinePoints, parameter);
    }

    public Vector3 interpolateSegment(Vector3[] points, float parameter)
    {
        // first and last points of the list act as anchors, the rest as control points
        // (ergo more points = higher degree of interpolation)

        points = (Vector3[])points.Clone(); // not sure if performant. But then, neither is this entire function

        for (int degree = 1; degree < points.Length; degree++)
        {
            for (int i = 0; i < points.Length-degree; i++)
            {
                points[i] = Vector3.Lerp(points[i], points[i+1], parameter);
            }
        }

        return points[0];
    }

    public Vector3 interpolateCubic(Vector3 anchorA, Vector3 controlA, Vector3 controlB, Vector3 anchorB, float parameter)
    {
        // not a performant implementation but a very conceptually clear one

        // first degree interpolations
        Vector3 startInfluence = Vector3.Lerp(anchorA, controlA, parameter);
        Vector3 midInfluence = Vector3.Lerp(controlA, controlB, parameter);
        Vector3 endInfluence = Vector3.Lerp(controlB, anchorB, parameter);

        // second degree interpolations
        Vector3 segmentOneInfluence = Vector3.Lerp(startInfluence, midInfluence, parameter);
        Vector3 segmentTwoInfluence = Vector3.Lerp(midInfluence, endInfluence, parameter);

        // third degree interpolations
        return Vector3.Lerp(segmentOneInfluence, segmentTwoInfluence, parameter);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        for (int i = 0; i < lineSegments.Length - 1; i++)
        {
            Gizmos.DrawLine(lineSegments[i], lineSegments[i + 1]);
        }

        if (!drawDebugGizmos) return;

        for (int i = 0; i < lineSegments.Length; i++)
        {
            Gizmos.DrawSphere(lineSegments[i], 0.01f);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(anchorStart.position, controlStart);
        Gizmos.DrawLine(anchorEnd.position, controlEnd);

        foreach (Vector3 point in splinePoints)
        {
            Gizmos.DrawSphere(point, 0.01f);
        }
    }
#endif // UNITY_EDITOR
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Cable : MonoBehaviour
{
    [SerializeField] Transform anchorStart;
    [SerializeField] float startWeight = 0f;
    [SerializeField] Transform anchorEnd;
    [SerializeField] float endWeight = 0f;
    public int resolution; // resolution zero means no subdivisions, i.e. a straight line between anchors
    Vector3[] lineSegments;

#if UNITY_EDITOR
    [SerializeField] bool drawDebugGizmos;
#endif

    // control p[oints auto-configured using anchor points & weights (see UpdateSegments())
    Vector3 controlStart;
    Vector3 controlEnd;

    private void OnValidate()
    {
        resolution = resolution < 0 ? 0 : resolution;
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
        controlStart = anchorStart.position + anchorStart.up * startWeight;
        controlEnd = anchorEnd.position + anchorEnd.up * endWeight;

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
        Vector3 startInfluence = Vector3.Lerp(anchorStart.position, controlStart, parameter);
        Vector3 midInfluence = Vector3.Lerp(controlStart, controlEnd, parameter);
        Vector3 EndInfluence = Vector3.Lerp(controlEnd, anchorEnd.position, parameter);

        Vector3 segmentOneInfluence = Vector3.Lerp(startInfluence, midInfluence, parameter);
        Vector3 segmentTwoInfluence = Vector3.Lerp(midInfluence, EndInfluence, parameter);

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
    }
#endif // UNITY_EDITOR
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Cable : MonoBehaviour
{
    [SerializeField] Transform anchorStart;
    float startWeight = 0f;
    [SerializeField] Transform anchorEnd;
    float endWeight = 0f;
    public int resolution; // resolution zero means no subdivisions, i.e. a straight line between anchors
    Vector3[] lineSegments;

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
        return Vector3.Lerp(anchorStart.position, anchorEnd.position, parameter);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        for(int i=0; i<lineSegments.Length; i++)
        {
            Gizmos.DrawSphere(lineSegments[i], 0.01f);
            if (i < lineSegments.Length-1)
            {
                Gizmos.DrawLine(lineSegments[i], lineSegments[i + 1]);
            }
        }
    }
#endif // UNITY_EDITOR
}

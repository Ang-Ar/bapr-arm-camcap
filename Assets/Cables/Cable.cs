using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

[ExecuteAlways]
public class Cable : MonoBehaviour
{
    public enum RenderMode
    {
        mesh,
        line,
    }

    [SerializeField] Transform anchorStart;
    [SerializeField] float startWeight = 0f;
    [SerializeField] Transform anchorEnd;
    [SerializeField] float endWeight = 0f;

    [SerializeField] float weightAlignmentFactor = 1f;
    [SerializeField] bool useSpring = false;

    [SerializeField] SpringFilter spring;

    [Space]
    public RenderMode displayMode = RenderMode.mesh;
    public float radius = 0.1f;
    public int resolution = 20; // resolution zero means no subdivisions, i.e. a straight line between anchors
    public int sides = 3;
    public bool capped = false;

    [SerializeField] MeshFilter meshFilter;
    [SerializeField] LineRenderer lineRenderer;


    Spline finalSpline = new Spline(3);

    private void OnValidate()
    {
        resolution = resolution < 0 ? 0 : resolution;
        weightAlignmentFactor = weightAlignmentFactor < 0f ? 0f : weightAlignmentFactor;

        UpdateSplines();
        UpdateVisuals();
    }

    // Start is called before the first frame update
    void Start()
    {
        meshFilter.mesh = new Mesh();
        finalSpline.SetTangentMode(TangentMode.Broken);

        UpdateSplines();
        UpdateVisuals();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSplines();
        UpdateVisuals();
    }

    public void UpdateSplines()
    {
        // alignment is based on similarity of (a) tangent given by control point and (b) direction to other end.
        // range (0, 1) from total misalignment to total alignment
        float startAlignment = Mathf.InverseLerp(-1f, 1f, Vector3.Dot(anchorStart.up, (anchorEnd.position - anchorStart.position).normalized));
        float endAlignment = Mathf.InverseLerp(-1f, 1f, Vector3.Dot(anchorEnd.up, (anchorStart.position - anchorEnd.position).normalized));

        // compute tangents (used to set bezier handles)
        // the more misaligned, the more pronounced the tangents (as if cable resists bending)
        Vector3 startTangent = anchorStart.up * startWeight * Mathf.Lerp(weightAlignmentFactor, 1, startAlignment);
        Vector3 endTangent = anchorEnd.up * endWeight * Mathf.Lerp(weightAlignmentFactor, 1, endAlignment);

        // make a single bezier curve segment from start to end point
        BezierCurve baseCurve = BezierCurve.FromTangent(anchorStart.position, startTangent, anchorEnd.position, endTangent);

        // if not using midpoint for physics, the one segment is enough and we are done
        if (! useSpring)
        {
            finalSpline.Knots = KnotsFromCurves(new BezierCurve[] { baseCurve });
            return;
        }

        // split bezier curve into two parts
        BezierCurve firstCurve;
        BezierCurve secondCurve;
        CurveUtility.Split(baseCurve, 0.5f, out firstCurve, out secondCurve);

        // combine both segments into a single spline
        finalSpline.Knots = KnotsFromCurves(new BezierCurve[] { firstCurve, secondCurve });

        // simulate spring physics on center knot
        // SpringFilter internally stores velocity & position of the final knot
        // all we need to do is feed it a new "target position" (where the virual spring is attached)
        BezierKnot centerKnot = finalSpline[1];
        centerKnot.Position = spring.Filter(finalSpline[1].Position);
        finalSpline[1] = centerKnot;
    }

    public void UpdateVisuals()
    {
        if (displayMode == RenderMode.mesh && meshFilter != null)
        {
            SplineMesh.Extrude(finalSpline, meshFilter.mesh, radius, sides, resolution+1, capped);
        }
        else
        {
            meshFilter.mesh.Clear();
        }

        if (displayMode == RenderMode.line && lineRenderer != null)
        {
            lineRenderer.positionCount = resolution+2;
            lineRenderer.SetPositions(LineSegmentsFromSpline(finalSpline, resolution));
            lineRenderer.startWidth = radius*2;
            lineRenderer.endWidth = radius*2;
        }
        else
        {
            lineRenderer.positionCount = 0;
        }
    }

    static public IEnumerable<BezierKnot> KnotsFromCurves(IList<BezierCurve> curves)
    {
        BezierKnot[] knots = new BezierKnot[curves.Count+1];
        for (int i = 0; i<curves.Count; i++)
        {
            knots[i].Position = curves[i].P0;
            knots[i].TangentOut = curves[i].Tangent0;
            knots[i+1].TangentIn = curves[i].Tangent1;
        }
        knots[knots.Length-1].Position = curves[curves.Count-1].P3;
        return knots;
    }

    static public Vector3[] LineSegmentsFromSpline(ISpline spline, int resolution)
    {
        Vector3[] result = new Vector3[resolution + 2];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = SplineUtility.EvaluatePosition(spline, i / (result.Length - 1f));
        }
        return result;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Vector3[] segmentPts = LineSegmentsFromSpline(finalSpline, resolution);
        for (int i = 0; i < segmentPts.Length - 1; i++)
        {
            Gizmos.DrawLine(segmentPts[i], segmentPts[i + 1]);
        }

        Gizmos.color = Color.yellow;
        foreach(BezierKnot knot in finalSpline)
        {
            Gizmos.DrawSphere(knot.Position, 0.01f);
            Gizmos.DrawRay(knot.Position, knot.TangentIn);
            Gizmos.DrawRay(knot.Position, knot.TangentOut);
        }
    }
#endif // UNITY_EDITOR
}

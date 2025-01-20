using BlenderConstraints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeLimiter : SimpleConstraintExtension
{
    [HideInInspector] public Quaternion coneAxis;
    [Range(0, 180)] public float minAngle = 0f;
    [Range(0, 180)] public float maxAngle = 180f;

    public override void ApplyConstraint()
    {
        float angle;
        Vector3 axis;
        (Quaternion.Inverse(coneAxis) * constrained.localRotation).ToAngleAxis(out angle, out axis);
        angle = Mathf.Clamp(angle, minAngle, maxAngle);
        constrained.localRotation = coneAxis * Quaternion.AngleAxis(angle, axis);

        Quaternion.Angle(coneAxis, constrained.localRotation);
    }

    void OnDrawGizmos()
    {
        if (!enabled || !gameObject.activeInHierarchy) return;

        Vector3 coneX = constrained.parent.rotation * coneAxis * Vector3.right;
        Vector3 coneY = constrained.parent.rotation * coneAxis * Vector3.up;
        Vector3 coneZ = constrained.parent.rotation * coneAxis * Vector3.forward;
        float smallAngle = Mathf.Min(minAngle, maxAngle);
        float LargeAngle = Mathf.Max(minAngle, maxAngle);
        float radius = 0.075f;

        Gizmos.color = Color.white;
        //Gizmos.DrawLine(constrained.position, constrained.position + coneY * radius);
        GizmoExtensions.DrawArc2(constrained.position, coneX, radius, coneY, smallAngle, LargeAngle);
        GizmoExtensions.DrawArc2(constrained.position, -coneX, radius, coneY, smallAngle, LargeAngle);
        GizmoExtensions.DrawArc2(constrained.position, coneZ, radius, coneY, smallAngle, LargeAngle);
        GizmoExtensions.DrawArc2(constrained.position, -coneZ, radius, coneY, smallAngle, LargeAngle);
        GizmoExtensions.DrawWireCircle(constrained.position + coneY*Mathf.Cos(Mathf.Deg2Rad * smallAngle) * radius, coneY, Mathf.Sin(Mathf.Deg2Rad* smallAngle) * radius);
        GizmoExtensions.DrawWireCircle(constrained.position + coneY * Mathf.Cos(Mathf.Deg2Rad * LargeAngle) * radius, coneY, Mathf.Sin(Mathf.Deg2Rad * LargeAngle) * radius);
    }
}

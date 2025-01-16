using BlenderConstraints;
using System;
using UnityEngine;

public class JointLimits : SimpleConstraintExtension
{
    [HideInInspector] public Quaternion restPose;
    Vector3 GlobalXAtRest => constrained.parent.TransformDirection(restPose * Vector3.right);
    Vector3 GlobalYAtRest => constrained.parent.TransformDirection(restPose * Vector3.up);
    Vector3 GlobalZAtRest => constrained.parent.TransformDirection(restPose * Vector3.forward);

    public EulerAxisOrder axisOrder = EulerAxisOrder.YXZ;
    [Range(-180, 180)] public float xMin, xMax;
    [Range(-180, 180)] public float yMin, yMax;
    [Range(-180, 180)] public float zMin, zMax;

    public override void ApplyConstraint()
    {
        var axisIndices = AxisUtils.axisIndexFromEnum[(int)axisOrder];

        // convert current pose (relative to rest pose) to custom eulers
        Vector3 customEulers = AxisUtils.QuaternionToCustomEulers(Quaternion.Inverse(restPose) * constrained.localRotation, axisOrder);
        // clamp individual components
        customEulers[axisIndices[0]] = Mathf.Deg2Rad * ClampAngle180(Mathf.Rad2Deg * customEulers[axisIndices[0]], xMin, xMax);
        customEulers[axisIndices[1]] = Mathf.Deg2Rad * ClampAngle180(Mathf.Rad2Deg * customEulers[axisIndices[1]], yMin, yMax);
        customEulers[axisIndices[2]] = Mathf.Deg2Rad * ClampAngle180(Mathf.Rad2Deg * customEulers[axisIndices[2]], zMin, zMax);
        // convert back to local rotation
        constrained.localRotation = restPose * AxisUtils.CustomEulersToQuaternion(customEulers, axisOrder);
    }

    void OnDrawGizmos()
    {
        // TODO: rotate each wedge according to the euler angles before it (gives a more accurate impression while rotating constrained bone)

        var axisIndices = AxisUtils.axisIndexFromEnum[(int)axisOrder];
        Vector3 customEulers = AxisUtils.QuaternionToCustomEulers(Quaternion.Inverse(restPose) * constrained.localRotation, axisOrder);

        Gizmos.color = Color.red;
        GizmoExtensions.DrawWireWedge2(constrained.position, GlobalXAtRest, 0.1f, GlobalYAtRest, xMin, xMax);

        Gizmos.color = Color.green;
        GizmoExtensions.DrawWireWedge2(constrained.position, GlobalYAtRest, 0.05f, GlobalZAtRest, yMin, yMax);

        Gizmos.color = Color.blue;
        GizmoExtensions.DrawWireWedge2(constrained.position, GlobalZAtRest, 0.1f, GlobalYAtRest, zMin, zMax);
    }

    // clamps an angle to a looping range of [-180, 180] (assuming to and from are already in that range)
    static float ClampAngle180(float value, float from, float to)
    {
        // wrap to natural [0, 360) range
        value %= 180f;
        if (to < from)
        {
            value = value < (from + to) / 2 ? value + 360f : value;
            to += 360f;
        }
        return Mathf.Clamp(value, from, to);
    }
}

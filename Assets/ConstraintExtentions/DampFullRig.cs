using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class DampFullRig : SimpleConstraintExtension
{
    public int count = 10;

    BoneDamper[] dampingData;
    public override void ApplyConstraint()
    {
        foreach (var damper in dampingData)
        {
            damper.rotationBuffer.Add(damper.subject.localRotation);
            damper.subject.localRotation = Quaternion.Lerp(damper.subject.localRotation, damper.Mean, weight);
        }
    }

    private void OnValidate()
    {
        if (count < 1)
        {
            Debug.LogError($"count must be at least 1 (was set to {count})");
            count = 1;
        }
        dampingData = BuildDampingData(constrained, count).ToArray();
    }

    private void Start()
    {
        dampingData = BuildDampingData(constrained, count).ToArray();
    }

    private List<BoneDamper> BuildDampingData(Transform root, int count)
    {
        List<BoneDamper> dampingData = new();
        dampingData.Add(new BoneDamper { subject=root, rotationBuffer=new CircularBuffer<Quaternion>(count, root.localRotation)});
        foreach(Transform child in root)
        {
            dampingData.AddRange(BuildDampingData(child, count));
        }
        return dampingData;
    }
}

struct BoneDamper
{
    public Transform subject;
    public CircularBuffer<Quaternion> rotationBuffer;

    public Quaternion Mean
    {
        get
        {
            // turns out a component-wise mean is a decent approximation for the average of n quaternions with similar orientations
            // sources:
            // https://math.stackexchange.com/questions/61146/averaging-quaternions
            Quaternion result = new Quaternion { x=0, y=0, z=0, w=0};
            foreach (Quaternion rotation in rotationBuffer)
            {
                result.x += rotation.x;
                result.y += rotation.y;
                result.z += rotation.z;
                result.w += rotation.w;
            }
            return result.normalized;
        }
    }
}

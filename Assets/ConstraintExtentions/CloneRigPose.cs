using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CloneRigPose : SimpleConstraintExtension
{
    public enum InclusionMode { whitelist, blacklist }

    [Space]
    public bool clonePosition = true;
    public bool cloneRotation = true;
    public bool cloneScale = true;

    [Space]
    public InclusionMode mode = InclusionMode.blacklist;
    public Transform[] exceptions = new Transform[0];

    BonePair[] boneMapping;

    public override void ApplyConstraint()
    {
        foreach (BonePair pair in boneMapping)
        {
            pair.constrainedBone.localPosition = Vector3.Lerp(pair.constrainedBone.localPosition, pair.targetBone.localPosition, clonePosition? weight : 1f);
            pair.constrainedBone.localRotation = Quaternion.Lerp(pair.constrainedBone.localRotation, pair.targetBone.localRotation, cloneRotation ? weight : 1f);
            pair.constrainedBone.localScale = Vector3.Lerp(pair.constrainedBone.localScale, pair.targetBone.localScale, cloneScale ? weight : 1f);
        }
    }

    private void OnValidate()
    {
        boneMapping = BuildBoneMapping(constrained, target).ToArray();
    }

    private void Start()
    {
        boneMapping = BuildBoneMapping(constrained, target).ToArray();
    }
    
    List<BonePair> BuildBoneMapping(Transform constrained, Transform target)
    {
        List<BonePair> boneMapping = new();

        bool isException = Array.FindIndex(exceptions, t => t == constrained || t == target) >= 0;
        if ((mode == InclusionMode.whitelist && isException) || (mode == InclusionMode.blacklist && !isException))
        {
            boneMapping.Add(new BonePair { constrainedBone = constrained, targetBone = target });
        }

        Debug.Assert(constrained.childCount == target.childCount,
            $"child count mismatch between \"{constrained.gameObject.name}\" and \"{target.gameObject.name}\"\n");
        for (int i = 0; i < constrained.childCount; i++)
        {
            boneMapping.AddRange(BuildBoneMapping(constrained.GetChild(i), target.GetChild(i)));
        }
        return boneMapping;
    }
}

struct BonePair
{
    public Transform constrainedBone;
    public Transform targetBone;
}

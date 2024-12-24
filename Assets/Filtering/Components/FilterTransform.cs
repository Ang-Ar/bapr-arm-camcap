using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilterTransform : MonoBehaviour
{
    [SerializeField] Transform constrained;
    [SerializeField] Transform target;

    [SerializeField] VectorFilterAsset positionFilterAsset;

    IVectorFilter positionFilter;

    // Start is called before the first frame update
    void Start()
    {
        positionFilter = positionFilterAsset.GetVectorFilter();
        positionFilter.ClearInputbuffer(target.position);
        positionFilter.ClearOutputbuffer(constrained.position);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        constrained.position = positionFilter.Filter(target.position);
    }
}

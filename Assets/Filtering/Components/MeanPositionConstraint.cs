using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeanPositionConstraint : MonoBehaviour
{
    [SerializeField] Transform constrained;
    [SerializeField] Transform target;
    [SerializeField] MeanVectorFilter filter;

    // Start is called before the first frame update
    void Start()
    {
        // need to re-initialise filter to create buffer since it isn't serialised
        filter = new MeanVectorFilter(filter.settings);
        filter.ClearInputbuffer(target.position);
        filter.ClearOutputbuffer(constrained.position);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        constrained.position = filter.Filter(target.position);
    }
}

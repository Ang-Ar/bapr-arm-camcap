using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringConstraint : MonoBehaviour
{
    [SerializeField] Transform constrained;
    [SerializeField] Transform target;
    [SerializeField] SpringFilter filter;

    // Start is called before the first frame update
    void Start()
    {
        filter.ClearInputbuffer(target.position);
        filter.ClearOutputbuffer(constrained.position);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        constrained.position = filter.Filter(target.position);
    }
}

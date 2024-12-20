using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpringFilter : IVectorFilter
{
    public float mass = 1f;
    public float springLength = 0f;
    public float springConstant = 1f;
    public float dampingFactor = 0.5f;
    public Vector3 gravityConstant = Vector3.zero;


    VectorFilterData data;

    SpringFilter()
    {
        data = new VectorFilterData(0, 2);
    }

    public Vector3 Filter(Vector3 measurement)
    {
        data.AddMeasurement(measurement);
        Vector3 position = data.GetResult(1);
        Vector3 velocity = data.GetVelocity(1, recursive: true);

        Vector3 springForce = (springLength - Vector3.Distance(data.GetMeasurement(0), data.GetResult(1)))
            * springConstant
            * (measurement - position).normalized;

        Vector3 dampingForce = velocity * dampingFactor;

        Vector3 gravityForce = mass * gravityConstant;

        velocity += springForce + dampingForce + gravityForce;
        position += velocity;

        data.AddResult(position);
        return position;
    }
}

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

    public SpringFilter()
    {
        data = new VectorFilterData(0, 2);
    }

    public void ClearInputbuffer(Vector3 value)
    {
        data.ClearMeasurementbuffer(value);
    }

    public void ClearOutputbuffer(Vector3 value)
    {
        data.ClearResultBuffer(value);
    }

    public Vector3 Filter(Vector3 measurement)
    {
        data.AddMeasurement(measurement);
        Vector3 position = data.GetResult(1);
        Vector3 velocity = data.GetVelocity(1, recursive: true);

        Vector3 springForce = (Vector3.Distance(measurement, position) - springLength)
            * springConstant
            * (measurement - position).normalized;

        Vector3 gravityForce = mass * gravityConstant;

        velocity += (springForce + gravityForce) / mass;

        // calculate damping after updating velocity for more accurate behaviour
        Vector3 dampingForce = velocity * dampingFactor;
        // damping force ignores mass for simplicity (easier to configure in-editor)
        velocity -= dampingForce;

        position += velocity;

        data.AddResult(position);
        return position;
    }
}

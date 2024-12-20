using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVectorFilter: IFilter<Vector3, Vector3>
{
    
}

public class VectorFilterData: FilterData<Vector3, Vector3>
{
    public VectorFilterData(int inputDegree, int outputDegree): base(inputDegree, outputDegree, Vector3.zero, Vector3.zero) { }
    public VectorFilterData(int inputDegree, int outputDegree, Vector3 initial) : base(inputDegree, outputDegree, initial, initial) { }
    public VectorFilterData(int inputDegree, int outputDegree, Vector3 initialMeasurment, Vector3 initialResult) : base(inputDegree, outputDegree, initialMeasurment, initialResult) { }

    // extremely basic velocity approximation, may add something better in the future. Requires degree (delay+1).
    public Vector3 GetVelocity(int delay)
    {
        return GetMeasurement(delay) - GetMeasurement(delay + 1);
    }

    // extremely basic acceleration approximation, may add something better in the future. Requires degree (delay+2).
    public Vector3 GetAcceleration(int delay)
    {
        // logically acceleration is the change in velocity:
        //   return GetVelocity(delay) - GetVelocity(delay+1);
        // which expands to (assuming delay = 0 and denoting a value n measurements into the past as xn)
        //   (x0 - x1) - (x1 -  x2)
        // = (x0 - 2 x1 + x2)
        return GetMeasurement(delay) - 2*GetMeasurement(delay+1) + GetMeasurement(delay+2);
    }
}

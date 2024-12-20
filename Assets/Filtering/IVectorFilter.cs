using UnityEngine;

// mostly here to enable polymorphism
public interface IVectorFilter: IFilter<Vector3, Vector3>
{
    
}

// specialisation adding Velocity & Acceleration estimates
public class VectorFilterData: FilterData<Vector3, Vector3>
{
    public VectorFilterData(int inputDegree, int outputDegree): base(inputDegree, outputDegree, Vector3.zero, Vector3.zero) { }
    public VectorFilterData(int inputDegree, int outputDegree, Vector3 initial) : base(inputDegree, outputDegree, initial, initial) { }
    public VectorFilterData(int inputDegree, int outputDegree, Vector3 initialMeasurment, Vector3 initialResult) : base(inputDegree, outputDegree, initialMeasurment, initialResult) { }

    // extremely basic velocity approximation, may add something better in the future. Requires degree (delay+1).
    // if "recursive" (i.e. reading from previous results), delay must be at least 1
    public Vector3 GetVelocity(int delay=0, bool recursive=false)
    {
        return recursive ?
            GetResult(delay) - GetResult(delay + 1) :
            GetMeasurement(delay) - GetMeasurement(delay + 1);
    }

    // extremely basic acceleration approximation, may add something better in the future. Requires degree (delay+2).
    // if "recursive" (i.e. reading from previous results), delay must be at least 1
    public Vector3 GetAcceleration(int delay=0, bool recursive=false)
    {
        // logically acceleration is the change in velocity:
        //   return GetVelocity(delay) - GetVelocity(delay+1);
        // which expands to (assuming delay = 0 and denoting a value n measurements into the past as xn)
        //   (x0 - x1) - (x1 -  x2)
        // = (x0 - 2 x1 + x2)
        return recursive ? 
            GetResult(delay) - 2*GetResult(delay+1) + GetResult(delay+2) :
            GetMeasurement(delay) - 2*GetMeasurement(delay+1) + GetMeasurement(delay+2);
    }
}

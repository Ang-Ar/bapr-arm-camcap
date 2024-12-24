using UnityEngine;
using UnityEngine.Assertions;

[System.Serializable]
public struct SpringFilterSettings
{
    public float mass;
    public float springLength;
    public float springConstant;
    public float dampingFactor;
    public Vector3 gravityConstant;

    static public SpringFilterSettings Default()
    {
        return new SpringFilterSettings
        {
            mass = 2,
            springLength = 0,
            springConstant = 1,
            dampingFactor = .5f,
            gravityConstant = Vector3.zero,
        };
    }
}

[CreateAssetMenu(fileName = "SpringFilter", menuName = "filters/spring")]
public class SpringfilterAsset: VectorFilterAsset
{
    public SpringFilterSettings settings;

    public SpringfilterAsset()
    {
        settings = SpringFilterSettings.Default();
    }

    public override IVectorFilter GetVectorFilter()
    {
        return new SpringFilter(settings);
    }

    public override IFilter<Vector3, Vector3> GetFilter()
    {
        return GetVectorFilter();
    }
}

[System.Serializable]
public class SpringFilter : IVectorFilter
{
    [SerializeField] SpringFilterSettings settings;

    VectorFilterData data;

    public SpringFilter()
    {
        settings = SpringFilterSettings.Default();
        // need two past results to esitmate velocity of spring-damped object
        data = new VectorFilterData(0, 2);
    }

    public SpringFilter(SpringFilterSettings settings): this()
    {
        this.settings = settings;
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

        Vector3 springForce = (Vector3.Distance(measurement, position) - settings.springLength)
            * settings.springConstant
            * (measurement - position).normalized;

        Vector3 gravityForce = settings.mass * settings.gravityConstant;

        velocity += (springForce + gravityForce) / settings.mass;

        // calculate damping after updating velocity for more accurate behaviour
        Vector3 dampingForce = velocity * settings.dampingFactor;
        // damping force ignores mass for simplicity (easier to configure in-editor)
        velocity -= dampingForce;

        position += velocity;

        data.AddResult(position);
        return position;
    }
}

using UnityEngine;
using UnityEngine.Assertions;

[System.Serializable]
public struct MeanFilterSettings
{
    public int itemCount;

    static public MeanFilterSettings Default()
    {
        return new MeanFilterSettings
        {
            itemCount = 10
        };
    }
}

[CreateAssetMenu(fileName = "MeanFilter", menuName = "filters/mean")]
public class MeanfilterAsset : VectorFilterAsset
{
    public MeanFilterSettings settings;

    public MeanfilterAsset()
    {
        settings = MeanFilterSettings.Default();
    }

    public override IVectorFilter GetVectorFilter()
    {
        return new MeanVectorFilter(settings);
    }

    public override IFilter<Vector3, Vector3> GetFilter()
    {
        return GetVectorFilter();
    }
}

[System.Serializable]
public class MeanVectorFilter : IVectorFilter
{
    public MeanFilterSettings settings;

    VectorFilterData data;

    public MeanVectorFilter(MeanFilterSettings settings)
    {
        Assert.IsFalse(settings.itemCount <= 0);
        this. settings = settings;
        // n items = current item + (n - 1) pat items => degree (n - 1)
        data = new VectorFilterData(inputDegree: settings.itemCount - 1, outputDegree: 0);
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
        // will create a resized position buffer if needed and copy past measurements from old to new
        ResizeBuffer(settings.itemCount);

        data.AddMeasurement(measurement);
        Vector3 sum = Vector3.zero;
        for (int i = 0; i < settings.itemCount; i++)
        {
            sum += data.GetMeasurement(i);
        }
        return sum / settings.itemCount;
    }

    private void ResizeBuffer(int newItemCount)
    {
        // stop if no need to resize the buffer
        if (data.MeasurementDegree == newItemCount) return;

        // if new buffer is larger than old one, unkown values will be filled with the oldest known measurement
        Vector3 oldestMeasurement = data.GetMeasurement(data.MeasurementDegree);
        VectorFilterData newBuffer = new VectorFilterData(newItemCount, 0, initial: oldestMeasurement);

        // copy all known values from old buffer to new until we reach the end of either one
        for (int i = 0; i <= data.MeasurementDegree && i <= newBuffer.MeasurementDegree; i++)
        {
            newBuffer.AddMeasurement(data.GetMeasurement(i));
        }

        data = newBuffer;
    }
}


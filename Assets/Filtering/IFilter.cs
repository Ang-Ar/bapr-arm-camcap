using UnityEngine.Assertions;

public interface IFilter<T, U>
{
    public void ClearInputbuffer(T value);
    public void ClearOutputbuffer(U value);
    public U Filter(T measurement);
}

/// <summary>
/// Stores a history of the past n input measurements (+ the current one) and m filtered values
/// where n = inputDegree and m = outputDegree (see constructor)
/// 
/// workflow after initialisation:
/// 1. call AddMeasurement(measurement)
///     whenever you get new data to be filtered
/// 2. call GetMeasurement(delay) and GetResult(delay)
///     to read historical values.
///     delay of 0 = current measurement (corresponding result not yet available)
/// 3. call AddResult(result)
///     to register the filtered result into history
/// 
/// warning: this class does NOT check for out-of-bounds indices and throws no errors if they do occur!
/// 
/// </summary>
/// <typeparam name="T"> the type of measured data </typeparam>
/// /// <typeparam name="U"> the type of resulting (filtered) data </typeparam>
public class FilterData<T, U>
{
    CircularBuffer<T> measurementHistory;
    CircularBuffer<U> resultHistory;

    // input degree n: store n past measurements (in addition to the current one)
    // output degree m: store m past results (m-th result is forgotten as soon as new result is registered)
    public FilterData(int inputDegree, int outputDegree)
    {
        measurementHistory = new CircularBuffer<T>(inputDegree+1);
        resultHistory = new CircularBuffer<U>(outputDegree);
    }

    public FilterData(int inputDegree, int outputDegree, T initialMeasurements, U initialResults)
    {
        measurementHistory = new CircularBuffer<T>(inputDegree+1, initialMeasurements);
        resultHistory = new CircularBuffer<U>(outputDegree, initialResults);
    }

    // call before GetMeasurement() whenever you get new data to ensure delay is accurate
    public void AddMeasurement(T measurement) => measurementHistory.Add(measurement);

    public T GetMeasurement(int delay) =>  measurementHistory[delay];

    public void AddResult(U result) => resultHistory.Add(result);

    // call after all calculations for the corresponding measurement to ensure delay is correct
    public U GetResult(int delay) => resultHistory[delay - 1];

    public void ClearMeasurementbuffer(T value)
    {
        measurementHistory.Clear(value);
    }

    public void ClearResultBuffer(U value)
    {
        resultHistory.Clear(value);
    }
}

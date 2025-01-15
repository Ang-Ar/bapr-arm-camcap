using System;
using UnityEngine;
using System.Linq;

public class SimpleFPSTool : MonoBehaviour
{
    [Tooltip("How often FPS display is updated (frames). Does not affect game at all.")]
    [Range(1, 100)]
    [SerializeField] int UIRefresh = 10;

    [Tooltip("-1 for unlimited / default system behavior")]
    [SerializeField] int targetFPS = -1;

    float savedAvg = 0;
    float[] pastMeasurements;
    int measurementCycle = 0;

    private void OnValidate()
    {
        UIRefresh = Math.Max(0, UIRefresh);
        targetFPS = (targetFPS == -1) ? -1 : Math.Max(0, targetFPS);
    }

    private void Awake()
    {
        pastMeasurements = new float[UIRefresh];
        Application.targetFrameRate = targetFPS;
    }

    void Update()
    {
        if (measurementCycle >= pastMeasurements.Length)
        {
            savedAvg = pastMeasurements.Aggregate(0f, (agg, item) => agg+item) / pastMeasurements.Length;
            measurementCycle = 0;
        }
        pastMeasurements[measurementCycle] = Time.deltaTime;
        measurementCycle++;
    }

    void OnGUI()
    {
        GUILayout.Label($"Current FPS: {MathF.Round(1f/savedAvg)}");
        GUILayout.Label($"Target FPS: {Application.targetFrameRate}");
        GUILayout.Label($"Vsync: {QualitySettings.vSyncCount}");
        GUILayout.Label($"Display refresh rate: {Math.Round(Screen.currentResolution.refreshRateRatio.value)}");
    }
}

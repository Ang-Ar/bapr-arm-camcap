using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class MainThreadUtil : MonoBehaviour
{
    public static MainThreadUtil Instance { get; set; }
    public static SynchronizationContext synchronizationContext { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    [ExecuteInEditMode]
    public static void Setup()
    {
        Instance = GameObject.FindObjectOfType<MainThreadUtil>();
        synchronizationContext = SynchronizationContext.Current;
    }
    //[ExecuteInEditMode]


    public static void Run(IEnumerator waitForUpdate)
    {
        try
        {
            synchronizationContext.Post(_ => Instance.StartCoroutine(
                        waitForUpdate), null);
        }
        catch { }
    }

    void Awake()
    {
        Instance = GameObject.FindObjectOfType<MainThreadUtil>();
        gameObject.hideFlags = HideFlags.HideAndDontSave; //this causes the radical UI to be grayed out even after exiting play mode, until you click on the controller again
        DontDestroyOnLoad(gameObject);
    }
}

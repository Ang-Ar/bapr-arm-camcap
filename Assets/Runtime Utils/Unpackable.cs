using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// moves all children to top level in hierarchy
// good for enabling parallel transform calculations while keeping an organised scene or using prefabs
public class Unpackable : MonoBehaviour
{
    public enum UnpackTime
    {
        [Tooltip("auto-unpack during Awake()")]
        always,
        [Tooltip("auto-unpack during Awake() in build only")]
        build,
        [Tooltip("manually unpack from script")]
        never,
    }

    public enum UnpackMode
    {
        [Tooltip("move children to the top of the transform hierarchy")]
        top,
        [Tooltip("parent children to this object's parent")]
        single,
    }

    [SerializeField] UnpackTime autoUnpack = UnpackTime.build;
    public UnpackMode mode = UnpackMode.top;

    // rebuild the hierarchy in Awake, BEFORE any connections between objects are made
    private void Awake()
    {
        if (autoUnpack == UnpackTime.always || (autoUnpack == UnpackTime.build && !Application.isEditor))
            Unpack();
    }

    public void Unpack() => Unpack(mode);

    public void Unpack(UnpackMode mode)
    {
        switch (mode)
        {
            case UnpackMode.top:
                transform.DetachChildren();
                break;

            case UnpackMode.single:
                // save static copy of the children list (modify-during-loop = bad idea)
                Transform[] children = transform.Cast<Transform>().ToArray();
                foreach (Transform child in children)
                {
                    child.parent = transform.parent;
                }
                break;
        }
        Destroy(this.gameObject);
    }
}

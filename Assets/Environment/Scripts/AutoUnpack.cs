using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Use to group objects in inspector but have independent transform hierarchies at run time
// good for enabling parallel transform calculations by making GameObjects top-level in the hierarchy
public class AutoUnpack : MonoBehaviour
{

    // rebuild the hierarchy in Awake(), i.e. BEFORE any connections between objects are made
    private void Awake()
    {
        //foreach(Transform child in transform)
        //{
        //    child.transform.parent = transform.parent;
        //}
        //Destroy(this.gameObject);
    }
}

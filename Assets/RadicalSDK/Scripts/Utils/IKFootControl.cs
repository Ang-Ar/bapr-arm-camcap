using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFootControl : MonoBehaviour
{
    Vector3 restPosition;
    // Start is called before the first frame update
    void Start()
    {
        restPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = restPosition;
    }
}

using BlenderConstraints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SimpleConstraintExtension : MonoBehaviour, IBlenderConstraintSimple
{
    #region IBlenderConstraintSimple properties
    public bool UpdateInEditMode { get => updateInEditMode; set => updateInEditMode = value; }
    
    public UpdateMode UpdateMode { get => updateMode; set => updateMode = value; }

    public MonoBehaviour MonoBehaviour => this;

    public GameObject GameObject => gameObject;

    public float Weight { get => weight; set => weight = value; }

    public Transform Constrained => constrained;

    public Transform Target => target;
    #endregion

    #region editor values
    [Range(0f, 1f)] public float weight = 1f;

    [Space]

    public Transform constrained;
    public Transform target;

    [Space]

    public bool updateInEditMode = false;
    public UpdateMode updateMode = UpdateMode.Update;
    #endregion

    #region actual logic
    void Update()
    {
        if ((updateMode == UpdateMode.Update && Application.isPlaying) || (updateMode != UpdateMode.Ordered && updateInEditMode && !Application.isPlaying))
        {
            ApplyConstraint();
        }
    }

    private void FixedUpdate()
    {
        if (updateMode == UpdateMode.FixedUpdate)
        {
            ApplyConstraint();
        }
    }

    abstract public void ApplyConstraint();

    public virtual IBlenderConstraint ConvertComponent(bool useAnimationRigging, bool updateInEditMode = true, UpdateMode updateMode = UpdateMode.Update)
    {
        throw new System.NotImplementedException();
    }
    #endregion
}

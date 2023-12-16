using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Target : MonoBehaviour
{
    public event Action<Target> OnDestroy;

    /// <summary>
    /// Executes when the target is shot.
    /// </summary>
    public abstract void OnShot();

    /// <summary>
    /// Should be called when the target is destroyed.
    /// </summary>
    protected virtual void SignalOnDestroy() 
    {
        OnDestroy?.Invoke(this);
    }
}

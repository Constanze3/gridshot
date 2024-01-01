using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Borders : VolumeComponent, IPostProcessComponent
{
    public ClampedFloatParameter strength = new ClampedFloatParameter(0.0f, 0.0f, 1.0f);

    public bool IsActive()
    {
        return (strength != 0);
    }

    public bool IsTileCompatible()
    {
        return false;
    }
}

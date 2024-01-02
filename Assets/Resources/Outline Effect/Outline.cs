using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine;

public class Outline : VolumeComponent, IPostProcessComponent
{
    public ClampedFloatParameter thickness = new(0f, 0f, 5f);
    public ClampedFloatParameter threshold = new(0.01f, 0f, 0.08f);
    public NoInterpColorParameter color = new(Color.black);

    public bool IsActive()
    {
        return (0.0f < thickness.value);
    }

    public bool IsTileCompatible()
    {
        return false;
    }
}
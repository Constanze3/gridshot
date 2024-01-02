using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine;

public class Blur : VolumeComponent, IPostProcessComponent
{
    [Tooltip("Standard deviation (spread) of the blur. Grid size is approx. 3x larger.")]
    public ClampedFloatParameter strength = new(0.0f, 0.0f, 15.0f);

    public bool IsActive()
    {
        return (0.0f < strength.value);
    }

    public bool IsTileCompatible()
    {
        return false;
    }
}
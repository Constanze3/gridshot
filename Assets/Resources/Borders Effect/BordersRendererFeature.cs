using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BordersRendererFeature : ScriptableRendererFeature
{
    public Material material;

    private BordersRenderPass bordersRenderPass;

    public override void Create()
    {
        bordersRenderPass = new BordersRenderPass();
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (bordersRenderPass.Setup(material)) {
            renderer.EnqueuePass(bordersRenderPass);
        }
    }
}

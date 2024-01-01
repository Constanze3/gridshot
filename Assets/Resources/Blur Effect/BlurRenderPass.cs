using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BlurRenderPass : ScriptableRenderPass
{
    private Blur settings;

    private RenderTargetIdentifier colorBuffer, blurBuffer;
    private int blurBufferID = Shader.PropertyToID("_BlurTex");

    private Material material;

    public bool Setup(ScriptableRenderer renderer)
    {
        settings = VolumeManager.instance.stack.GetComponent<Blur>();
        renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;

        if (settings != null && settings.IsActive())
        {
            material = new Material(Shader.Find("PostProcessing/Blur"));
            return true;
        }

        return false;
    }
    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        colorBuffer = renderingData.cameraData.renderer.cameraColorTargetHandle;
        RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;

        cmd.GetTemporaryRT(blurBufferID, descriptor, FilterMode.Bilinear);
        blurBuffer = new RenderTargetIdentifier(blurBufferID);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        CommandBuffer cmd = CommandBufferPool.Get("Blur Post Process");

        int gridSize = Mathf.CeilToInt(settings.strength.value * 6.0f);
        if (gridSize % 2 == 0)
        {
            gridSize++;
        }

        material.SetInteger("_GridSize", gridSize);
        material.SetFloat("_Spread", settings.strength.value);

        // Execute effect using effect material with two passes.
        cmd.Blit(colorBuffer, blurBuffer, material, 0);
        cmd.Blit(blurBuffer, colorBuffer, material, 1);

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

    public override void FrameCleanup(CommandBuffer cmd)
    {
        if (cmd == null) throw new System.ArgumentNullException("cmd");
        cmd.ReleaseTemporaryRT(blurBufferID);
    }
}
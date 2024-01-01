using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BordersRenderPass : ScriptableRenderPass
{
    private Borders settings;

    private RenderTargetIdentifier colorBuffer, borderBuffer;
    private int borderBufferID = Shader.PropertyToID("_BorderTex");

    private Material material;

    public bool Setup(Material material)
    {
        settings = VolumeManager.instance.stack.GetComponent<Borders>();
        renderPassEvent = RenderPassEvent.AfterRenderingTransparents;

        if (settings != null && settings.IsActive())
        {
            this.material = material;
            return true;
        }

        return false;
    }

    

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        colorBuffer = renderingData.cameraData.renderer.cameraColorTargetHandle;
        RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;

        cmd.GetTemporaryRT(borderBufferID, descriptor, FilterMode.Bilinear);
        borderBuffer = new RenderTargetIdentifier(borderBufferID);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        CommandBuffer cmd = CommandBufferPool.Get("Borders Post Process");

        cmd.Blit(colorBuffer, borderBuffer, material);
        cmd.Blit(borderBuffer, colorBuffer);

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

    public override void FrameCleanup(CommandBuffer cmd)
    {
        if (cmd == null) throw new System.ArgumentNullException("cmd");
        cmd.ReleaseTemporaryRT(borderBufferID);
    }
}

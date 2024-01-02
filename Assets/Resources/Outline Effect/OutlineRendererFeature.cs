using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OutlineRendererFeature : ScriptableRendererFeature
{
    private class OutlineRenderPass : ScriptableRenderPass
    {
        Outline settings;
        private Material material;

        private RenderTargetIdentifier colorBuffer, outlineBuffer;
        private int borderBufferID = Shader.PropertyToID("_BorderTex");

        public bool Setup(ScriptableRenderer renderer)
        {
            settings = VolumeManager.instance.stack.GetComponent<Outline>();
            renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;

            if (settings != null && settings.IsActive())
            {
                material = new Material(Shader.Find("PostProcessing/BasicOutline"));
                return true;
            }

            return false;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get("Borders Post Process");

            colorBuffer = renderingData.cameraData.renderer.cameraColorTargetHandle;
            RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;

            cmd.GetTemporaryRT(borderBufferID, descriptor, FilterMode.Bilinear);
            outlineBuffer = new RenderTargetIdentifier(borderBufferID);

            material.SetFloat("_Thickness", settings.thickness.value);
            material.SetFloat("_Threshold", settings.threshold.value);
            material.SetColor("_Color", settings.color.value);

            cmd.Blit(colorBuffer, outlineBuffer, material);
            cmd.Blit(outlineBuffer, colorBuffer);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            if (cmd == null) throw new System.ArgumentNullException("cmd");
            cmd.ReleaseTemporaryRT(borderBufferID);
        }
    }

    private OutlineRenderPass outlineRenderPass;

    public override void Create()
    {
        outlineRenderPass = new OutlineRenderPass();
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (outlineRenderPass.Setup(renderer)) renderer.EnqueuePass(outlineRenderPass);
    }
}
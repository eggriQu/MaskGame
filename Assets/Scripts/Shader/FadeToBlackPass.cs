using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class FadeToBlack : ScriptableRendererFeature
{
    public Shader FadeShader;
    public float m_FadeIntensity;
    Material m_Material;
    private FadePass m_FadePass = null;
    


    public override void Create()
    {
        m_Material = CoreUtils.CreateEngineMaterial(FadeShader);
        m_FadePass = new FadePass(m_Material);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
 
        renderer.EnqueuePass(m_FadePass);
    }

   
    
    protected override void Dispose(bool disposing)
    {
        CoreUtils.Destroy(m_Material);
    }
}

internal class FadePass : ScriptableRenderPass
{
    ProfilingSampler m_ProfilingSampler;
    Material m_Material;
    RTHandle m_Target;
    public static float m_FadeMult;
    
    


    public FadePass(Material material)
    {
        m_Material = material;
        m_ProfilingSampler = new ProfilingSampler("Fade Pass");
        renderPassEvent = RenderPassEvent.AfterRendering;
    }
   
    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        m_Target = renderingData.cameraData.renderer.cameraColorTargetHandle;

    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (m_Material == null) return;

        var cameraData = renderingData.cameraData;
        if (cameraData.camera.cameraType != CameraType.Game)
            return;

        CommandBuffer cmd = CommandBufferPool.Get();
        using (new ProfilingScope(cmd, m_ProfilingSampler))
        {
            m_Material.SetFloat("_FadeMult", m_FadeMult);
            Blitter.BlitCameraTexture(cmd, m_Target, m_Target, m_Material, 0);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);

        }
    }
}
    
        
        
        


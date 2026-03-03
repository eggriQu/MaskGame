using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;


public class FadeRendererFeature : ScriptableRendererFeature
{
    class FadePass : ScriptableRenderPass
    {
        
        private const string m_PassName = "FadePass";
        Material m_BlitMaterial;

        public void Setup(Material mat)
        {
            m_BlitMaterial = mat;
            requiresIntermediateTexture = true;
        }

        
     
        
        
        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            Debug.Log("It doin somethin at least");
            var resourceData = frameData.Get<UniversalResourceData>();
            if (resourceData.isActiveTargetBackBuffer)
            {
                Debug.Log("isActiveTargetBackBuffer");
                return;
            }

            if (m_BlitMaterial == null)
            {
                Debug.LogError("FadePass material missing!");
            }
               

            var source = resourceData.activeColorTexture;
            var destinationDesc = renderGraph.GetTextureDesc(source);
            destinationDesc.name = $"CameraColor-{m_PassName}";
            destinationDesc.clearBuffer = false;
            
            TextureHandle destination = renderGraph.CreateTexture(destinationDesc);
            
            RenderGraphUtils.BlitMaterialParameters para = new(source, destination, m_BlitMaterial,0);
            renderGraph.AddBlitPass(para, passName: m_PassName);

            Debug.Log("we doin it?");
            resourceData.cameraColor = destination;
        }
        
     
    }

    public RenderPassEvent injectionPoint = RenderPassEvent.AfterRenderingPostProcessing;
    public Material material;
    
    FadePass m_Fade;

    public override void Create()
    {
        m_Fade = new FadePass();
        m_Fade.renderPassEvent = injectionPoint;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (material == null)
        {
            Debug.LogError("FadePass material missing!");
            return;
        }
            

        m_Fade.Setup(material);
        renderer.EnqueuePass(m_Fade);
    }
}

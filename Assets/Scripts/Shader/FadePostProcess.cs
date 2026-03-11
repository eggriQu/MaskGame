using UnityEngine;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class FadePostProcess : MonoBehaviour
{
    public Shader postProcessShader; 
    Material postProcessMaterial;

    public float fadeMult;


    //[ImageEffectOpaque]
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (postProcessMaterial == null)
        {
            postProcessMaterial = new Material(postProcessShader);
        }

        RenderTexture renderTexture = RenderTexture.GetTemporary(
            src.width, //Width
            src.height, //Height
            16, //Depth buffer
            RenderTextureFormat.ARGBHalf //Format
        );

          postProcessMaterial.SetFloat("_FadeMult", fadeMult);

        Graphics.Blit(src, renderTexture, postProcessMaterial, 0);
        Graphics.Blit(renderTexture, dest);


        RenderTexture.ReleaseTemporary(renderTexture);
    }
}

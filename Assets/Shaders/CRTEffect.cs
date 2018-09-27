using UnityEngine;
using System.Collections;


public class CRTEffect : MonoBehaviour
{
    public Shader CRTShader;

    private Material mat;

    void Awake()
    {
        mat = new Material(CRTShader);
    }

    void OnDestroy()
    {
        Destroy(mat);
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        RenderTexture pass1 = RenderTexture.GetTemporary(src.width, src.height);
        Graphics.Blit(src, pass1, mat, 0); // render first pass
        Graphics.Blit(pass1, dest, mat, 1); // render second pass
        RenderTexture.ReleaseTemporary(pass1);
    }
}
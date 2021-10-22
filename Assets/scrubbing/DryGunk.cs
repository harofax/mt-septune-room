using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DryGunk : MonoBehaviour
{
    public Shader dryShader;

    private Material _dryMaterial;

    private MeshRenderer _meshRenderer;

    [Range(0.001f, 0.1f)]
    public float dryAmount;

    [Range(0f, 1f)]
    public float dryOpacity;
    // Start is called before the first frame update
    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _dryMaterial = new Material(dryShader);
    }
    
    

    // Update is called once per frame
    void Update()
    {
        _dryMaterial.SetFloat("_DryAmount", dryAmount);
        _dryMaterial.SetFloat("_DryOpacity", dryOpacity);

        RenderTexture dry = (RenderTexture) _meshRenderer.material.GetTexture("_Splat");
        RenderTexture temp = RenderTexture.GetTemporary(dry.width, dry.height, 0, RenderTextureFormat.ARGBFloat);
        Graphics.Blit(dry, temp, _dryMaterial);
        Graphics.Blit(temp, dry);
        
        _meshRenderer.material.SetTexture("_Splat", dry);
        
        RenderTexture.ReleaseTemporary(temp);
    }
}

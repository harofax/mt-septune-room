using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrubGunkAway : MonoBehaviour
{
    public Camera mainCamera;

    public Shader drawShader;

    private RenderTexture _splatmap;

    private Material _baseMaterial, _drawMaterial;

    private RaycastHit _hit;

    private LayerMask _hittableLayer;
    [Range(1, 15)]
    public float reach = 5.0f;
    
    [Range(1, 50)]
    public float brushSize;
    [Range(0, 1)]
    public float brushStrength;

    private static readonly int Coordinate = Shader.PropertyToID("_Coordinate");
    private static readonly int Strength = Shader.PropertyToID("_Strength");
    private static readonly int Size = Shader.PropertyToID("_Size");
    private static readonly int MainTex = Shader.PropertyToID("_MainTex");
    private static readonly int _Color = Shader.PropertyToID("_Color");
    private static readonly int Splat = Shader.PropertyToID("_Splat");

    // Start is called before the first frame update
    void Start()
    {
        _drawMaterial = new Material(drawShader);
        _drawMaterial.SetVector(_Color, Color.red);

        _baseMaterial = GetComponent<MeshRenderer>().material;
        _splatmap = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat);
        _baseMaterial.SetTexture(Splat, _splatmap);
        
        // environ == floors, ground, etc -- not dirt
        var environ_layer = 6;
        // gunk == stuff you clean up with a tool
        var gunk_layer = 7;

        var environLayermask = 1 << environ_layer;
        var gunkLayermask = 1 << gunk_layer;

        var finalLayermask = environLayermask | gunkLayermask;
        _hittableLayer = finalLayermask;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetMouseButton(0)) return;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out _hit, reach, _hittableLayer)) return;
        
        _drawMaterial.SetVector(Coordinate, new Vector4(_hit.textureCoord.x, _hit.textureCoord.y, 0, 0));
        _drawMaterial.SetFloat(Strength, brushStrength);
        _drawMaterial.SetFloat(Size, brushSize);
        RenderTexture temp = RenderTexture.GetTemporary(_splatmap.width, _splatmap.height, 0,
            RenderTextureFormat.ARGBFloat);
        Graphics.Blit(_splatmap, temp);
        Graphics.Blit(temp, _splatmap, _drawMaterial);
        RenderTexture.ReleaseTemporary(temp);
    }

    private void OnGUI()
    {
        GUI.DrawTexture(new Rect(0,0, 256, 256), _splatmap, ScaleMode.ScaleToFit, false, 1);
    }
}

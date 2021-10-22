using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using Random = UnityEngine.Random;

public class GunkSurface : MonoBehaviour
{
    public Shader dryShader;

    private Material _dryMaterial;

    

    private MeshRenderer _meshRenderer;
    public RenderTexture splatmap;



    private Material _baseMaterial;

    public bool randomGunk = true;
    public bool fullGunk = false;

    [Range(0f, 1f)]
    public float dryAmount;

    [Range(0f, 0.3f)]
    public float dryOpacity;

    public float dryRate = 60f;

    private IEnumerator dryRoutine;

    public float scale = 3.2f;
    
    public Vector2 splatSize;
    public float splatScale = 1.0f;
    private static readonly int DryAmount = Shader.PropertyToID("_DryAmount");
    private static readonly int DryOpacity = Shader.PropertyToID("_DryOpacity");
    private static readonly int Splat = Shader.PropertyToID("_Splat");
    private static readonly int MainTex = Shader.PropertyToID("_MainTex");

    public float range = 10f;

    public bool tellDistance = false;

    private Renderer meshRenderer;
    private bool isLOD = false;

    [SerializeField]
    private Material LODMaterial;
    //[SerializeField]
    private Material splatMaterial;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        splatMaterial = meshRenderer.material;
        InitializeSurface();
        InvokeRepeating(nameof(ChunkLoad), 1, 1);
    }

    void ChunkLoad()
    {
        bool inRange = (scr_fpscontroller.PlayerPosition - transform.position).sqrMagnitude < range;
        switch (inRange)
        {
            case false when !isLOD:
                meshRenderer.material = LODMaterial;
                GunkManager.SaveGunkSurface(this);
                splatmap.Release();
                isLOD = true;
                break;
            case true when isLOD:
                splatmap = GunkManager.LoadGunkSurface(this);
                meshRenderer.material = splatMaterial;
                _baseMaterial.SetTexture(Splat, splatmap);
                isLOD = false;
                break;
        }

        if (tellDistance)
        {
            print( "dist: " + (scr_fpscontroller.PlayerPosition - transform.position).sqrMagnitude);
        }
    }
    
    void ApplyFullGunk()
    {
        Graphics.Blit(Texture2D.blackTexture, splatmap);
    }

    void InitializeSurface()
    {
        _baseMaterial = splatMaterial;
        
        splatmap = new RenderTexture((int) splatSize.x, (int) splatSize.y, 0, RenderTextureFormat.ARGBHalf
        )
        {
            enableRandomWrite = true
        };
        splatmap.Create();
        
        Graphics.Blit(Texture2D.redTexture, splatmap);

        if (randomGunk) // and !latent
        {
            
            ApplyRandomGunk(splatmap);
        }

        if (fullGunk)
        {
            Graphics.Blit(Texture2D.blackTexture, splatmap);
        }
        _baseMaterial.SetTexture(Splat, splatmap);
        _dryMaterial = new Material(dryShader);

        GunkManager.AddGunkSurface(this);
        meshRenderer.material = _baseMaterial;
    }
    
    void ApplyRandomGunk(RenderTexture splatMap)
    {
        int noiseScale = 2048;
        Texture2D noiseTex = new Texture2D(noiseScale, noiseScale);   //splatMap.width, splatMap.height);

        Color[] pix = new Color[noiseScale*noiseScale];    //splatMap.width * splatMap.height];

        int random_x = Random.Range(0, 10000);
        int random_y = Random.Range(0, 10000);
        
        float y = 0.0F;
        
        while (y < noiseTex.height)
        {
            float x = 0.0F;

            while (x < noiseTex.width)
            {
                
                float xCoord = random_x + x / noiseTex.width * scale;
                float yCoord = random_y + y / noiseTex.height * scale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord) * 2;
                sample = Mathf.PerlinNoise(sample * xCoord, sample * yCoord);
                pix[(int) y * noiseScale + (int) x] = new Color(sample, 0, 0);
                
                x++;
            }

            y++;
        }
        
        noiseTex.SetPixels(pix);
        noiseTex.Apply();
        Graphics.Blit(noiseTex, splatMap);
    }

    public void GunkItUp()
    {
        ApplyRandomGunk(splatmap);
    }

    IEnumerator DrySurface()
    {
        while (Application.isPlaying)
        {
            yield return new WaitForSeconds(1 / dryRate);
            // ---- stuff for drying ----
            _dryMaterial.SetFloat(DryAmount, dryAmount);
            _dryMaterial.SetFloat(DryOpacity, dryOpacity);

            RenderTexture dry = (RenderTexture) _baseMaterial.GetTexture(Splat);
            RenderTexture temp = RenderTexture.GetTemporary(dry.width, dry.height, 0, RenderTextureFormat.ARGBHalf);
            Graphics.Blit(dry, temp, _dryMaterial);
            Graphics.Blit(temp, dry);
        
            _baseMaterial.SetTexture(Splat, dry);
        
            RenderTexture.ReleaseTemporary(temp);
        }

        StartCoroutine(DrySurface());
    }

    // Update is called once per frame
    void Update()
    {
        if (isLOD) return;
        if ((scr_fpscontroller.PlayerPosition - transform.position).sqrMagnitude > range) return;
        
        // ---- stuff for drying ----
        _dryMaterial.SetFloat(DryAmount, dryAmount);
        _dryMaterial.SetFloat(DryOpacity, dryOpacity);

        RenderTexture dry = (RenderTexture) _baseMaterial.GetTexture(Splat);
        RenderTexture temp = RenderTexture.GetTemporary(dry.width, dry.height, 0, RenderTextureFormat.ARGBHalf);
        Graphics.Blit(dry, temp, _dryMaterial);
        Graphics.Blit(temp, dry);
        
        _baseMaterial.SetTexture(Splat, dry);
        
        RenderTexture.ReleaseTemporary(temp);

    }

    private void FixedUpdate()
    {
        
    }

    private void OnDestroy()
    {
        GunkManager.RemoveGunkSurface(this);
    }
}

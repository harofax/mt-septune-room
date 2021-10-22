using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunkTrackCounter : MonoBehaviour
{
    public ComputeShader countingShader;
    public RenderTexture gunkTexture;

    public MeshRenderer gunkDisplay;
    private Material gunkDisplayMaterial;

    public MeshRenderer progressBar;
    private Material progressBarMaterial;
    
    private ComputeBuffer computeBuffer;
    private uint[] countData;
    private int kernelMain, kernelInit;

    private Dictionary<RenderTexture, float> scoreTracker = new Dictionary<RenderTexture, float>();

    public float refreshRate = 1.0f;
    private static readonly int Progress1 = Shader.PropertyToID("_Progress");

    private long total_pixels;
    
    private const int scoreIncrement = 10000;
   
    private int m_score = 0;
    public int score
    {
        get { return m_score; }
        set
        {
            if (m_score == value) return;
            m_score = value;
            if (OnVariableChange != null)
                OnVariableChange(m_score);
        }
    }

    public delegate void OnVariableChangeDelegate(int newVal);

    public event OnVariableChangeDelegate OnVariableChange;


    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(LateStart), 0.5f);

        gunkDisplayMaterial = gunkDisplay.material;
        progressBarMaterial = progressBar.material;

        InvokeRepeating(nameof(RefreshRadar), 5.0f, refreshRate);

    }

    void LateStart()
    {
        gunkTexture = GunkManager.GetGunkSurfaceObject(0).splatmap;
        // if (null == countingShader || null == gunkTexture)
        // {
        //     Debug.Log("Shader or input texture missing (score counting)");
        //     yield break;
        // }

        kernelMain = countingShader.FindKernel("CSMain");
        kernelInit = countingShader.FindKernel("CSInit");

        computeBuffer = new ComputeBuffer(1, sizeof(uint));
        countData = new uint[1];

        // if (kernelInit < 0 || kernelMain < 0 ||
        //     null == computeBuffer || null == countData)
        // {
        //     Debug.Log("Initialization failed.");
        //     yield break;
        // }

        countingShader.SetTexture(kernelMain, "InputTexture", gunkTexture);
        countingShader.SetBuffer(kernelMain, "ResultBuffer", computeBuffer);
        countingShader.SetBuffer(kernelInit, "ResultBuffer", computeBuffer);
    }

    public void SetSurface(GunkSurface gunkSurface)
    {
        gunkTexture = gunkSurface.splatmap;
        countingShader.SetTexture(kernelMain, "InputTexture", gunkTexture);
        countingShader.SetTexture(kernelInit, "InputTexture", gunkTexture);
        gunkDisplayMaterial.mainTexture = gunkTexture;
        
        total_pixels = gunkTexture.width * gunkTexture.height;

        if (scoreTracker.ContainsKey(gunkTexture)) return;
        
        scoreTracker.Add(gunkTexture, total_pixels - CountCleanPixels());

    }

    private void OnDestroy()
    {
        if (null == computeBuffer) return;
        computeBuffer.Release();
        computeBuffer = null;
    }

    uint CountCleanPixels()
    {
        countingShader.Dispatch(kernelInit, 64, 1, 1);
        countingShader.Dispatch(kernelMain, gunkTexture.width / 8, gunkTexture.height / 8, 1);
        computeBuffer.GetData(countData);

        uint cleanPix = countData[0];

        //print("clean pixls: " + cleanPix);

        //float percent = ((float) cleanPix / ((float) gunkTexture.width * (float) gunkTexture.height)); //* 100.0f;
        //return 1.0f - percent;

        return cleanPix;
    }

    void RefreshRadar()
    {
        var current_clean_pixels = CountCleanPixels();
        float percentage = (float) current_clean_pixels / ((float) total_pixels);
        progressBarMaterial.SetFloat(Progress1, 1.0f - percentage);

                           // v-- intial clean pixels
        
        //score = (int) ((current_clean_pixels - scoreTracker[gunkTexture] ) / scoreIncrement);
        score = (int) ((scoreTracker[gunkTexture] - (total_pixels - CountCleanPixels())) / 10000f);
        // print("curr clean pixls: " + current_clean_pixels);
        // print("percentage: " + percentage);
        // print("1.0 - perc: " + (1.0f - percentage));
        // print("total pixls: " + total_pixels);
        // print("clean threshold: " + 0.97 * total_pixels);
        // if we're 97% done, clean up entire surface
        if (current_clean_pixels > 0.97 * total_pixels)
        {
            Graphics.Blit(Texture2D.redTexture, gunkTexture);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

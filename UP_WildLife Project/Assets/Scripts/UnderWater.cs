using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.Universal;

public class UnderWater : MonoBehaviour
{
    public UnityEngine.Rendering.Volume Vol;

    private UnityEngine.Rendering.Universal.DepthOfField Dph;
    private UnityEngine.Rendering.Universal.LensDistortion LD;
    private UnityEngine.Rendering.Universal.ChromaticAberration CA;
    private UnityEngine.Rendering.Universal.Vignette V;


    public float BlurAMount;
    public float FocalLength;

    [Header("Hit effects")]
    public float Smoothness;
    public float HitIntensity;


    [Header("Fog Setting")]
    public bool EnableFog;
    public float FogDensity = 0.29f;
    public Color _color;
    public float FogStartDistance;


    [Header("Lenes Distrotation")]
    public float Intensity;
    public float XMultiplier;
    public float YMultiplier;


    [Header("Chromatic")]
    public float CA_Intensity;



    [Header("Normal Fog")]
    public float NewFogDensity;
    public Color NewFogColor;
    public bool NewFogEnabled;
    public FogMode Mode;



    private void Start()
    {


        Vol.profile.TryGet(out Dph);
        Vol.profile.TryGet(out LD);
        Vol.profile.TryGet(out CA);
        Vol.profile.TryGet(out V);


    }

    private void Update()
    {



        FogSetting();
        _depthOfFieldView();
        _LensDistrotation();
        _ChromaticAberration();
        HitEffects();
        if(NewFogEnabled)
        {
        NewFog();

        }
    }



    void FogSetting()
    {
        RenderSettings.fog = EnableFog;
        RenderSettings.fogDensity = FogDensity;
        RenderSettings.fogColor = _color;
        RenderSettings.fogStartDistance = FogStartDistance;



    }


    void _depthOfFieldView()
    {


        Dph.focusDistance.value = BlurAMount;
        Dph.focalLength.value = FocalLength;

    }


    void _LensDistrotation()
    {
        LD.intensity.value = Intensity;
        LD.xMultiplier.value = XMultiplier;
        LD.yMultiplier.value = YMultiplier;






    }


    void _ChromaticAberration()
    {
        CA.intensity.value = CA_Intensity;


    }


    public void NewFog()
    {
        RenderSettings.fogMode = Mode;
        RenderSettings.fogDensity = NewFogDensity;
        RenderSettings.fogColor = NewFogColor;
    }


    public void HitEffects()
    {
        HitIntensity = Mathf.Clamp(HitIntensity, 0f, 0.5f);
        V.intensity.Override(HitIntensity);
        V.smoothness.Override(Smoothness);


    }
}

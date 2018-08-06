using UnityEngine;
using System.Collections;
using System.Threading;

public class PostProcessManager : MonoBehaviour {

    iniParser parser = new iniParser();
    private GraphicsSettings gs = new GraphicsSettings();

	void Start () {
        Refresh();
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            Refresh();
        }
    }

    void Refresh()
    {
        parser.Clear();
        if (parser.DoesExist(IniFiles.CONFIG))
        {
            parser.Load(IniFiles.CONFIG);
        }
        else
        {
            gs.Create();
            parser.Load(IniFiles.CONFIG);
        }

        DepthOfField34 dof = gameObject.GetComponent<DepthOfField34>();
        Bloom blf = gameObject.GetComponent<Bloom>();
        AmbientObscurance ssao = gameObject.GetComponent<AmbientObscurance>();
        SunShafts sunshaft = gameObject.GetComponent<SunShafts>();
        NoiseAndGrain nag = gameObject.GetComponent<NoiseAndGrain>();
        EdgeDetectEffectNormals edn = gameObject.GetComponent<EdgeDetectEffectNormals>();
        Vignetting vig = gameObject.GetComponent<Vignetting>();

        if (bool.Parse(parser.Get("fx_enable")))
        {
            if (bool.Parse(parser.Get("fx_dof")))
            {
                dof.enabled = true;
                dof.resolution = DofResolution.High;
                dof.quality = bool.Parse(parser.Get("fx_dof_foreground")) ? Dof34QualitySetting.BackgroundAndForeground : Dof34QualitySetting.OnlyBackground;
                dof.bokeh = bool.Parse(parser.Get("fx_dof_bokeh")) ? true : false;

                dof.focalPoint = float.Parse(parser.Get("fx_dof_focaldistance"));
                dof.smoothness = float.Parse(parser.Get("fx_dof_smoothness"));
                dof.focalSize = float.Parse(parser.Get("fx_dof_focalSize"));

                dof.visualize = bool.Parse(parser.Get("fx_dof_visualizeFocus"));

                switch (parser.Get("fx_dof_blurriness"))
                {
                    case "0":
                        dof.bluriness = DofBlurriness.Low;
                        break;
                    case "1":
                        dof.bluriness = DofBlurriness.High;
                        break;

                    case "2":
                        dof.bluriness = DofBlurriness.VeryHigh;
                        break;
                    default:
                        dof.bluriness = DofBlurriness.High;
                        break;
                }

                dof.maxBlurSpread = float.Parse(parser.Get("fx_dof_blurSpread"));
                dof.foregroundBlurExtrude = float.Parse(parser.Get("fx_dof_foregroundSize"));

                switch (parser.Get("fx_dof_bokeh_destination"))
                {
                    case "0":
                        dof.bokehDestination = BokehDestination.Background;
                        break;
                    case "1":
                        dof.bokehDestination = BokehDestination.BackgroundAndForeground;
                        break;

                    case "2":
                        dof.bokehDestination = BokehDestination.Foreground;
                        break;

                    default:
                        dof.bokehDestination = BokehDestination.Background;
                        break;
                }

                dof.bokehIntensity = float.Parse(parser.Get("fx_dof_bokeh_intensity"));
                dof.bokehThreshholdLuminance = float.Parse(parser.Get("fx_dof_bokeh_minLuminance"));
                dof.bokehThreshholdContrast = float.Parse(parser.Get("fx_dof_bokeh_minContrast"));
                dof.bokehDownsample = int.Parse(parser.Get("fx_dof_bokeh_DownSample"));
                dof.bokehScale = float.Parse(parser.Get("fx_dof_bokeh_sizeScale"));
            }
            else
            {
                dof.enabled = false;
            }

            // SSAO

            if (bool.Parse(parser.Get("fx_SSAO")))
            {
                ssao.enabled = true;
                ssao.intensity = float.Parse(parser.Get("fx_SSAO_intensity"));
                ssao.radius = float.Parse(parser.Get("fx_SSAO_radius"));
                ssao.blurIterations = int.Parse(parser.Get("fx_SSAO_blurIterations"));
                ssao.blurFilterDistance = float.Parse(parser.Get("fx_SSAO_blurFilterDistance"));
                ssao.downsample = int.Parse(parser.Get("fx_SSAO_downsample"));
            }
            else
            {
                ssao.enabled = false;
            }

            // NOISE GRAIN

            nag.enabled = bool.Parse(parser.Get("fx_noisegrain")) ? true : false;
            nag.intensityMultiplier = float.Parse(parser.Get("fx_noisegrain_intensity"));

            // BLOOM

            if(bool.Parse(parser.Get("fx_bloom"))){
                blf.enabled = true;

                switch (parser.Get("fx_bloom_quality"))
                {
                    case "0":
                        blf.quality = Bloom.BloomQuality.Cheap;
                        break;
                    case "1":
                        blf.quality = Bloom.BloomQuality.High;
                        break;
                    default:
                        blf.quality = Bloom.BloomQuality.High;
                        break;
                }

                switch (parser.Get("fx_bloom_mode"))
                {
                    case "0":
                        blf.tweakMode = Bloom.TweakMode.Basic;
                        break;
                    case "1":
                        blf.tweakMode = Bloom.TweakMode.Complex;
                        break;
                    default:
                        blf.tweakMode = Bloom.TweakMode.Complex;
                        break;
                }

                switch (parser.Get("fx_bloom_blendMode"))
                {
                    case "0":
                        blf.screenBlendMode = Bloom.BloomScreenBlendMode.Screen;
                        break;
                    case "1":
                        blf.screenBlendMode = Bloom.BloomScreenBlendMode.Add;
                        break;
                    default:
                        blf.screenBlendMode = Bloom.BloomScreenBlendMode.Add;
                        break;
                }

                blf.hdr = bool.Parse(parser.Get("fx_bloom_hdr")) ? Bloom.HDRBloomMode.On : Bloom.HDRBloomMode.Off;
                blf.bloomIntensity = float.Parse(parser.Get("fx_bloom_intensity"));
                blf.bloomThreshhold = float.Parse(parser.Get("fx_bloom_threshhold"));
                blf.bloomBlurIterations = int.Parse(parser.Get("fx_bloom_blurIterations"));
                blf.blurWidth = float.Parse(parser.Get("fx_bloom_sampleDistance"));

                switch (parser.Get("fx_bloom_lensFlareMode"))
                {
                    case "0":
                        blf.lensflareMode = Bloom.LensFlareStyle.Ghosting;
                        break;
                    case "1":
                        blf.lensflareMode = Bloom.LensFlareStyle.Anamorphic;
                        break;
                    case "2":
                        blf.lensflareMode = Bloom.LensFlareStyle.Combined;
                        break;
                    default:
                        blf.lensflareMode = Bloom.LensFlareStyle.Ghosting;
                        break;
                }

                blf.lensflareIntensity = float.Parse(parser.Get("fx_bloom_LFlocalIntensity"));
                blf.lensflareThreshhold = float.Parse(parser.Get("fx_bloom_LFthreshhold"));

            } else {
                blf.enabled = false;
            }
            
            // Sunshafts

            if(bool.Parse(parser.Get("fx_sunshaft"))){
                sunshaft.enabled = true;
                sunshaft.useDepthTexture = bool.Parse(parser.Get("fx_sunshaft_zBuffer"));

                switch (parser.Get("fx_sunshaft_resolution"))
                {
                    case "0":
                        sunshaft.resolution = SunShaftsResolution.Low;
                        break;
                    case "1":
                        sunshaft.resolution = SunShaftsResolution.Normal;
                        break;
                    case "2":
                        sunshaft.resolution = SunShaftsResolution.High;
                        break;
                    default:
                        sunshaft.resolution = SunShaftsResolution.Normal;
                        break;
                }

                switch (parser.Get("fx_sunshaft_blendMode"))
                {
                    case "0":
                        sunshaft.screenBlendMode = ShaftsScreenBlendMode.Add;
                        break;
                    case "1":
                        sunshaft.screenBlendMode = ShaftsScreenBlendMode.Screen;
                        break;
                    default:
                        sunshaft.screenBlendMode = ShaftsScreenBlendMode.Screen;
                        break;
                }

                sunshaft.maxRadius = float.Parse(parser.Get("fx_sunshaft_distFalloff"));
                sunshaft.sunShaftBlurRadius = float.Parse(parser.Get("fx_sunshaft_blurSize"));
                sunshaft.radialBlurIterations = int.Parse(parser.Get("fx_sunshaft_blurIterations"));
                sunshaft.sunShaftIntensity = float.Parse(parser.Get("fx_sunshaft_intensity"));
                sunshaft.useSkyBoxAlpha = float.Parse(parser.Get("fx_sunshaft_alphaMask"));

            } else {
                sunshaft.enabled = false;
            }

            // Edge detect

            edn.enabled = bool.Parse(parser.Get("fx_edgeDetect")) ? true : false;

            // Vignetting

            if(bool.Parse(parser.Get("fx_vignetting"))){
                vig.enabled = true;
                vig.intensity = float.Parse(parser.Get("fx_vignetting_intensity"));
                vig.blurSpread = float.Parse(parser.Get("fx_vignetting_blurredCornors"));
                vig.chromaticAberration = float.Parse(parser.Get("fx_vignetting_aberration"));
            } else {
                vig.enabled = false;
            }

        }
        else
        {
            dof.enabled = false;
            ssao.enabled = false;
            nag.enabled = false;
            blf.enabled = false;
            sunshaft.enabled = false;
            edn.enabled = false;
            vig.enabled = false;
        }
    }

}

using UnityEngine;
using System.Collections;

public class GraphicsSettings {

	iniParser parser = new iniParser();

	public void Create(){
        parser.Set("Audio", "a_masterlvl", "100", "Scale of 0 - 100");
        parser.Set("Audio", "a_musiclvl", "25", "Scale of 0 - 100");
        parser.Set("Audio", "a_ambientlvl", "75", "Scale of 0 - 100");

        bool fullscreen = Screen.fullScreen;
		parser.Set("Graphics","g_resolution",Screen.width+"x"+Screen.height);
        parser.Set("Graphics", "g_windowed", fullscreen.ToString(),"Toggle Windowed mode");
        parser.Set("Graphics", "g_qualitylvl", QualitySettings.GetQualityLevel().ToString(),"Scale of 0 - 6, 0 being lowest, 5 being max and 6 being custom");
        parser.Set("Graphics", "g_antialias", "8", "Can only be: 0, 1, 2, 4, 8");
        parser.Set("Graphics", "g_vsync", "2", "Can only be: 0, 1, 2");

        // shaders

        parser.Set("Post Processing", "fx_enable", "True", "Dissable or enable post processing effects");

        // --- DoF
        parser.Set("Post Processing", "fx_dof", "True", "Dissable or enable Depth of Field effect");
        parser.Set("Post Processing", "fx_dof_visualizeFocus", "False");
        parser.Set("Post Processing", "fx_dof_smoothtime", "2", "Dynamic DoF smooth from - to distance");
        parser.Set("Post Processing", "fx_dof_foreground", "True", "Dissable or enable Depth of Field effect on foreground aswell as background");
        parser.Set("Post Processing", "fx_dof_bokeh", "True", "Dissable or enable bokeh effect, Requires Depth of Field to be active");
        parser.Set("Post Processing", "fx_dof_focaldistance", "1");
        parser.Set("Post Processing", "fx_dof_smoothness", "0.5");
        parser.Set("Post Processing", "fx_dof_focalSize", "0");
        parser.Set("Post Processing", "fx_dof_blurriness", "1","0 = low, 1 = high, 2 = very high");
        parser.Set("Post Processing", "fx_dof_blurSpread", "0.5");
        parser.Set("Post Processing", "fx_dof_foregroundSize", "0");
        parser.Set("Post Processing", "fx_dof_bokeh_destination", "0"," 0 = BackGround, 1 = background + foreground, 2 = foreground");
        parser.Set("Post Processing", "fx_dof_bokeh_intensity", "0.15", "Scale of 0.00 - 1.00");
        parser.Set("Post Processing", "fx_dof_bokeh_minLuminance", "0.55", "Scale of 0.00 - 1.00");
        parser.Set("Post Processing", "fx_dof_bokeh_minContrast", "0.1", "Scale of 0.00 - 1.00");
        parser.Set("Post Processing", "fx_dof_bokeh_DownSample", "1","Can only be: 1, 2, 3");
        parser.Set("Post Processing", "fx_dof_bokeh_sizeScale", "2.4", "Scale of 0.00 - 20.00");

        
        // --- SSAO

        parser.Set("Post Processing", "fx_SSAO", "True", "Dissable or enable Screen Space Ambient Occlusion");
        parser.Set("Post Processing", "fx_SSAO_intensity", "0.50","Scale of 0.00 - 3.00");
        parser.Set("Post Processing", "fx_SSAO_radius", "0.50", "Scale of 0.00 - 3.00");
        parser.Set("Post Processing", "fx_SSAO_blurIterations", "1", "Can only be: 0, 1, 2, 3");
        parser.Set("Post Processing", "fx_SSAO_blurFilterDistance", "1.25", "Scale of 0.00 - 3.00");
        parser.Set("Post Processing", "fx_SSAO_downsample", "0", "Can only be: 0, 1");

        // --- noise grain

        parser.Set("Post Processing", "fx_noisegrain", "True", "Dissable or Enable noise effect, Requires DX11");
        parser.Set("Post Processing", "fx_noisegrain_intensity", "0.1");

        // --- Bloom

        parser.Set("Post Processing", "fx_bloom", "True", "Dissable or enable Light glow / bleed");
        parser.Set("Post Processing", "fx_bloom_quality", "1", "0 = cheap, 1 = high");
        parser.Set("Post Processing", "fx_bloom_mode", "1", "0 = basic, 1 = complex");
        parser.Set("Post Processing", "fx_bloom_blendMode", "1", "0 = screen, 1 = add");
        parser.Set("Post Processing", "fx_bloom_hdr", "True", "Dissable or enable High Dynamic Range");
        parser.Set("Post Processing", "fx_bloom_intensity", "0.50");
        parser.Set("Post Processing", "fx_bloom_threshhold", "0.50", "Scale of 0.00 - 3.00");
        parser.Set("Post Processing", "fx_bloom_blurIterations", "2", "Scale of 1 - 4");
        parser.Set("Post Processing", "fx_bloom_sampleDistance", "2", "Scale of 0.00 - 10.00");
        parser.Set("Post Processing", "fx_bloom_lensFlareMode", "0", "0 = Ghosting, 1 = anamorphic, 2 = combined");
        parser.Set("Post Processing", "fx_bloom_LFlocalIntensity", "0.00");
        parser.Set("Post Processing", "fx_bloom_LFthreshhold", "0.30", "Scale of 0.00 - 4.00");

        // --- Sun Shafts (God rays)

        parser.Set("Post Processing", "fx_sunshaft", "True", "Dissable or enable sun shafts (God rays)");
        parser.Set("Post Processing", "fx_sunshaft_zBuffer", "True", "Toggle Rely on Z Buffer");
        parser.Set("Post Processing", "fx_sunshaft_resolution", "2", "0 = low, 1 = normal, 2 = high");
        parser.Set("Post Processing", "fx_sunshaft_blendMode", "0", "0 = add, 1 = screen");
        parser.Set("Post Processing", "fx_sunshaft_distFalloff", "0.250", "Distance fallout, scale of 0.00 - 1.00");
        parser.Set("Post Processing", "fx_sunshaft_blurSize", "2.5", "Scale of 0.00 - 10.00");
        parser.Set("Post Processing", "fx_sunshaft_blurIterations", "2", "Scale of 0 - 4");
        parser.Set("Post Processing", "fx_sunshaft_intensity", "1.15");
        parser.Set("Post Processing", "fx_sunshaft_alphaMask", "0.75", "Scale of 0.00 - 1.00");

        // --- Edge Detect

        parser.Set("Post Processing", "fx_edgeDetect", "True", "Toggle edge detection, Similar to borderlands");

        // --- Vignetting

        parser.Set("Post Processing", "fx_vignetting", "True", "Toggle simulation of common lens artifacts");
        parser.Set("Post Processing", "fx_vignetting_intensity", "0.375");
        parser.Set("Post Processing", "fx_vignetting_blurredCornors", "0.00");
        parser.Set("Post Processing", "fx_vignetting_aberration", "0.2");




        parser.Set("Controls", "input_controller", "True", "If using Controller");

		parser.Save(IniFiles.CONFIG);
	}

	public void load(iniParser config){

        string resolution = config.Get("g_resolution");
        int posx = resolution.IndexOf("x");
        int rW = int.Parse(resolution.Substring(0,(posx)));
        int rH = int.Parse(resolution.Substring(posx + 1));
        bool rWM = bool.Parse(config.Get("g_windowed"));

        Screen.SetResolution(rW, rH, !rWM);
	}

	public void Save(){

	}
}

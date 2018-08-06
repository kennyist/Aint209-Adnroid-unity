using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class MainMenu : MonoBehaviour {
	
	private bool mainMenu = true;
	private bool settingsMenu = false;
	private bool settingsSaveMessage = false;
	private bool startingServer = false;
	private bool findingServer = false;
	private bool lobby = false;
	private bool timedOut = false;
    private bool resolutionMenu = false;

    private string[] resolutionsSTR;
    private string currentResolution;

	private string playerName;

	private NetworkManager network;

	public GUISkin skinTopBar;
	public GUISkin skinOptions;
	public Texture2D avatar;
	public Texture2D settings;

    string ready = "READY";
    bool audio = false, video = false, game = false;

	private iniParser parser = new iniParser();

	private GraphicsSettings gs = new GraphicsSettings();

    int curResInt = -1, vsync = 0;

    float nativeHeight = 1920f, nativeWidth = 1080f;

    // Option values
	
	void Start () {
        resolutionsSTR = new string[Screen.resolutions.Length];
        windowed = Screen.fullScreen;
		network = GameObject.FindGameObjectWithTag("Lobby").GetComponent<NetworkManager>();

        //GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity,new Vector3(Screen.width / nativeWidth, Screen.height / nativeHeight));
		
        if(parser.DoesExist(IniFiles.CONFIG)){
            parser.Load(IniFiles.CONFIG);
        } else {
            gs.Create();
            parser.Load(IniFiles.CONFIG);
        }

        if (!Application.isEditor)
        {
            int i = 0;
            foreach (Resolution res in Screen.resolutions)
            {
                if (Screen.width == res.width && Screen.height == res.height)
                {
                    currentResolution = res.width + "x" + res.height;
                    curResInt = i;
                }
                else
                {
                    currentResolution = "Select res";
                }
                resolutionsSTR[i] = res.width + "x" + res.height;
                i++;
            }
        }
        else
        {
            curResInt = 0;
        }

        loadSettings();

	}

	void Update(){
		if(network.timedOut){
			timedOut = true;
			findingServer = false;
			network.timedOut = false;
		}
	}

	void OnDisconnectedFromServer(NetworkDisconnection info) {
		mainMenu = true;
		lobby = false;
	}

    // Option values
    private bool windowed, postProccess, DoF, DoFbokeh, DoFforeground, noise, SSAO, bloom, HDR, sunShaft, controller = false;
    private float dofFocusRange, noiseIntensity;
    int antialiasSel = 0, masterLVL = 0, musicLVL = 0, ambientlvl = 0;

    void loadSettings()
    {
        windowed = bool.Parse(parser.Get("g_windowed"));
        postProccess = bool.Parse(parser.Get("fx_enable"));
        DoF = bool.Parse(parser.Get("fx_dof"));
        DoFforeground = bool.Parse(parser.Get("fx_dof_foreground"));
        DoFbokeh = bool.Parse(parser.Get("fx_dof_bokeh"));
        noise = bool.Parse(parser.Get("fx_noisegrain"));
        SSAO = bool.Parse(parser.Get("fx_SSAO"));
        bloom = bool.Parse(parser.Get("fx_bloom"));
        HDR = bool.Parse(parser.Get("fx_bloom_hdr"));
        sunShaft = bool.Parse(parser.Get("fx_sunshaft"));

        controller = bool.Parse(parser.Get("input_controller"));
        masterLVL = int.Parse(parser.Get("a_masterlvl"));
        musicLVL = int.Parse(parser.Get("a_musiclvl"));
        ambientlvl = int.Parse(parser.Get("a_ambientlvl"));

        switch (int.Parse(parser.Get("g_antialias")))
        {
            case 1:
                antialiasSel = 1;
                break;
            case 2:
                antialiasSel = 2;
                break;

            case 4:
                antialiasSel = 3;
                break;

            case 8:
                antialiasSel = 4;
                break;

            default:
                antialiasSel = 0;
                break;
        }

        vsync = int.Parse(parser.Get("g_vsync"));

        dofFocusRange = float.Parse(parser.Get("fx_dof_focaldistance"));
        noiseIntensity = float.Parse(parser.Get("fx_noisegrain_intensity"));
    }

    void SaveSettings()
    {

        parser.Save(IniFiles.CONFIG);
        gs.load(parser);
        parser.Load(IniFiles.CONFIG);
    }

    Vector2 optionScroll = new Vector2();
    bool antialiasMenu = false;
    string[] antialiasString = { "NONE", "x1", "x2", "x4", "x8" };
    string[] vsyncString = { "NONE", "x1", "x2" };

	void OnGUI(){

		GUI.skin = skinTopBar;

        GUI.Label(new Rect(Screen.width - 100, 0, 50, 10), "0.1.0.5", "Version");

		if(timedOut){
			GUI.Box(new Rect((Screen.width / 2) - 100, Screen.height / 2, 200,30),"Connection Timed Out");
			if(GUI.Button(new Rect((Screen.width / 2) - 25, (Screen.height / 2) + 40, 50, 30),"Ok")){
				timedOut = false;
			}
		}

		if(startingServer){
			GUI.Box(new Rect((Screen.width / 2) - 100, Screen.height / 2, 200,30),"Starting Server");
			if(network.ServerStarted){
				startingServer = false;
				lobby = true;
				mainMenu = false;
				playerName = "Host - Player 1";
			}
		}

		if(findingServer){
			GUI.Box(new Rect((Screen.width / 2) - 100, Screen.height / 2, 200,30),"Finding Server");
			if(network.joinedGame){
				findingServer = false;
				lobby = true;
				mainMenu = false;
				network.joinedGame = false;
			}
		}

		if(mainMenu && !startingServer && !findingServer && !timedOut && !settingsMenu){
			GUILayout.BeginArea(new Rect(50, 150, 300, 300));
			GUILayout.BeginVertical();
            
            #if !UNITY_ANDROID
			if(GUILayout.Button("CREATE GAME")){
				network.StartServer();
				startingServer = true;
			};
            #endif

			if(GUILayout.Button("JOIN GAME")){
				network.RefreshHosts();
				findingServer = true;
			}
            GUILayout.Space(30f);
            if (GUILayout.Button("OPTIONS"))
            {
                settingsMenu = !settingsMenu;
            }

            #if !UNITY_ANDROID
			GUILayout.Button("LEVEL EDITOR");
            GUILayout.Button("CUSTOMIZATION");
            #endif

            GUILayout.Space(30f);
            GUILayout.Button("PATCH NOTES");
			if(GUILayout.Button("QUIT GAME")){
				Application.Quit();
			}

            GUILayout.EndVertical();
            GUILayout.EndArea();
		}

        if (lobby)
        {
            GUILayout.BeginArea(new Rect(50, 150, 300, 300));
            GUILayout.BeginVertical();

            #if !UNITY_ANDROID
            GUILayout.Button("OPTIONS");
            GUILayout.Button("CUSTOMIZATION");
            GUILayout.Space(30f);
            if (GUILayout.Button(ready))
            {
                GameObject.FindGameObjectWithTag("Lobby").SendMessage("Ready");
                ready = (ready == "READY") ? "NOT READY" : "READY";
            }
            GUILayout.Space(30f);

            #endif
            if (GUILayout.Button("LEAVE GAME"))
            {
                GameObject.FindGameObjectWithTag("Lobby").SendMessage("LeaveGame");
            }

            GUILayout.EndVertical();
            GUILayout.EndArea();
        }

        if (settingsMenu)
        {
            if (audio)
            {

                GUI.Label(new Rect(50, 75, 100, 50), "AUDIO", "SettingTitle");

                GUILayout.BeginArea(new Rect(50, 150, 400, 300));
                GUILayout.BeginVertical();

                GUILayout.BeginHorizontal();
                GUILayout.Label("MASTER LEVEL");
                masterLVL = (int)GUILayout.HorizontalSlider(masterLVL, 0, 100);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("MUSIC LEVEL");
                musicLVL = (int)GUILayout.HorizontalSlider(musicLVL, 0, 100);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("AMBIENT LEVEL");
                ambientlvl = (int)GUILayout.HorizontalSlider(ambientlvl, 0, 100);
                GUILayout.EndHorizontal();

                GUILayout.Space(30f);

                if (GUILayout.Button("BACK"))
                {
                    audio = false;
                    Save("audio");
                }

                GUILayout.EndVertical();
                GUILayout.EndArea();
            }
            else if (game)
            {
                GUI.Label(new Rect(50, 75, 100, 50), "GAME", "SettingTitle");

                GUILayout.BeginArea(new Rect(50, 150, 400, 300));
                GUILayout.BeginVertical();

                GUILayout.BeginHorizontal();
                GUILayout.Label("CONTROLLER");
                if(GUILayout.Button(controller.ToString().ToUpper())){
                    controller = !controller;
                }
                GUILayout.EndHorizontal();

                GUILayout.Space(30f);

                if (GUILayout.Button("BACK"))
                {
                    game = false;
                    Save("game");
                }

                GUILayout.EndVertical();
                GUILayout.EndArea();
            }
            else if (video)
            {

                GUI.Label(new Rect(50, 75, 100, 50), "VIDEO", "SettingTitle");

                GUILayout.BeginArea(new Rect(50, 150, 400, 300));
                GUILayout.BeginVertical();

                GUILayout.BeginHorizontal();
                GUILayout.Label("RESOLUTION");
                if (GUILayout.Button("<"))
                {
                    if (curResInt == 0)
                    {
                        curResInt = Screen.resolutions.Length - 1;
                    }
                    else
                    {
                        curResInt--;
                    }
                }
                GUILayout.Label(Screen.resolutions[curResInt].width + "x" + Screen.resolutions[curResInt].height + "".ToUpper(), "SettingLabel");
                if (GUILayout.Button(">"))
                {
                    if (curResInt == Screen.resolutions.Length - 1)
                    {
                        curResInt = 0;
                    }
                    else
                    {
                        curResInt++;
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("WINDOWED");
                if (GUILayout.Button(windowed.ToString().ToUpper()))
                {
                    windowed = !windowed; 
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("V-SYNC");
                if (GUILayout.Button("<"))
                {
                    if (vsync == 0)
                    {
                        vsync = vsyncString.Length - 1;
                    }
                    else
                    {
                        vsync--;
                    }
                }
                GUILayout.Label(vsyncString[vsync],"SettingLabel");
                if (GUILayout.Button(">"))
                {
                    if (vsync == vsyncString.Length - 1)
                    {
                        vsync = 0;
                    }
                    else
                    {
                        vsync++;
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("ANTI ALIASING");
                if (GUILayout.Button("<"))
                {
                    if (antialiasSel == 0)
                    {
                        antialiasSel = antialiasString.Length - 1;
                    }
                    else
                    {
                        antialiasSel--;
                    }
                }
                GUILayout.Label(antialiasString[antialiasSel], "SettingLabel");
                if (GUILayout.Button(">"))
                {
                    if (antialiasSel == antialiasString.Length - 1)
                    {
                        antialiasSel = 0;
                    }
                    else
                    {
                        antialiasSel++;
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.Space(30f);

                if (GUILayout.Button("BACK"))
                {
                    video = false;
                    Save("video");
                }

                GUILayout.EndVertical();
                GUILayout.EndArea();
            }
            else
            {
                GUI.Label(new Rect(50, 75, 100, 50), "OPTIONS", "SettingTitle");
                GUILayout.BeginArea(new Rect(50, 150, 400, 300));
                GUILayout.BeginVertical();

                if (GUILayout.Button("AUDIO")) { audio = true; }
                if (GUILayout.Button("VIDEO")) { video = true; }
                if (GUILayout.Button("GAME")) { game = true; }

                GUILayout.Space(30f);

                if (GUILayout.Button("BACK"))
                {
                    settingsMenu = !settingsMenu;
                }

                GUILayout.EndVertical();
                GUILayout.EndArea();
            }
        }
	}

    void Save(string menu)
    {
        switch (menu)
        {
            case "audio":
                parser.Set("Audio", "a_masterlvl", masterLVL.ToString());
                parser.Set("Audio", "a_musiclvl", musicLVL.ToString());
                parser.Set("Audio", "a_ambientlvl", ambientlvl.ToString());
                break;

            case "game":
                parser.Set("Controls", "input_controller", controller.ToString());
                break;

            case "video":
                parser.Set("Graphics", "g_resolution", Screen.resolutions[curResInt].width + "x" + Screen.resolutions[curResInt].height);
                parser.Set("Graphics", "g_windowed", windowed.ToString());
                parser.Set("Graphics", "g_vsync", vsync.ToString());
                switch (antialiasSel)
                {
                    case 1:
                        parser.Set("Graphics", "g_antialias", "1");
                        break;
                    case 2:
                        parser.Set("Graphics", "g_antialias", "2");
                        break;

                    case 3:
                        parser.Set("Graphics", "g_antialias", "4");
                        break;

                    case 4:
                        parser.Set("Graphics", "g_antialias", "8");
                        break;

                    default:
                        parser.Set("Graphics", "g_antialias", "0");
                        break;
                }
                break;
        }

        parser.Save(IniFiles.CONFIG);
        parser.Load(IniFiles.CONFIG);
        loadSettings();
    }
}

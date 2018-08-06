using UnityEngine;
using System.Collections;

public class XPmanager : MonoBehaviour {

    iniParser parser = new iniParser();
    
    public GUISkin skin;

    [System.Serializable]
    public class Colors
    {
        public Color background = Color.black;
        public Color foreground = Color.cyan;
        public Color xpShadowIncrease = Color.white;
    }

    [System.Serializable]
    public class Textures
    {
        public Texture2D background;
        public Texture2D foreground;
        public Texture2D xpShadowIncrease;
    }

    public enum DisplayMode { Colors, Textures }

    public float barHeight = 10f, screenPer = 0.33f;
    public DisplayMode mode = new DisplayMode();
    public Colors colors = new Colors();
    public Textures textures = new Textures();

    int XP;
    float barwidth;
    float boxwidth;
    float barstrt;
    int localxp;
    Texture2D background;
    Texture2D foreground;
    Texture2D xpShadow;

	// Use this for initialization
	void Start () {

        if (mode == DisplayMode.Colors)
        {
            background = new Texture2D(1, 1);
            background.SetPixel(1, 1, colors.background);
            background.wrapMode = TextureWrapMode.Repeat;
            background.Apply();

            foreground = new Texture2D(1, 1);
            foreground.SetPixel(1, 1, colors.foreground);
            foreground.wrapMode = TextureWrapMode.Repeat;
            foreground.Apply();

            xpShadow = new Texture2D(1, 1);
            xpShadow.SetPixel(1, 1, colors.xpShadowIncrease);
            xpShadow.wrapMode = TextureWrapMode.Repeat;
            xpShadow.Apply();
        }
        else {
            background = textures.background;
            foreground = textures.foreground;
            xpShadow = textures.xpShadowIncrease;
        }

        barwidth = Screen.width * screenPer;
        boxwidth = barwidth / 10;
        barstrt = Screen.width * ((1.0f - screenPer) / 2);
        Debug.Log(barstrt+":"+Screen.width);

        parser.Load(IniFiles.XP);
        if (parser.Get("XP").Equals(""))
        {
            parser.Set("", "XP", "0");

            float curxp = 0;

            for (int i = 0; i < 200; i++)
            {
                if (i != 0)
                {
                    curxp += 800 + (400 * (i-1));
                }

                int cp = Mathf.CeilToInt(curxp);
                parser.Set("Ranks", "level-" + i, curxp.ToString());
            }
            curxp = 9999999;

            parser.Set("Ranks", "level-200", curxp.ToString());
            parser.Save(IniFiles.XP);
        }
        localxp = int.Parse(parser.Get("XP"));

        calc();
	}

    int curXp;

    public void AddXp(int ammount)
    {
        Debug.Log("XP ADDED");
        XP = int.Parse(parser.Get("XP")) + ammount;
        parser.Set("", "XP", XP.ToString());
    }

    void Update()
    {

        if(Input.GetKeyDown(KeyCode.P)){
            XP = int.Parse(parser.Get("XP")) + 2000;
            parser.Set("","XP",XP.ToString());
        }

        curXp = int.Parse(parser.Get("XP"));

        if (localxp < curXp)
        {
            localxp = Mathf.CeilToInt(Mathf.Lerp(localxp, curXp, 1 * Time.deltaTime));
            //calc();
        }
        calc();
    }

    int currentLevel,currentLevelXp,currentXpDifferance,currentLevelDifference;
    int nextLevel, nextLevelXp;
    float tenPercentDifferance;

    int localLevel,masterLevel, masterLevelDifferance, masterXpDifferance;

    int localNumBoxes, masterNumBoxes;
    float lastLocalBoxWidth, lastMasterBoxWidth;

    void calc()
    {
        bool triggered = false;
        for (int i = 0; i <= 200; i++)
        {
            int kp = int.Parse(parser.Get("level-" + i));
            if (kp > localxp && !triggered)
            {
                localLevel = i - 1;
                currentLevel = localLevel;
                currentLevelXp = int.Parse(parser.Get("level-" + currentLevel));
                nextLevel = i;
                nextLevelXp = kp;

                currentLevelDifference = nextLevelXp - currentLevelXp;
                currentXpDifferance = localxp - currentLevelXp;

                triggered = true;
            }
            if (kp > curXp)
            {
                masterLevel = i - 1;
                masterLevelDifferance = int.Parse(parser.Get("level-" + masterLevel)) - curXp;
                masterXpDifferance = curXp - currentLevelXp;
                break;
            }
        }

        tenPercentDifferance = currentLevelDifference / 10;
        

        localNumBoxes = 0;

        for (int i = 0; i < 11; i++)
        {
            //Debug.Log(tenPercentDifferance + " : " + currentXpDifferance + " : " + currentLevelDifference + " : " + tenPercentDifferance * i);
            if ((tenPercentDifferance * i) > currentXpDifferance)
            {
                localNumBoxes = i - 1;
                break;
            }
        }

        lastLocalBoxWidth = (currentXpDifferance - (tenPercentDifferance * localNumBoxes)) / tenPercentDifferance;

        if (localLevel < masterLevel)
        {
            lastMasterBoxWidth = 0f;
            masterNumBoxes = 10;
        }
        else
        {
            for (int i = 0; i < 11; i++)
            {
                if ((tenPercentDifferance * i) > masterXpDifferance)
                {
                    masterNumBoxes = i - 1;
                    break;
                }
            }

            lastMasterBoxWidth = (masterXpDifferance - (tenPercentDifferance * masterNumBoxes)) / tenPercentDifferance;
        }
    }

    void OnGUI()
    {
        
        GUI.skin = skin;

        //GUI.Box(new Rect(0, 200, 700, 30), "cl:" + curlvl + " - clxp:" + curlvlxp + " - nl:" + nextlvl + " - nlxp:" + nextlvlxp + " - dif:" + differance + " - xpdif:" + xpdifferance + " - numb:" + numboxs + " -  lastBoxW:" + boxWidthLast.ToString("0.00"));

        float x = barstrt;

        GUI.Label(new Rect(barstrt - 30, 10, 20, 30), "" + currentLevel);
        for (int i = 0; i <= 9; i++)
        {
            GUI.DrawTexture(new Rect(x, 20, boxwidth, barHeight), background);
            x += boxwidth + 5;
        }
        GUI.Label(new Rect(x, 10, 20, 30), "" + nextLevel);

        GUI.Label(new Rect((Screen.width / 2) - 78, 10 + barHeight + 8, 200, 30), localxp+" / "+nextLevelXp);

        x = barstrt;

        for (int i = 1; i <= masterNumBoxes; i++)
        {
            GUI.DrawTexture(new Rect(x, 20, boxwidth, barHeight), xpShadow);
            x += boxwidth + 5;
        }

        GUI.DrawTexture(new Rect(x, 20, boxwidth * lastMasterBoxWidth, barHeight), xpShadow);
        
        x = barstrt;

        for (int i = 1; i <= localNumBoxes; i++)
        {
            GUI.DrawTexture(new Rect(x, 20, boxwidth, barHeight), foreground);
            x += boxwidth + 5;
        }

        GUI.DrawTexture(new Rect(x, 20, boxwidth * lastLocalBoxWidth, barHeight), foreground);
        
	}


}

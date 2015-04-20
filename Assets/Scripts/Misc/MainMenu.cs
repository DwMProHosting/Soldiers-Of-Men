using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class Map
{
    public string name;
    public string levelName;
}


public class MainMenu : MonoBehaviour
{
	private Rect windowSize = new Rect (15,15,1000,400);
	public int curMenu = 0;
	
	private string[] settings;
	private int curIndex;
	
	public float dif = 1;
	HostData[] datas;
	public Vector2 scroll;
	private string gameName = "Server 666";
	
	//You have to change this!
	public string uniqueGameName = "MyUniqueGameName";
	private string servername;
	//After which time will the server list be refreshed ?
	public float refreshTime = 10;
	
	public Map[] maps;
	public int curMapIndex;
	public int serverlist = 0;
	//connect to a server
	private string connecttoserverip = "23.102.161.247";
	//Hosting Server
	private string serverip = "";


	

    void Start()
    {
        InvokeRepeating("GetHostList", 0, refreshTime);
        settings = QualitySettings.names;
        Network.isMessageQueueRunning = true;
    }

    void GetHostList()
    {
        MasterServer.RequestHostList(uniqueGameName);
    }

    void Update()
    {
        dif = (Screen.width / 12.8f) / 100;
        datas = MasterServer.PollHostList();
		MasterServer.RequestHostList(uniqueGameName);
    }
	private void MyWindow(int id) {
				//Joining A Game
				
				GUILayout.BeginArea (new Rect (windowSize.width / 4 - 249, windowSize.height / 6 - 47, 996, 1000));
		scroll = GUILayout.BeginScrollView(scroll);
		foreach (HostData data in datas)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label("<color=red>Game Name: </color>" + data.gameName + "   |   " +  "<color=red>Players: </color>" + data.connectedPlayers + " / " + data.playerLimit);
			if (GUILayout.Button("Connect"))
			{
				Network.Connect(data);
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndScrollView();
		GUILayout.EndArea ();
		GUI.DragWindow ();
		}
    void OnGUI()
	{


		if (serverlist == 1) {
			windowSize = GUI.Window (0, windowSize, MyWindow, "My Windowasdfas");
				}
        if (curMenu == 0)
        {
			GUI.Box(new Rect(Screen.width / 2 - 230, Screen.height / 2 - 150, 500, 200), "");
			GUILayout.BeginArea(new Rect(Screen.width / 2 - 230, Screen.height / 2 - 150, 500, 200));
			if(GUILayout.Button("Play"))
			{
				curMenu = 1;
				MasterServer.RequestHostList(uniqueGameName);
			}
			if(GUILayout.Button("Options"))
			{
				curMenu = 2;
			}
			if(GUILayout.Button("Quit"))
			{
				Application.Quit();
			}
			GUILayout.EndArea();
        }
        else if (curMenu == 1)
        {
            GUI.Box(new Rect(20 * dif, 20 * dif, 200 * dif, 200 * dif), "");
            GUILayout.BeginArea(new Rect(25 * dif, 25 * dif, 190 * dif, 190 * dif));
            gameName = GUILayout.TextField(gameName);
	        if (GUILayout.Button("Start Server"))
            {
				Network.InitializeServer(17, 25001, false);
                MasterServer.RegisterHost(uniqueGameName, gameName);
				Debug.Log("Starting Server");
            }
            if (GUILayout.Button("Select Map"))
            {
                curMenu = 5;
            }
			if(GUILayout.Button("Find Server")) {
				serverlist = 1;
			}
			if(GUILayout.Button("REFRESH")) {
				MasterServer.RequestHostList(uniqueGameName);
			}
            if (GUILayout.Button("Back"))
            {
                curMenu = 0;
            }
			GUILayout.EndArea();
           GUI.Box(new Rect(Screen.width - 400 * dif, 0, 400 * dif, Screen.height), "");
            GUILayout.BeginArea(new Rect(Screen.width - 400 * dif, 0, 400 * dif, Screen.height));
            GUILayout.Label("Avaiable Servers: " + datas.Length);
            scroll = GUILayout.BeginScrollView(scroll);
            foreach (HostData data in datas)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(data.gameName + " Players: " + data.connectedPlayers + " / " + data.playerLimit);
                if (GUILayout.Button("Connect"))
                {
                    Network.Connect(data);
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }
        else if (curMenu == 2)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 100 * dif, Screen.height / 2 - 80 * dif, 200 * dif, 50 * dif), "Audio"))
            {
                curMenu = 3;
            }
            if (GUI.Button(new Rect(Screen.width / 2 - 100 * dif, Screen.height / 2 - 25 * dif, 200 * dif, 50 * dif), "Graphics"))
            {
                curMenu = 4;
            }
            if (GUI.Button(new Rect(Screen.width / 2 - 100 * dif, Screen.height / 2 + 30 * dif, 200 * dif, 50 * dif), "Back"))
            {
                curMenu = 0;
            }
        }
        else if (curMenu == 3)
        {
            AudioListener.volume = GUI.HorizontalSlider(new Rect(Screen.width / 2 - 100 * dif, Screen.height / 2 - 25 * dif, 200 * dif, 50 * dif), AudioListener.volume, 0f, 1f);
            GUI.Label(new Rect(Screen.width / 2 - 100 * dif, Screen.height / 2 - 45 * dif, 200 * dif, 50 * dif), "Volume: " + (AudioListener.volume * 100).ToString("F0") + " %");
            if (GUI.Button(new Rect(Screen.width / 2 - 100 * dif, Screen.height / 2 + 30 * dif, 200 * dif, 50 * dif), "Back"))
            {
                curMenu = 2;
            }
        }
        else if (curMenu == 4)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 75 * dif, Screen.height / 2 - 25 * dif, 150 * dif, 50 * dif), settings[curIndex]))
            {
                QualitySettings.SetQualityLevel(curIndex);
            }
            if (GUI.Button(new Rect(Screen.width / 2 - 100 * dif, Screen.height / 2 - 25 * dif, 20 * dif, 50 * dif), "←"))
            {
                if (curIndex != 0) curIndex--;
                else curIndex = settings.Length - 1;
            }

            if (GUI.Button(new Rect(Screen.width / 2 + 80 * dif, Screen.height / 2 - 25 * dif, 20 * dif, 50 * dif), "→"))
            {
                if (curIndex != settings.Length - 1) curIndex++;
                else curIndex = 0;
            }

            //I only made switching between quality settings. You can make it much more complex if you want.
            if (GUI.Button(new Rect(Screen.width / 2 - 100 * dif, Screen.height / 2 + 30 * dif, 200 * dif, 50 * dif), "Back"))
            {
                curMenu = 2;
            }
        }
        else if (curMenu == 5)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 75 * dif, Screen.height / 2 - 25 * dif, 150 * dif, 50 * dif), maps[curMapIndex].name))
            {
            }
            if (GUI.Button(new Rect(Screen.width / 2 - 100 * dif, Screen.height / 2 - 25 * dif, 20 * dif, 50 * dif), "←"))
            {
                if (curMapIndex != 0) curMapIndex--;
                else curMapIndex = maps.Length - 1;
            }

            if (GUI.Button(new Rect(Screen.width / 2 + 80 * dif, Screen.height / 2 - 25 * dif, 20 * dif, 50 * dif), "→"))
            {
                if (curMapIndex != maps.Length - 1) curMapIndex++;
                else curMapIndex = 0;
            }

            if (GUI.Button(new Rect(Screen.width / 2 - 100 * dif, Screen.height / 2 + 30 * dif, 200 * dif, 50 * dif), "Back"))
            {
                curMenu = 1;
            }
        }
    }

    void OnServerInitialized()
    {
        networkView.RPC("LoadLevel", RPCMode.AllBuffered, maps[curMapIndex].levelName);
        Debug.Log("Server initialized and ready");
    }
    [RPC]
    void LoadLevel(string level)
    {
        Network.isMessageQueueRunning = false;
        Application.LoadLevel(level);
    }

    void OnConnectedToServer()
    {
        Debug.Log("Connected to server");
        Network.isMessageQueueRunning = true;
    }
}

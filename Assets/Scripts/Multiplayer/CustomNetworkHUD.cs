using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Mirror.Discovery;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class CustomNetworkHUD : MonoBehaviour
{
    public CustomNetworkDiscovery networkDiscovery;
    public Dictionary<long, DiscoveryResponse> discoveredServers = new Dictionary<long, DiscoveryResponse>();
    public GameObject gamefinderCanvas;
    [Space]
    public Vector2 scrollViewPos = Vector2.zero;
    public Rect area1 = new Rect(0, 0, 300, 100);
    public Rect stopArea;
    [Space]
    public GUIStyle style ;
    public GUIStyle lablelStyle;
    public void LoadSceneLocally(string sceneName){
        SceneManager.LoadScene(sceneName);
    }
    private void OnGUI() {
            if (NetworkManager.singleton == null)
                return;
                
            MultiplayerWaitVisibility();
            if (NetworkServer.active || NetworkClient.active){//is either host or client
                PauseMenuVisiblity();
                AttemptingConn();
                DisableHostButton();
                return;
            }

            if (!NetworkClient.isConnected && !NetworkServer.active && !NetworkClient.active){
            PauseMenu.InGameSession = false;
            EnableHostButton();
            } 
            }
    //Check if it's trying to connect
    void AttemptingConn(){
            if (NetworkClient.active && !NetworkClient.isConnected){
                //Client is'nt added onto the server
                GUILayout.BeginArea(area1);
                GUILayout.Label("Connecting to [" + NetworkManager.singleton.networkAddress + "]...",lablelStyle);
                if (GUILayout.Button("Cancel",GUILayout.Width(70)))
                {
                    NetworkManager.singleton.StopClient();
                }
                GUILayout.EndArea();
            }
    }
    public Button myprefab;
    //you can change tis only to a transform if you wont need the game object lattter
    public GameObject viewportContent;
    public TMPro.TMP_Text noOfServers;
    private void UpdateServersListGUI() {
            //Remove existing server list first
            foreach (Transform child in viewportContent.transform) {
            GameObject.Destroy(child.gameObject);
         }
            noOfServers.text = $"Found [<#00FF00>{discoveredServers.Count}</color>] Games";
            foreach (DiscoveryResponse info in discoveredServers.Values)
            {
             Button serverButton = Instantiate(myprefab,viewportContent.transform);
             serverButton.GetComponentInChildren<TMPro.TMP_Text>().text = info.hostName;
             serverButton.GetComponent<ServerButtonInfo>().info = info;
             serverButton.onClick.AddListener(delegate () {this.Connect(info);});
            }

    }
    void PauseMenuVisiblity()
        {
            PauseMenu.InGameSession = true;
        }
    public Canvas multtiplayerWaitStatus;
    void MultiplayerWaitVisibility(){
        if(!PauseMenu.InGameSession)return;
        if(NetworkManager.singleton.numPlayers!=2 && NetworkServer.active && NetworkClient.isConnected){
            multtiplayerWaitStatus.gameObject.SetActive(true);
        }
        else{
            multtiplayerWaitStatus.gameObject.SetActive(false);
        }
    }
    public NetworkManager manager;
    public void StopGame(){
            if (NetworkServer.active && NetworkClient.isConnected)
            {
                    NetworkManager.singleton.StopHost();
            }
            // stop client if client-only
            else if (NetworkClient.isConnected)
            {
                    NetworkManager.singleton.StopClient();
            }
    }
    void EnableHostButton(){
        gamefinderCanvas.SetActive(true);
    }
    void DisableHostButton(){
        gamefinderCanvas.SetActive(false);
    }

    private void Start() {
    //Refrence to the network discovery was not set
    networkDiscovery = GetComponent<CustomNetworkDiscovery>();
    int[] options = new int[]{60,30};
    NetworkManager.singleton.serverTickRate = options[PlayerPrefs.GetInt("TickRate")]; 
    }
    // LAN Host
    public void HostGame(){
                discoveredServers.Clear();
                NetworkManager.singleton.StartHost();
                networkDiscovery.AdvertiseServer();
                
    }
    public void FindGame(){
            discoveredServers.Clear();
            networkDiscovery.StartDiscovery();
        }
    void Connect(DiscoveryResponse info)
        {   
            NetworkManager.singleton.StartClient(info.uri);
        }
    public void OnDiscoveredServer(DiscoveryResponse info)
        {
            // Note that you can check the versioning to decide if you can connect to the server or not using this method
            discoveredServers[info.serverId] = info;
            UpdateServersListGUI();
        }
    }
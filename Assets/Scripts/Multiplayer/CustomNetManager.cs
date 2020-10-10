using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mirror{
public class CustomNetManager : NetworkManager
{
        public Transform topPaddleSpawn;
        public Transform bottomPaddleSpawn;
        [SerializeField]
        public GameObject ball;
        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            // add player at correct spawn position
            Transform start = numPlayers == 0 ? topPaddleSpawn : bottomPaddleSpawn;
            GameObject player = Instantiate(playerPrefab, start.position, start.rotation);
            NetworkServer.AddPlayerForConnection(conn, player);
            if (numPlayers == 2)
            {
                ball = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "Ball"));
                NetworkServer.Spawn(ball);
                FindObjectOfType<GameSetup>().BallStartSequence();
            }

        }
    public override void OnServerDisconnect(NetworkConnection conn)
        {
            Debug.Log("Server Disconnected");
            if (ball != null)
                NetworkServer.Destroy(ball);
            // call base functionality (actually destroys the player)
            NetworkServer.DestroyPlayerForConnection(conn);
            //Stoping the host also calls the OnStopClient method
            //calling stop host even aftter the session was stopped by the manager causes a deadlock
            if (NetworkServer.active && NetworkClient.isConnected)
            {
                    StopHost();
            }
        }
    public override void OnClientConnect(NetworkConnection conn)
    {   
        base.OnClientConnect(conn);
    }
    #region Audio
    public override void OnStopClient(){
        //If a client disconnects
        AudioManager.instance.ThemeSong("Resume");
        AudioManager.instance.MultiplayerThemeSong("Pause");

    }
    #endregion
}
}
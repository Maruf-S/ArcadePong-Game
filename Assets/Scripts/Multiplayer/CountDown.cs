using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class CountDown : NetworkBehaviour
{
            PlayerNet localPlayer;
            public void OnSend()
        {
            // get our player

            //One way of doing it 
            // PlayerNet pleaseWork = NetworkClient.connection.identity.GetComponent<PlayerNet>();
            // pleaseWork.CmdSend("");
            localPlayer = ClientScene.localPlayer.GetComponent<PlayerNet>();
            localPlayer.VoteVoted();
        }
}

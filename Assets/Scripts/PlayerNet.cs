using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;
public class PlayerNet : NetworkBehaviour
{
        [SyncVar]
        public string playerName;
        public void VoteVoted(){
            if(!isLocalPlayer)return;
            //Set the Button interacteblity of each client to false so they can only vote once
            //You cant disable the button on the Game manager since that will run for all clients and disable all the buttons across all clients 
            FindObjectOfType<CountDown>().GetComponent<Button>().interactable = false;
            CmdCastVote();
        }
        [Command]
        //Command sent when a vote is voted
        public void CmdCastVote()
        { 
        FindObjectOfType<GameSetup>().voteCast();
        }
        void SendNameToServer(){
            if(!isLocalPlayer)return;
            CmdSetPlayerName(string.IsNullOrEmpty(PlayerPrefs.GetString("PlayerName"))? "NoobMaster" : PlayerPrefs.GetString("PlayerName"));
        }
        [Command]
        public void CmdSetPlayerName(string name){
            playerName = name;
        }
        public override void OnStartLocalPlayer()
        {
            SendNameToServer();
        }
        private void Update() {
                SetName();
        }
        public TextMesh nameText;
        void SetName()
        {
            nameText.text = playerName;
        }
}
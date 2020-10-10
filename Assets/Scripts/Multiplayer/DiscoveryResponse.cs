using System;
using System.Net;
using Mirror;

//For Debuging
[System.Serializable]
public class DiscoveryResponse : MessageBase
{
        public IPEndPoint EndPoint { get; set; }

        public Uri uri;

        //Broadcast the Server name 
        // nexttime load the name from player prefs
        public string hostName;
        // Prevent duplicate server appearance when a connection can be made via LAN on multiple NICs
        public long serverId;
        //add a host version here if you want it to be ignored by the client if they're not the same version
}
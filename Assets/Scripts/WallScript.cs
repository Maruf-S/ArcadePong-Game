using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

    public class WallScript : NetworkBehaviour
{
    [ServerCallback]
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.name == "Ball(Clone)"){
            FindObjectOfType<GameSetup>().Scored(this.gameObject.name);
        }
    }

}


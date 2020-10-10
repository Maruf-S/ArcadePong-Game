using UnityEngine;
using Mirror;
public class BallManager : NetworkBehaviour
{   Vector2 curVelocity;
    [ClientRpc]
    void RpcBallBounceAudio(){
    AudioManager.instance.Play("BallCollide0",1);
    }
    [ClientRpc]
    void RpcPaddleAudio(){
        AudioManager.instance.Play("PaddleSound0",1);
    }
    [Server]
    private void OnCollisionEnter2D(Collision2D other) {
        curVelocity = GetComponent<Rigidbody2D>().velocity;
        // if(other.collider.tag == "goals")return;
        // RpcBallBounceAudio();
        if(other.collider.tag == "player"){
            //////////////////////////////////////////////
            //////////////////////////////////////////////
        ContactPoint2D[] contacts = new ContactPoint2D[2];
        int numContacts  = other.GetContacts(contacts);
        for (int i = 0; i < numContacts; i++)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2( curVelocity.x / 3 + ((contacts[i].point-other.gameObject.GetComponent<Rigidbody2D>().position).x * 7f) ,curVelocity.y);
        }
        if(curVelocity.y<=0){
            GetComponent<Rigidbody2D>().velocity += new Vector2(0,-1.5f);
        // Debug.Log("Added negative Velocity");
        }
        else{
        // Debug.Log("Added Postive Velocity");
            GetComponent<Rigidbody2D>().velocity += new Vector2(0,1.5f);
        }
        RpcPaddleAudio();
        }
        else{
            RpcBallBounceAudio();
        }
    }
}
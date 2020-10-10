using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePlayerBallManager : MonoBehaviour
{   
    public Vector2 velocityIncrease = new Vector2(0,1.5f);
    // private Vector3 lastPos;
    
    public Vector3 Direction;
    Vector2 curVelocity;
    private void Update() {
    }
    private void OnCollisionEnter2D(Collision2D other) {
        curVelocity = GetComponent<Rigidbody2D>().velocity;
        if(other.collider.tag == "player"){
//////////////////THE PHYSICS BREAKS IF THE BALL MOVES ABOVE THIS SPPED SO TO FIT IT IN THE FIXED UPDATE CYCLE SO WE KEEP IT AROUND 30///////////////////////
        if(curVelocity.y>30){
            Debug.LogError("Critical speed  " +curVelocity.y);
            ///////////////////////////////////////////?????????????///////////////////////////////////
            GetComponent<Rigidbody2D>().velocity = new Vector2(curVelocity.x,27);            
        }
        ContactPoint2D[] contacts = new ContactPoint2D[2];
        //?//
        int numContacts  = other.GetContacts(contacts);
        for (int i = 0; i < numContacts; i++)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2( curVelocity.x / 3 + ((contacts[i].point-other.gameObject.GetComponent<Rigidbody2D>().position).x * 8f) ,curVelocity.y);
        }
        if(curVelocity.y<=0){
            gameObject.GetComponent<Rigidbody2D>().velocity += (velocityIncrease*-1);
        // Debug.Log("Added negative Velocity");
        }
        else{
        // Debug.Log("Added Postive Velocity");
         gameObject.GetComponent<Rigidbody2D>().velocity += velocityIncrease;
        }
        //Play a collusion audio
        AudioManager.instance.Play("PaddleSound0",1);
        // FindObjectOfType<AudioManager>().Play("PaddleSound0",Mathf.Log10(gameObject.GetComponent<Rigidbody2D>().velocity.y)-0.2f);
        }
        else{
            AudioManager.instance.Play("BallCollide0",1);
        }
    }
}

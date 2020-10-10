using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePlayerAI : MonoBehaviour
{
    //NOT REALLY AN AI
    public int moveSpeed  = 500;
    public GameObject ball;
    private Vector2 ballPos;
    public float timescale;
    public int level;
    private void Awake() {
        ///////////////////////////////
        ///////////////////////////////
        Time.timeScale = 1;
        SetLevel();
        /////////////////////////////////
    }
    void SetLevel(){
        //Level of difficulty
        level = Mathf.RoundToInt(PlayerPrefs.GetFloat("Difficulty"));
        Debug.Log("Playing Level + "+level );
        switch (level)
        {
            case(0):
            moveSpeed = 4;
            break;
            case(1):
            moveSpeed = 5;
            break;
            case(2):
            moveSpeed = 7;
            break;
            case(3):
            moveSpeed = 7;
            Time.timeScale = 1.3f;
            timescale = Time.timeScale;
            break;
        }
    }
    private void FixedUpdate() {
        Move();
    }
    void Move(){
        if(!ball){
        ball = GameObject.FindGameObjectWithTag("Ball");
        }
        // if(ball.GetComponent<SinglePlayerBallManager>().Direction == Vector3.right){

        // }
        if(ball.GetComponent<Rigidbody2D>().velocity.y<0)return;
        // ballPos = ball.transform.position;
        //Not a correct usage for Lerp but since it's only one Lerp it won't affect perfromace
        transform.position = Vector3.Lerp(transform.position, new Vector3(ball.transform.position.x,transform.position.y,0), moveSpeed*Time.deltaTime);
        /////TRIAL
        // if(transform.position.x > ballPos.x){

        //     transform.position += new Vector3(-moveSpeed*Time.deltaTime,0,0);
        // }
        // if(transform.position.x < ballPos.x){
        //     transform.position += new Vector3(moveSpeed*Time.deltaTime,0,0);
        // }
}
}
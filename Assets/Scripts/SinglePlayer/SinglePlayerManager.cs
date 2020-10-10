using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SinglePlayerManager : MonoBehaviour 

{
    // Start is called before the first frame update
        public Camera mainCam;
        public Transform Player01;
        public Transform Player02;
        public BoxCollider2D topWall;
        public BoxCollider2D bottomWall;
        public BoxCollider2D rightWall;
        public BoxCollider2D leftWall;
        public ParticleSystem celebration; 
//Change this to Start
    private void Start() {
        Player01.transform.position = new Vector2(Player01.transform.position.x,mainCam.ScreenToWorldPoint(new Vector3(0f, 20f, 0f)).y);
        Player02.transform.position = new Vector2( Player02.transform.position.x,mainCam.ScreenToWorldPoint(new Vector3(0f, Screen.height -20f, 0)).y);
    }
    void Resize(){
        //Resize the walls to the edge of the cameraview
        topWall.size  = new Vector2(mainCam.ScreenToWorldPoint(new Vector3(Screen.width*2f,0f,0f)).x,1f);
        topWall.offset = new Vector2(0f,mainCam.ScreenToWorldPoint(new Vector3(0f,Screen.height,0f)).y+0.5f);        
        
        bottomWall.size = new Vector2(mainCam.ScreenToWorldPoint(new Vector3(Screen.width * 2, 0f, 0f)).x, 1f);
        bottomWall.offset = new Vector2(0f, mainCam.ScreenToWorldPoint(new Vector3(0f, 0f, 0f)).y - 0.5f);

        leftWall.size = new Vector2(1f, mainCam.ScreenToWorldPoint(new Vector3(0f, Screen.height * 2f, 0f)).y); ;
        leftWall.offset = new Vector2(mainCam.ScreenToWorldPoint(new Vector3(0f, 0f, 0f)).x - 0.5f, 0f);

        rightWall.size = new Vector2(1f, mainCam.ScreenToWorldPoint(new Vector3(0f, Screen.height * 2f, 0f)).y);
        rightWall.offset = new Vector2(mainCam.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f)).x + 0.5f, 0f);
    }
    //End line of resizing walls

    public Rigidbody2D ballRB;
    public Text gameCountDown;
    public Canvas gameCountdownCanvas;
    private void Awake() {
        BallStartSequence();
    }
    IEnumerator countDownUIcoroutine(){
        //Simulate the ball aftter the walls have been snapped into postion
        ballRB.simulated = true;
        //Score board becomes visible soon as match starts
        scoreBoardCanvas.gameObject.SetActive(true);
        gameCountdownCanvas.gameObject.SetActive(true);
        int countDown = 3;
        while (countDown>0)
        {
            gameCountDown.text = countDown.ToString();
            yield return new WaitForSeconds(1f);
            countDown--;
        }
        gameCountDown.text = "Go!!";
        yield return new WaitForSeconds(0.3f);
        gameCountdownCanvas.gameObject.SetActive(false);
        BallServe();
    }
    void BallServe(){
        ballMovementAllowed = true;
        float randomNumber = Random.Range(0, 2);
        AudioManager.instance.Play("Ready",1);
        if (randomNumber <= 0.5) {
            ballRB.AddForce(new Vector2 (10, 80));
        }
        else {
            ballRB.AddForce(new Vector2 (-10, -80));        
    }
    }

    public Canvas scoreBoardCanvas;
    public Text scoreTop;
    public Text scoreBottom;
    public Canvas gameOver;
    public TMP_Text gameOverText;
    protected Vector2Int MatchScore = new Vector2Int(0,0);

    public void Scored(string wallName){
        //PLAY SCORED AUDIO
        AudioManager.instance.Play("Scored",1f);
        if(wallName=="bottomWall"){
            MatchScore.x +=1;
        }
        else if (wallName =="topWall"){
            MatchScore.y+=1;
        }
        RpcUpdateScore(MatchScore);
        if(MatchScore.x>=5 || MatchScore.y>=5){
            SnapBallToOrigin();
            RpcShowGameOver();
        }
        else{
            StartCoroutine(scoredCoroutine());
        }
    }
    bool onWaitForServe = false;
    bool ballMovementAllowed = false;    
    //Wait for the player to make a move to serve the ball aftter a score
    private void Update() {
        Resize();
        if(onWaitForServe && (Input.GetAxisRaw("Horizontal")!=0)){
            onWaitForServe = false;
            BallServe();
        }
        if(!ballMovementAllowed && ballRB.velocity!=Vector2.zero){
            //TMP solution because the physics keeps breaking
            Debug.LogWarning("Glitch Avoided");
            SnapBallToOrigin();
        }
    }
    //Coroutine that runs when a goal is scored
    IEnumerator scoredCoroutine(){
    
    SnapBallToOrigin();
    ballMovementAllowed = false;
    yield return new WaitForSeconds(1f);
    // SnapBallToOrigin();
    onWaitForServe = true;
    }
    void SnapBallToOrigin(){
        ballRB.position = Vector2.zero; 
        ballRB.velocity = Vector2.zero;
        ballRB.angularVelocity = 0f;
        // ballNet.transform.position = Vector3.zero;
    }
    private void RpcUpdateScore(Vector2 MatchScore){
        scoreTop.text = MatchScore.x.ToString();
        scoreBottom.text = MatchScore.y.ToString();
    }
    void RpcShowGameOver(){
        //We don't want the ballPhysics glitch aftter the match ends too
        ballMovementAllowed = false;
        gameOver.gameObject.SetActive(true);
        FindObjectOfType<TextAnimator>().DoAnimation();
        if(MatchScore.y>MatchScore.x){
            Debug.Log("You won!!!!!");
            celebration.Play();
            AudioManager.instance.Play("Win",1);
            return;
        }
        Debug.Log("You Lost");
    }
    void RpcHideGameOver(){
        gameOver.gameObject.SetActive(false);
    }
    //End of line for ingame mechanics of the ball
    // Restarting the game
    public void BallStartSequence(){

        MatchScore = Vector2Int.zero;
        StartCoroutine(countDownUIcoroutine());
    }
    public void RetryGame(){
        MatchScore = Vector2Int.zero;
        RpcUpdateScore(MatchScore);
        SnapBallToOrigin();
        RpcHideGameOver();
        BallStartSequence();
    }
}

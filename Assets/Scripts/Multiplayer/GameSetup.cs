using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;
public class GameSetup : NetworkBehaviour 

{
    ///////////////////////////////////////////////////////////////EXPIREMTAL????????????????????????????????
    // #region Experimental
    // [Server]
    // private void Start() {
    //     FindObjectOfType<CustomNetRoomManager>().doStuff();
    // }
        
    // #endregion
    public Camera mainCam;
    public Transform Player01;
    public Transform Player02;
    GameObject ballNet;
    public Text gameCountDown;
    public Canvas gameCountdownCanvas;
    bool ballMovementAllowed = true;
    public Rigidbody2D Ball_RB;
    public ParticleSystem celebration; 
    #region TemporaryFixForAPhysicsGlitch
    [ServerCallback]
    void TempGlitchFix(){
        if(ballMovementAllowed)return;
        if(!ballNet){
            Debug.LogWarning("An error had occoured but it's been possibly fixed");
            //Resseting in case of a network error
            ballMovementAllowed = true;
            return;
        }
        if(!Ball_RB){
                Ball_RB = ballNet.GetComponent<Rigidbody2D>();
        }
        if(!ballMovementAllowed && Ball_RB.velocity!=Vector2.zero){
            //TMP solution because the physics keeps breaking when it gets enabled
            Debug.LogWarning("Glitch Avoided Report");
            SnapBallToOrigin();
        }
    }
    private void Update() {
        TempGlitchFix();
    }
    #endregion
    [ClientRpc]
    //Clean up the code here by using the new !isclientonly attrib to check wether this runs on the host only
    void RpcStartRoutineForClient(){
        StartCoroutine(countDownUIcoroutine(false));
    }
    [Server]
    public void BallStartSequence(){
        //Remove this and create some kind of way to continue the game from a saved score so a client can countinue a game from a saved score
        MatchScore = Vector2Int.zero;
                ////////////////
                ///////////////
                //////
                ///////////////
                ///////////////
                ////
                ////
                ///
                ////
        StartCoroutine(countDownUIcoroutine(true));
        RpcStartRoutineForClient();
        RpcSetMultiplayerTheme();
    }
    IEnumerator countDownUIcoroutine(bool isHost){
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
        if(isHost){BallServe(false,null);}
    }
    [ServerCallback]
    void BallServe(bool alreadySimulated,string scorer){
        ballNet =  FindObjectOfType<CustomNetManager>().ball;
        if(!ballNet){
            return;
        }
        Rigidbody2D rbBall = ballNet.GetComponent<Rigidbody2D>();

        if(!alreadySimulated){
          rbBall.simulated = true;
            }
        ballMovementAllowed = true;
        if(scorer == null){
        //First time serving, just do random
        float randomNumber = Random.Range(0, 2);
        if (randomNumber <= 0.5) {
            rbBall.AddForce(new Vector2 (Random.Range(-5, 5),Random.Range(51, 61)));
        }
        else {
            rbBall.AddForce(new Vector2 (Random.Range(-5, 5),Random.Range(-61, -51)));        
         }
         }
        else if(scorer =="bottomWall"){
            //top player scored so he gets the serve
            rbBall.AddForce(new Vector2 (Random.Range(-5, 5),Random.Range(51, 61)));
        }
        else if(scorer =="topWall"){
            rbBall.AddForce(new Vector2 (Random.Range(-5, 5),Random.Range(-61, -51)));    
        }
        RpcScoredSound();
}
    //End Line of the ServeBall
    public Canvas scoreBoardCanvas;
    public Text scoreTop;
    public Text scoreBottom;
    public Canvas gameOver;
    public TMP_Text gameOverText;
    //You can use either a hook or use a RPC function everytime the score is updated on the Server
    [SyncVar(hook = nameof(MatchScoreUpdateClient))]
    public Vector2Int MatchScore = new Vector2Int(0,0);

    //Using a Hook to update the matchscore on clients
    void MatchScoreUpdateClient(Vector2Int oldVal, Vector2Int newVal){
        scoreTop.text = newVal.x.ToString();
        scoreBottom.text = newVal.y.ToString();
    }
    [ServerCallback]
    public void Scored(string wallName){
        //Changing the x or y of the Vector2Int MatchScore wouldn't trigger the Hook function because the Object wouldn't be changed
        //So we have to Create a New object Everytime the Score Changes
        Vector2Int MatchScoreOld = MatchScore;
        if(wallName=="bottomWall"){
            MatchScore = new Vector2Int(MatchScoreOld.x +=1,MatchScoreOld.y);
        }
        else if (wallName =="topWall"){
        MatchScore = new Vector2Int(MatchScoreOld.x,MatchScoreOld.y+=1);
        }

        // RpcUpdateScore(MatchScore);

        //Check if the Game is over
        if(MatchScore.x>=5 || MatchScore.y>=5){
            SnapBallToOrigin();
            RpcShowGameOver();
        }
        else{
            //Pass on the wall name so the ball will be served to the winner
            StartCoroutine(scoredCoroutine(wallName));
        }
    }
    //Coroutine that runs when a goal is scored
    IEnumerator scoredCoroutine(string wallName){
    SnapBallToOrigin();
    yield return new WaitForSeconds(0.9f);
    RpcBallReadySound();
    BallServe(true,wallName);
    }
    [ServerCallback]
    void SnapBallToOrigin(){
        ballNet =  FindObjectOfType<CustomNetManager>().ball;
        // ballNet = FindObjectOfType<CustomNetRoomManager>().ball;
        Rigidbody2D rbBall = ballNet.GetComponent<Rigidbody2D>();
        ballMovementAllowed = false;
        rbBall.velocity = Vector3.zero;
        rbBall.position = Vector3.zero;
        rbBall.angularVelocity = 0f;
    }
    /////////////// Another Way to Sync the MatchScore from the Server to Clients //////////////////////////
    // [ClientRpc]
    // private void RpcUpdateScore(Vector2 MatchScore){
    //     scoreTop.text = MatchScore.x.ToString();
    //     scoreBottom.text = MatchScore.y.ToString();
    // }
    [ClientRpc]
    void RpcShowGameOver(){
        gameOver.gameObject.SetActive(true);
        gameOverText.enabled = true;
        FindObjectOfType<TextAnimator>().DoAnimation();
        //WHO WON?
//        MatchScore.y represents the hosts score since the host is allways spawned below
        //did the host win?
        if(isServer && isClient && MatchScore.y > MatchScore.x){
            Debug.LogWarning("You won");
            celebration.Play();
            AudioManager.instance.Play("Win",1);
            return;
        }
        //did the client win?
        else if(isClientOnly && MatchScore.x>MatchScore.y){
            Debug.LogWarning("You Won");
            celebration.Play();
            AudioManager.instance.Play("Win",1);
            return;
        }
        Debug.LogWarning("Loser");
    }
    [ClientRpc]
    void RpcHideGameOver(){
        gameOver.gameObject.SetActive(false);
    }
    //End of line for ingame mechanics of the ball
    // Restarting the game
    [Server]
    //Reset all stats and hide the UI elements for rematch
    void RetryGame(){
        MatchScore =Vector2Int.zero;
        votes = 0;
        RpcAllowVotingForAll();
        //Doesn't need a RPC function since the ball is only simulated on the server
        SnapBallToOrigin();
        RpcHideGameOver();
        BallStartSequence();
    }

    //Voting for remach
    public TMP_Text voteToRetryText;
    public Button voteToRetryButton;
    [SerializeField]
    [SyncVar(hook = nameof(NoOfVotesChanged))]
    int votes;
    [Server]
    //Sent from the player prefab
    public void voteCast(){
    //Changing this variables on the server triggers the hook NoOfVotesChanged
    votes++;
    //Check if both players want a rematch
    if(votes>=2){
        Invoke("RetryGame",0.5f);
    }
    }
    [ClientRpc]
    //Enables the interacteblity of the voting buttons for all clients
    void RpcAllowVotingForAll(){
        FindObjectOfType<CountDown>().GetComponent<Button>().interactable = true;
    }
    //Listner for when a vote is cast
    void NoOfVotesChanged(int oldVa, int newVal){
        votes = newVal;
        voteToRetryText.text = "Vote to Retry "+votes+"/2";
    }
    ///End of line for voting

    //On DIsconnect
    private void OnEnable() {
                    CountDown voteButton = FindObjectOfType<CountDown>(); 
                if(!voteButton)return;
                voteButton.GetComponent<Button>().interactable = true;
    }
    public void CleanUp(){
        // votes = 0;
        MatchScoreUpdateClient(Vector2Int.zero,Vector2Int.zero); 
        NoOfVotesChanged(0,0);
        MatchScore = Vector2Int.zero;
        gameOver.gameObject.SetActive(false);
        gameCountdownCanvas.gameObject.SetActive(false);
        scoreBoardCanvas.gameObject.SetActive(false);
    }
    //Since the GameManager has a network identity attached it will be enabled only when it becomes a server or a client
    //So when it gets disabled by the network manager Just clean up the UI elements

    // Try Loading  the scene as new
    private void OnDisable() {
        CleanUp();
    }
    
    #region Sound
    
    [ClientRpc]
    void RpcBallReadySound(){
        AudioManager.instance.Play("Ready",1);
    } 
    [ClientRpc]
    void RpcScoredSound(){
        AudioManager.instance.Play("Scored",1);
    }
    [ClientRpc]
    public void RpcSetMultiplayerTheme(){
        AudioManager.instance.ThemeSong("Pause");
        AudioManager.instance.MultiplayerThemeSong("Play");
    }
    #endregion
    public Transform hostParticle;
    public Transform clientParticle;
    #region Particle System
    public override void OnStartClient(){
        if (NetworkServer.active && NetworkClient.isConnected)
            {
            Debug.LogWarning("Server then");
            celebration.transform.position = hostParticle.position;
            celebration.transform.rotation = hostParticle.rotation;
            }
            // stop client if client-only
            else if (NetworkClient.isConnected)
            {
            Debug.LogWarning("ClientOnly");
            celebration.transform.position = clientParticle.position;
            celebration.transform.rotation = clientParticle.rotation;
            }
    }
    #endregion
}

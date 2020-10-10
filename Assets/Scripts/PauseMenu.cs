using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour

{
    public static bool InGameSession;
    public static bool IsGamePaused;
    public float timescale = 0;
    public bool singlePlayerSession;
    public Canvas PauseCanvas;
    private void Awake() {
        InGameSession = singlePlayerSession;
    }
    // Update is called once per frame
    void Update()
    {
        //Don't show the pause Menu if not in game session
        if(!InGameSession)return;
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(IsGamePaused){Resume();}
            else{
                Pause();
            }
        }
    }
    public void Resume(){
        PauseCanvas.gameObject.SetActive(false);
        if(singlePlayerSession){
            Time.timeScale = FindObjectOfType<SinglePlayerAI>().timescale;
        }
        else{
            Time.timeScale = 1;
        }
        IsGamePaused = false; 
    }
    void Pause(){
        PauseCanvas.gameObject.SetActive(true);
        Time.timeScale = timescale;
        IsGamePaused=!IsGamePaused;
    }
    public void GoToMenu(){
        IsGamePaused = false;
        Time.timeScale = 1f;
        if(!singlePlayerSession){
            FindObjectOfType<CustomNetworkHUD>().StopGame();
            Resume();
            //go to multiplayer menu
        }
        else{
        SceneManager.LoadScene("Menu");
        }
        // stopserver is alredy called
        Debug.LogWarning("Your Scene Here");    
        }
    public void Quit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}

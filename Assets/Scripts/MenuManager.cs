using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using UnityEngine.Audio;
public class MenuManager : MonoBehaviour
{
    Resolution[] resolutions;
    public GameObject Menu;
    public AudioMixer mixer;
    private void Awake() {
    // LoadDefaultValues();
    // Time.timeScale = 1f;
    //You need this if u came from a paused menu becuase to pause a game time scale was made 0
    //
    }
    private void Start() {
            resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToArray();
    }
    private static int BoolToInt(bool value){
			return value ? 1 : 0;
		}
    private static bool IntToBool(int value){
			return value == 0 ? false : true;
		}
    public void PlayMultiPlayer(){
        SceneManager.LoadScene("MultiplayerScene");
    }
    public void playSinglePlayer(){
        SceneManager.LoadScene("SinglePlayerScene");
    }

    #region UpdateSettings
    public void UpdatePlayerName(string name){
        PlayerPrefs.SetString("PlayerName",name);
    }
    public void UpdateDifficulty(float difficulty){
        Debug.Log(difficulty);
        PlayerPrefs.SetFloat("Difficulty",Mathf.RoundToInt(difficulty));
    }
    public void UpdateGraphicsQuality(float index){
        PlayerPrefs.SetFloat("GraphicsQuality",Mathf.RoundToInt(index));
    }
    // private void Start() {
    //     UpdateScreenResolution(1);
    // }
    public void UpdateScreenResolution(int index){
        PlayerPrefs.SetInt("Resolution",index);
        }
    public void UpdateFullScreen(bool isFullScreen){
        PlayerPrefs.SetInt("FullScreen",BoolToInt(isFullScreen));
    }
    public void UpdateMusicLevel(float level){
        PlayerPrefs.SetFloat("MusicLevel",level);
    }
    public void UpdateSoundLevel(float level){
        PlayerPrefs.SetFloat("SoundEffectsLevel",level);
    }
    public void UpdateServerTickRate(int tickRate){
        //frick
        // if(tickRate == 0)
        // {tickRate=60;}
        // else{tickRate = 30;}
        PlayerPrefs.SetInt("TickRate",tickRate);
    }
    public void UpdateTelepathyPort(int port){
        // //damit
        // Debug.Log(x[port]);
        PlayerPrefs.SetInt("Port",port);
    }
    #endregion
    #region DefaultValues
    public void LoadDefaultValues(){
        UpdatePlayerName("NoobMaster"+Random.Range(1,99));
        UpdateDifficulty(1);
        //huh
        UpdateGraphicsQuality(1);
        UpdateScreenResolution(0);
        UpdateFullScreen(true);
        UpdateMusicLevel(1);
        UpdateSoundLevel(1);
        UpdateServerTickRate(0);
        UpdateTelepathyPort(0);
        //might have to apply res and stuff

    }
    #endregion
    #region Graphics
    public void SetQualityLevel(float index)
        {
            SetQualityLevel(Mathf.RoundToInt(index));
        }

    public void SetQualityLevel(int index){
			if (QualitySettings.GetQualityLevel () != index) {
				QualitySettings.SetQualityLevel (index,true);
				PlayerPrefs.SetFloat ("GraphicsQuality",index);
			}
		}
    public void setVsync(bool value){
        QualitySettings.vSyncCount = BoolToInt(value);
    }
    #endregion
    private void Update() {
        // Debug.Log(PlayerPrefs.GetString("PlayerName"));
    //     Debug.Log(PlayerPrefs.GetInt("Port"));
     }
    public void ChangeResolution(){
        int index = PlayerPrefs.GetInt("Resolution");
        Screen.SetResolution (resolutions[resolutions.Length-1-index].width,
                                resolutions[resolutions.Length-1-index].height,
                                IntToBool(PlayerPrefs.GetInt("FullScreen")));
    }
    #region Links
    public void SendEmail ()
     {
      string email = "bluescenes20@gmail.com";
      string subject = MyEscapeURL("Feedback on ArcadePong");
      string body = MyEscapeURL("");
      Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
     }
     string MyEscapeURL (string url)
     {
      return WWW.EscapeURL(url).Replace("+","%20");
     }
     public void openGithubLink(){
         Application.OpenURL("https://github.com/Maruf-S/ArcadePong-Game");
     }
    #endregion
    public void Quit(){
        Application.Quit();
    }
}
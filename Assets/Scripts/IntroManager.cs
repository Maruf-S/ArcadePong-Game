using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
public class IntroManager : MonoBehaviour
{
    public AudioMixer mixer;
    Resolution[] resolutions;
    // Start is called before the first frame update
    void Start()
    {
        resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToArray();
        Debug.Log("FirstTimeOpening:" + !IntToBool(PlayerPrefs.GetInt("InverseFirstTimeOpening")));
        if(!IntToBool(PlayerPrefs.GetInt("InverseFirstTimeOpening"))){
            //Loading Screen res// Only the first time its opened
            LoadDefaultValues();
            ChangeResolution();
            PlayerPrefs.SetInt("InverseFirstTimeOpening",1);
        }
        Screen.fullScreen = IntToBool(PlayerPrefs.GetInt("FullScreen"));
        LoadPreviousValues();
        AudioManager.instance.ThemeSong("Play");
    }
    private void Update() {
        if(Input.anyKeyDown){
            LoadScene("Menu");
        }
    }
    public void LoadScene(string sceneName){
        SceneManager.LoadScene(sceneName);
    }
    void LoadPreviousValues(){
    setVsync(PlayerPrefs.GetInt("Vsync"));
    //Loading Audio
    mixer.SetFloat("SoundEffectsVolume",Mathf.Log10(PlayerPrefs.GetFloat("SoundEffectsLevel"))*20);  
    mixer.SetFloat("MusicVolume",Mathf.Log10(PlayerPrefs.GetFloat("MusicLevel"))*20);
    //Loading Graphics
    int qualityIndex = PlayerPrefs.GetInt ("GraphicsQuality", QualitySettings.GetQualityLevel ());
	SetQualityLevel (qualityIndex);
    }
    #region DefaultValues
    public void LoadDefaultValues(){
        //9/10/2020/
        Debug.LogWarning("Loading Default Values");
        UpdatePlayerName("NoobMaster"+Random.Range(1,99));
        UpdateDifficulty(1);
        UpdateScreenResolution(0);
        UpdateFullScreen(true);
        UpdateMusicLevel(0.15f);
        UpdateSoundLevel(0.4f);
        UpdateServerTickRate(1);
        UpdateTelepathyPort(0);
    }
    #endregion

    #region UpdateSettings
    public void UpdatePlayerName(string name){
        PlayerPrefs.SetString("PlayerName",name);
    }
    public void UpdateDifficulty(float difficulty){
        PlayerPrefs.SetFloat("Difficulty",Mathf.RoundToInt(difficulty));
    }
    public void UpdateGraphicsQuality(float index){
        Debug.Log("INDeX Of " + index);
        PlayerPrefs.SetFloat("GraphicsQuality",Mathf.RoundToInt(index));
    }
    public void UpdateScreenResolution(int index){
        PlayerPrefs.SetInt("Resolution",index);
        // var resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToArray();
        // Debug.Log("Resolution is + " + resolutions[resolutions.Length-1-PlayerPrefs.GetInt("Resolution")]);
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
        PlayerPrefs.SetInt("Port",port);
    }
    #endregion
    public void ChangeResolution(){
        int index = PlayerPrefs.GetInt("Resolution");
        Screen.SetResolution (resolutions[resolutions.Length-1-index].width,
                                resolutions[resolutions.Length-1-index].height,
                                IntToBool(PlayerPrefs.GetInt("FullScreen")));
    }
    #region Graphics
    public void setVsync(int value){
        QualitySettings.vSyncCount = value;
    }
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
    #endregion
    private static int BoolToInt(bool value){
			return value ? 1 : 0;
		}
    private static bool IntToBool(int value){
			return value == 0 ? false : true;
		}
}

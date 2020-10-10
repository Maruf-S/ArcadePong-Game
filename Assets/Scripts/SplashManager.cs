using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SplashManager : MonoBehaviour
{
    public float waitSeconds;
    // // Start is called before the first frame update
    // void Start()
    // {
    //     StartCoroutine(Wait());
    // }
    // IEnumerator Wait(){
    //     yield return new WaitForSeconds(waitSeconds);
    //     LoadIntro();
    // }
    public VideoPlayer vid;
 
 
    void Start(){
     vid = GetComponent<VideoPlayer>();
     vid.loopPointReached += CheckOver;

     }
    void CheckOver(VideoPlayer vp)
    {
     print  ("Video Is Over");
     LoadIntro();
    }
    // Update is called once per frame
    // void Update()
    // {
    //     if(Input.anyKeyDown){
    //         LoadIntro();
    //     }
    // }
    
    public void LoadIntro(){
        SceneManager.LoadScene("Intro");
    }
}

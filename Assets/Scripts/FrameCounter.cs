using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FrameCounter : MonoBehaviour
{
        public float UpdateInterval = 5.0f;
        private float m_LastInterval = 0;
        private int m_Frames = 0;
        private string htmlColorTag;
        private const string fpsLabel = "{0:2}</color> <#8080ff>";

        private TMP_Text m_TextMeshPro;
        private void Awake() {
            //Check if the user wants it visible
            if(PlayerPrefs.GetInt("FpsCounter")==0)
            gameObject.SetActive(false);
        }
        void Start()
        {
            m_TextMeshPro = GetComponentInChildren<TMP_Text>();
            m_LastInterval = Time.realtimeSinceStartup;
            m_Frames = 0;
        }

        void Update()
        {
            m_Frames += 1;
            float timeNow = Time.realtimeSinceStartup;
            if (timeNow > m_LastInterval + UpdateInterval)
            {
                // display two fractional digits (f2 format)
                float fps = m_Frames / (timeNow - m_LastInterval);
                if (fps < 20)
                    htmlColorTag = "<color=red>";
                else if (fps < 40)
                    htmlColorTag = "<color=yellow>";
                else
                    htmlColorTag = "<color=green>";
                //string format = System.String.Format(htmlColorTag + "{0:F2} </color>FPS \n{1:F2} <#8080ff>MS",fps, ms);
                //m_TextMeshPro.text = format;

                m_TextMeshPro.SetText(htmlColorTag + fpsLabel, fps );

                m_Frames = 0;
                m_LastInterval = timeNow;
            }
        }
}

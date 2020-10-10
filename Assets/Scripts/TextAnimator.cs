using UnityEngine;
using System.Collections;
using TMPro;
    public class TextAnimator : MonoBehaviour
    {
        private TMP_Text m_TextComponent;
        [SerializeField]
        float timeInSeconds = 0.5f;
        void Awake()
        {
            m_TextComponent = gameObject.GetComponent<TMP_Text>();
        }


        public void DoAnimation()
        {
            StartCoroutine(RevealCharacters(m_TextComponent));
        }


        /// Method revealing the text one character at a time.
        IEnumerator RevealCharacters(TMP_Text textComponent)
        {
            textComponent.ForceMeshUpdate();

            TMP_TextInfo textInfo = textComponent.textInfo;

            int totalVisibleCharacters = textInfo.characterCount; // Get # of Visible Character in text object
            int visibleCount = 0;
            textComponent.maxVisibleCharacters = 0;

            while (visibleCount<=totalVisibleCharacters)
            {
                textComponent.maxVisibleCharacters = visibleCount;
                yield return new WaitForSeconds(timeInSeconds);// How many characters should TextMeshPro display?
                visibleCount ++;
            }
            yield return timeInSeconds;
        }

    }
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class speechManager : MonoBehaviour
{
    
    static bool isSpeaking = false;
    public GameObject speechBubble;
    public TextMeshProUGUI speechText;
    SpriteRenderer sprite; 

    public TextAsset speechDetails;
    int speechCounter = 0;
    
    public class Speech
    {
        public float duration;
        public string to_say;
    }

    public class talkList
    {
        public Speech[] speechList;
    }

    public talkList speeches = new talkList();

    
    void Start()
    {
        sprite = speechBubble.GetComponent<SpriteRenderer>();
        speeches = JsonUtility.FromJson<talkList>(speechDetails.text);
        // Basic JSON read into a class array 
    }

    // Update is called once per frame
    void Update()
    {
        if (isSpeaking)
        {
            // isSpeaking is a bool I didn't know when to change
            // This would assumedly be implemented when more of the control flow of the character speaking is developed 
            StartCoroutine(MakeSpeak((speeches.speechList[speechCounter].duration)));
        }
    }

    IEnumerator MakeSpeak(float gh)
    {
        // If there's more in the character's dialogue array 
        if (!(speechCounter >= speeches.speechList.Length))
        {
            // turns on the speech bubble 
            sprite.color = new Color(1,1,1,1);
            // Says a thing
            speechText.text = speeches.speechList[speechCounter].to_say;
            // Waits for duration
            yield return new WaitForSeconds(gh);
            // Then makes the speech bubble transparent, unhits the bool and increments the speechCounter
            sprite.color = new Color(1,1,1,0);
            isSpeaking = false; 
            speechCounter++;
        }
        else // If there's not more in the character's dialogue box, it stalls indefinitely 
        {
            yield return new WaitForSeconds(9999);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneSwitch2 : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject transitionButton;
    public void ChangeScene()
    {
        SceneManager.LoadScene(0); 

        // Does some cleanup after the base scene is loaded

        // Unhits these bools, so a question can be generated correctly as soon as the quiz scene is reentered 
        loadQuestions.displayingQuestion = false;
        DisplayQuestion.updater = false;

        // Unlocks the buttons for when the scene is reentered 
        answerButtons.activeButtons = true;
    }
}

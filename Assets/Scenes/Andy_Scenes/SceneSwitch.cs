using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    public GameObject quizButton;
    public GameObject journalButton; 
    public void ChangeScene()
    {
        SceneManager.LoadScene(1);
    }

    public void changeOtherScene()
    {
        SceneManager.LoadScene(2);
    }
}

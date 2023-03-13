using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class journalTransitionOut : MonoBehaviour
{
    public GameObject cancelButton;

    public void changeScene()
    {
        SceneManager.LoadScene(0);
    }
}

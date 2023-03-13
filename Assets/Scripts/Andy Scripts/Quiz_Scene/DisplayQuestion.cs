// Displays the questions that loadQuestions sends from the json file 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DisplayQuestion : MonoBehaviour
{

    //Defines the GameObjects being used
    public GameObject screenQuestion;
    public GameObject answerA;
    public GameObject answerB;
    public GameObject answerC;
    public GameObject answerD;
    public GameObject answerE;
    public GameObject answerG;

    // Static variables that other classes play with to display questions
    public static string newQuestion;
    public static string newA;
    public static string newB;
    public static string newC;
    public static string newD;
    public static string newE;
    public static string newG;

    // Set to false by default so it doesn't try to display a question before loadQuestions initializes them
    public static bool updater = false;

    // Checks to see if a question needs to be displayed (updater = true)
    // Runs on a coroutine for consistency reasons 
    void Update()
    {
        if (updater == false)
        {
            updater = true;
            StartCoroutine(Display());
        }
    }

    IEnumerator Display()
    {
        // The delay for the coroutine 
        yield return new WaitForSeconds(.1f);

        // Changes the text on the static fields defined earlier 
        screenQuestion.GetComponent<TextMeshProUGUI>().text = newQuestion;
        answerA.GetComponent<TextMeshProUGUI>().text = newA;
        answerB.GetComponent<TextMeshProUGUI>().text = newB;
        answerC.GetComponent<TextMeshProUGUI>().text = newC;
        answerD.GetComponent<TextMeshProUGUI>().text = newD;
        answerE.GetComponent<TextMeshProUGUI>().text = newE;
        answerG.GetComponent<TextMeshProUGUI>().text = newG;
    }

}

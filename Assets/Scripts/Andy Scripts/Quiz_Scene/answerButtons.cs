using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Script to handle the click events of the buttons, also helps with controlling program flow 
public class answerButtons : MonoBehaviour
{
    public GameObject answerAred;
    public GameObject answerAblue;
    public GameObject answerAgreen;
    public GameObject answerBred;
    public GameObject answerBblue;
    public GameObject answerBgreen;
    public GameObject answerCred;
    public GameObject answerCblue;
    public GameObject answerCgreen;
    public GameObject answerDred;
    public GameObject answerDblue;
    public GameObject answerDgreen;
    public GameObject answerA;
    public GameObject answerB;
    public GameObject answerC;
    public GameObject answerD;
    public GameObject answerE;
    public GameObject answerEred;
    public GameObject answerEblue;
    public GameObject answerEgreen;
    public GameObject answerG;
    public GameObject answerGred;
    public GameObject answerGblue;
    public GameObject answerGgreen;
    
    bool isCorrect = false;
    public static bool activeButtons = true;

    // These are all functionally identical 
    // If the answer selected is correct, the corresponding button turns green. Else, red
    // Then, locks out the buttons, marks if the answer was correct, and starts the coroutine to trigger the next question 
    public void AnswerA()
    {
        if (loadQuestions.correctAnswer == 1)
        {
            answerAgreen.SetActive(true);
            answerAblue.SetActive(false);
            isCorrect = true;
        }
        else
        {
            answerAred.SetActive(true);
            answerAblue.SetActive(false);
        }

        activeButtons = false;
        StartCoroutine(NextQuestion());
    }

    public void AnswerB()
    {
        if (loadQuestions.correctAnswer == 2)
        {
            answerBgreen.SetActive(true);
            answerBblue.SetActive(false);
            isCorrect = true;
        }
        else
        {
            answerBred.SetActive(true);
            answerBblue.SetActive(false);
        }

        activeButtons = false;
        StartCoroutine(NextQuestion());
    }
    public void AnswerC()
    {
        if (loadQuestions.correctAnswer == 3)
        {
            answerCgreen.SetActive(true);
            answerCblue.SetActive(false);
            isCorrect = true;
        }
        else
        {
            answerCred.SetActive(true);
            answerCblue.SetActive(false);
        }

        activeButtons = false;
        StartCoroutine(NextQuestion());
    }
    public void AnswerD()
    {
        if (loadQuestions.correctAnswer == 4)
        {
            answerDgreen.SetActive(true);
            answerDblue.SetActive(false);
            isCorrect = true;
        }
        else
        {
            answerDred.SetActive(true);
            answerDblue.SetActive(false);
        }

        activeButtons = false;
        StartCoroutine(NextQuestion());
    }

    public void AnswerE()
    {
        if (loadQuestions.correctAnswer == 5)
        {
            answerEgreen.SetActive(true);
            answerEblue.SetActive(false);
            isCorrect = true;
        }
        else
        {
            answerEred.SetActive(true);
            answerEblue.SetActive(false);
        }

        activeButtons = false;
        StartCoroutine(NextQuestion());
    }

    public void AnswerG()
    {
        if (loadQuestions.correctAnswer == 1)
        {
            answerGgreen.SetActive(true);
            answerGblue.SetActive(false);
            isCorrect = true;
        }
        else
        {
            answerGred.SetActive(true);
            answerGblue.SetActive(false);
        }

        activeButtons = false;
        StartCoroutine(NextQuestion());
    }

    IEnumerator NextQuestion()
    {
        // Runs on a delay, so people have time to recognize if they got the question correct or incorrect
        yield return new WaitForSeconds(1f);
        
        // Defines a string that represents a question index 
        string answerString = ((loadQuestions.questionIndex).ToString());

        // Then sets a PlayerPref with it. Assigns 1 for correct answers, 0 for wrong
        if (isCorrect == true)
        {
            PlayerPrefs.SetInt(answerString, 1);
        }
        else 
        {
            PlayerPrefs.SetInt(answerString, 0);
        }

        // Resets this bool trigger 
        isCorrect = false;

        // Disables the red and green button panels, reactivates the base panels 
        answerAgreen.SetActive(false);
        answerBgreen.SetActive(false);
        answerCgreen.SetActive(false);
        answerDgreen.SetActive(false);
        answerEgreen.SetActive(false);
        answerGgreen.SetActive(false); 

        answerAred.SetActive(false);
        answerBred.SetActive(false);
        answerCred.SetActive(false);
        answerDred.SetActive(false);
        answerEred.SetActive(false);
        answerGred.SetActive(false);

        answerAblue.SetActive(true);
        answerBblue.SetActive(true);
        answerCblue.SetActive(true);
        answerDblue.SetActive(true);
        answerEblue.SetActive(true);
        answerGblue.SetActive(true);

        // Unlocks the buttons, then unmarks the displayingQuestion trigger so a new question can load
        activeButtons = true;
        loadQuestions.displayingQuestion = false;
    }

    void Update()
    {
        // Locks or unlocks the buttons, based on the activeButtons bool
        if (activeButtons == false)
        {
            answerA.GetComponent<Button>().enabled = false;
            answerB.GetComponent<Button>().enabled = false;
            answerC.GetComponent<Button>().enabled = false;
            answerD.GetComponent<Button>().enabled = false;
            answerE.GetComponent<Button>().enabled = false;
            answerG.GetComponent<Button>().enabled = false;
        }
        else 
        {
            answerA.GetComponent<Button>().enabled = true;
            answerB.GetComponent<Button>().enabled = true;
            answerC.GetComponent<Button>().enabled = true;
            answerD.GetComponent<Button>().enabled = true;
            answerE.GetComponent<Button>().enabled = true;
            answerG.GetComponent<Button>().enabled = true;
        }
    }
}

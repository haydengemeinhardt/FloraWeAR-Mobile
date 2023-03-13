using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class loadQuestions : MonoBehaviour
{
    // The JSON file being used 
    public TextAsset textJson;

    //Indexes the correct answer 
    public static int correctAnswer;

    // Checks to see if a question is being displayed, set to false to display a question when the program starts
    public static bool displayingQuestion = false;

    // Tracks the number of the question being currently used 
    public static int questionNumber = 0;

    // Hardcoded into the JSON file to keep track of individual questions. Useful for checking which questions have been gotten right and wrong
    // Also allows for randomization, as this doesn't have to match questionNumber 
    public static int questionIndex;

    // Allows me to clean up question transitions by knowing if I need to animate buttons out of the scene
    public bool was_multiple = false;
    public bool was_five_last = false;

    //Creates a question class that our questions will be read into 
    [System.Serializable]
    public class Questions
    {
        public string question;
        public string[] answers;
        public int cor;
        public int qi; 
    }

    //Is a basic array class to store a group of questions
    [System.Serializable]
    public class QuestionList
    {
        public Questions[] quizlist;
    }

    // Creates an array of the quiz class 
    public QuestionList quiz = new QuestionList();

    void Start()
    {
        // Initializes the array of questions 
        quiz = JsonUtility.FromJson<QuestionList>(textJson.text);
        

        // I could not tell you why this is necessary but the program doesn't work as intended without it
        // If the first question has 5 or 6 answers, the correct animation is hardcoded to play on start 
        if (quiz.quizlist[questionNumber].answers.Length > 4 && quiz.quizlist[questionNumber].answers.Length < 6)
        {
            button5controller.five_anim = true;
            was_five_last = true;
        }
        else if (quiz.quizlist[questionNumber].answers.Length > 5)
        {
            button6controller.playanim = true;
            was_multiple = true; 
        }
    }

    void Update()
    {
        // If there is not currently a question being displayed 
        if (!displayingQuestion)
        {
            // functionally just says "while there are still questions in the array"
            if (quiz.quizlist.Length > questionNumber)
            {
                // Assigns everything to be displayed by the DisplayQuestions file by pulling from the array of questions 
                DisplayQuestion.newQuestion = quiz.quizlist[questionNumber].question;
                DisplayQuestion.newA = quiz.quizlist[questionNumber].answers[0];
                DisplayQuestion.newB = quiz.quizlist[questionNumber].answers[1];
                DisplayQuestion.newC = quiz.quizlist[questionNumber].answers[2];
                DisplayQuestion.newD = quiz.quizlist[questionNumber].answers[3];

                //If there are exactly 5 answers
                if (quiz.quizlist[questionNumber].answers.Length == 5)
                {
                    DisplayQuestion.newE = quiz.quizlist[questionNumber].answers[4];
                    button5controller.five_anim = true;
                    button6controller.undoanim = true;
                    was_five_last = true;
                }
                //if there are exactly 6 answers 
                else if (quiz.quizlist[questionNumber].answers.Length == 6)
                {
                    was_five_last = false;
                    was_multiple = true;
                    DisplayQuestion.newE = quiz.quizlist[questionNumber].answers[4];
                    DisplayQuestion.newG = quiz.quizlist[questionNumber].answers[5];
                    button5controller.playanim = true;
                    button6controller.playanim = true;

                }
                // If there are 4 or less answers 
                else
                {
                    // If the last question had five answers, animates the 5th answer button out
                    if (was_five_last)
                    {
                        button5controller.undo_five = true;
                    }
                    // If the last question had six answers, animated the 6th answer button out 
                    else if (was_multiple)
                    {
                        button5controller.undoanim = true;
                        button6controller.undoanim = true;
                    }
                    // Sets these to false, as this question only has four answers
                    was_five_last = false;
                    was_multiple = false;
                }
                correctAnswer = quiz.quizlist[questionNumber].cor;
                questionIndex = quiz.quizlist[questionNumber].qi;

                // Marks that a question is being displayed and increments the question number for later 
                displayingQuestion = true;
                questionNumber++;
            }
            else // If there are no questions left to display 
            {
                // Disables the answer buttons to avoid shenanigans 
                answerButtons.activeButtons = false;

                // Triggers the animation for the scene transition button 
                // Doing it this way felt less clunky than having it automatically transition 
                quizTransition.playanim = true;
                DisplayQuestion.newQuestion = "Congrats! You've completed the quiz. Click the button to continue!";
                DisplayQuestion.newA = "";
                DisplayQuestion.newB = "";
                DisplayQuestion.newC = "";
                DisplayQuestion.newD = "";
                DisplayQuestion.newE = "";
                DisplayQuestion.newG = "";

                // Marks a question as being displayed, so this else statement doesn't run over itself a bunch
                displayingQuestion = true;

                // Also resets the question number 
                questionNumber = 0;

                // And sets these to false
                was_five_last = false;
                was_multiple = false;
            }
            // Marks the trigger to display the question with the DisplayQuestions class 
            DisplayQuestion.updater = false;
        }
    }
}

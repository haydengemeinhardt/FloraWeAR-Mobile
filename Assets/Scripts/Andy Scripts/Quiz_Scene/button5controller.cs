using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script to account for questions with variable numbers of answers
public class button5controller : MonoBehaviour
{
    private Animator anim5;
    public static bool playanim = false;
    public static bool undoanim = false;
    public static bool five_anim = false;
    public static bool undo_five = false;
    // Readies the animator on startup
    void Start()
    {
        anim5 = GetComponent<Animator>();
    }

    
    void Update()
    {
        // When the trigger is hit, plays the animation, then unhits the trigger so it doesn't repeat
        if (playanim == true)
        {
            anim5.Play("Base Layer.button_5_anim");
            playanim = false;
        }  
        // Undoes the animation 
        if (undoanim == true)
        {
            anim5.Play("Base Layer.button_5_deanim");
            undoanim = false;
        }
        // This one plays if the question has exactly five answers 
        if (five_anim == true)
        {
            anim5.Play("Base Layer.5_answer_choices");
            five_anim = false;
        }  
        // Undoes the above 
        if (undo_five == true)
        {
            anim5.Play("Base Layer.5_answer_choices_deanim");
            undo_five = false;
        }
    }
}

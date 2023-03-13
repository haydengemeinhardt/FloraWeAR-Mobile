using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quizTransition : MonoBehaviour
{
    private Animator anim;
    public static bool playanim = false;

    // Readies the animator on startup
    void Start()
    {
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        // When the trigger is hit, plays the animation, then unhits the trigger so it doesn't repeat
        if (playanim == true)
        {
            anim.Play("Base Layer.quiz_scene_button");
            playanim = false;
        }
    }
}

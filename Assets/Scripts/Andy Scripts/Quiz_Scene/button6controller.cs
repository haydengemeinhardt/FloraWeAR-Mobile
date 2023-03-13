using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class button6controller : MonoBehaviour
{
    private Animator anim6;
    public static bool playanim = false;
    public static bool undoanim = false;

    // Readies the animator on startup
    void Start()
    {
        anim6 = GetComponent<Animator>();
    }


    // Animated the 6th answer button, for when it's necessary
    void Update()
    {
        if (playanim == true)
        {
            anim6.Play("Base Layer.button_6_anim");
            playanim = false;
        }  

        if (undoanim == true)
        {
            anim6.Play("Base Layer.button_6_deanim");
            undoanim = false;
        }
    }
}

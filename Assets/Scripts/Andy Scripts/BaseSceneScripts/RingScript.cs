using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class RingScript : MonoBehaviour
{
    TextMeshProUGUI text_catcher;
    // Start is called before the first frame update
    void Start()
    {
        text_catcher = GameObject.Find("PlantWear_type_text").GetComponent<TextMeshProUGUI>();
    }

    public void changeTextRing()
    {
        text_catcher.text = "Ring";
         master_control.farmwear_used = "Ring";
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class RadishScript : MonoBehaviour
{
    TextMeshProUGUI text_catcher;
    // Start is called before the first frame update
    void Start()
    {
        text_catcher = GameObject.Find("Plant_type_text").GetComponent<TextMeshProUGUI>();
    }

    public void changeTextRadish()
    {
        text_catcher.text = "Radish";
        master_control.plant_used = "radish";
    }
}


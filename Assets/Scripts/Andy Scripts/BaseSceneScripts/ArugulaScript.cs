using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ArugulaScript : MonoBehaviour
{
    TextMeshProUGUI text_catcher;
    // Start is called before the first frame update
    void Start()
    {
        text_catcher = GameObject.Find("Plant_type_text").GetComponent<TextMeshProUGUI>();
    }

    public void changeTextArugula()
    {
        text_catcher.text = "Arugula";
        master_control.plant_used = "arugula";
    }
}

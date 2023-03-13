using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BraceletScript : MonoBehaviour
{
    TextMeshProUGUI text_catcher;
    // Start is called before the first frame update
    void Start()
    {
        text_catcher = GameObject.Find("PlantWear_type_text").GetComponent<TextMeshProUGUI>();
    }

    public void changeTextBracelet()
    {
        text_catcher.text = "Bracelet";
        master_control.farmwear_used = "bracelet";
    }

}

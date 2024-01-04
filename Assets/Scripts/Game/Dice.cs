using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Dice : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Button button;
    public void Awake() 
    {    
        text.text = "";
        button.interactable = true;
    }

    public void Roll()
    {
        int num = Random.Range(1, 7);
        text.text = num.ToString();
        button.interactable = false;
    }
}

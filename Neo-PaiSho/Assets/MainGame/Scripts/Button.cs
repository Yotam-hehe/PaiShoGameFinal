using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Button : MonoBehaviour
{
    public UnityEvent buttonClick;
    public Text buttonText;
    // Start is called before the first frame update
    void Awake()
    {
        if (buttonClick == null) { buttonClick = new UnityEvent(); }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void NewText()
    {
        if (buttonText.text != "Stop")
        {
            buttonText.text = "Stop";
        }
        else
        {
            buttonText.text = "Rotate";
        }
    }

}

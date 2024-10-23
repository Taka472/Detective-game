using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlMenuPair : MonoBehaviour
{
    [SerializeField] private Text text;

    public void ChangeTextColor(Color color)
    {
        text.color = color;
    }

    public void SetText(string Text)
    {
        text.text = Text;
    }
}

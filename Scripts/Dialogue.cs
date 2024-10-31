using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue
{
    public Color color;
    public string speaker;
    public string storyLine;

    public Dialogue(Color color, string speaker, string storyLine)
    {
        this.color = color;
        this.speaker = speaker;
        this.storyLine = storyLine;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonSenseScript : MonoBehaviour
{
    [Header("Ink dialogue")]
    [SerializeField] private TextAsset[] dialogues;

    public TextAsset[] getDialogues()
    {
        return dialogues;
    }
}

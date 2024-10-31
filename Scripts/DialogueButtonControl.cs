using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueButtonControl : MonoBehaviour
{
    [SerializeField] private Text logButtonText;
    [SerializeField] private Text inventoryButtonText;

    [SerializeField] private KeyBindDictionary keys;

    private void Update()
    {
        logButtonText.text = keys.GetDictionary()["DialogueLog"].ToString() + ": Dialogue log";
        inventoryButtonText.text = keys.GetDictionary()["OpenInventory"].ToString() + ": Inventory";
    }
}

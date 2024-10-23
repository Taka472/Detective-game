using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBindDictionary : MonoBehaviour
{
    private Dictionary<string, KeyCode> keys = new();

    [SerializeField] private DataManager dataManager;

    private void Awake()
    {
        Initial();
        LoadData();
    }

    public void Initial()
    {
        keys.Add("WalkLeft", KeyCode.A);
        keys.Add("WalkRight", KeyCode.D);
        keys.Add("Interact", KeyCode.X);
        keys.Add("OpenInventory", KeyCode.Z);
        keys.Add("OpenSuspects", KeyCode.C);
        keys.Add("DialogueLog", KeyCode.V);
    }

    public void LoadData()
    {
        keys = dataManager.LoadKeyBindDictionary();
    }

    public Dictionary<string, KeyCode> GetDictionary()
    {
        return keys;
    }
}

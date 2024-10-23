using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using AssemblyCSharp;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    private string path;
    private string keyPath;

    public PlayerModel playerModel = new();
    public KeyBindModel keyBindModel = new();

    [SerializeField] private InventoryControl inventoryControl;
    [SerializeField] private LocationControl locationControl;
    [SerializeField] private NPCInteractionControl npcInteractionControl;
    [SerializeField] private EvidenceHolders evidenceHolders;

    private void Awake()
    {
        instance = this;
        path = Path.Combine(Application.streamingAssetsPath, "playerData.json");
        keyPath = Path.Combine(Application.streamingAssetsPath, "keyBindData.json");
        
        //Check if both save file exist 
        if (File.Exists(path))
        {
            Debug.Log("Found player saved file");
        }
        else Debug.LogError("Cannot find saved file");

        if (File.Exists(keyPath))
        {
            Debug.Log("Found key bind saved file");
        }
        else Debug.LogError("Cannot find key bind saved file");

        // InitialData();
        LoadData();
    }

    public void SetPlayerModel(PlayerModel model)
    {
        this.playerModel = model;
    }

    public void InitialData()
    {
        keyBindModel = new();

        keyBindModel.keyDictionary.Add(new KeyBindModel.KeyValue("WalkLeft", "A"));
        keyBindModel.keyDictionary.Add(new KeyBindModel.KeyValue("WalkRight", "D"));
        keyBindModel.keyDictionary.Add(new KeyBindModel.KeyValue("Interact", "X"));
        keyBindModel.keyDictionary.Add(new KeyBindModel.KeyValue("OpenInventory", "Z"));
        keyBindModel.keyDictionary.Add(new KeyBindModel.KeyValue("OpenSuspects", "C"));
        keyBindModel.keyDictionary.Add(new KeyBindModel.KeyValue("DialogueLog", "V"));

        string jsonData = KeyBindModel.GetJsonFromModel(keyBindModel, true);
        File.WriteAllText(path, jsonData);
        Debug.Log(KeyBindModel.GetJsonFromModel(keyBindModel, true));   
    }

    public void SaveData(PlayerModel playerModel)
    {
        string jsonData = PlayerModel.GetJsonFromModel(playerModel, true);
        File.WriteAllText(path, jsonData);
    }

    public void LoadData()
    {
        if (!File.Exists(path)) return;

        string jsonData = File.ReadAllText(path);
        playerModel = PlayerModel.GetModelFromJson(jsonData);

        List<Evidence> list = new();
        foreach(int i in playerModel.evidenceIDList)
        {
            list.Add(evidenceHolders.EvidenceList()[i]);
        }

        inventoryControl.evidencesID = list;
        locationControl.locationNames = playerModel.locationList;
        npcInteractionControl.firstTime = playerModel.firstTimeInteractionList;
    }

    public void SaveKeyBind(KeyBindModel keyBindModel)
    {
        string jsonData = KeyBindModel.GetJsonFromModel(keyBindModel, true);
        File.WriteAllText(keyPath, jsonData);
    }

    public Dictionary<string, KeyCode> LoadKeyBindDictionary()
    {
        if (!File.Exists(keyPath)) return null;

        string jsonData = File.ReadAllText(keyPath);

        keyBindModel = KeyBindModel.GetModelFromJson(jsonData);

        Dictionary<string, KeyCode> dictionary = new();

        foreach (KeyBindModel.KeyValue key in keyBindModel.keyDictionary)
        {
            dictionary.Add(key.key, StringToKeyCode(key.value));
        }

        return dictionary;
    }

    private KeyCode StringToKeyCode(string key)
    {
        if (System.Enum.TryParse(key, true, out KeyCode code))
        {
            return code;
        }
        return KeyCode.None;
    }
}

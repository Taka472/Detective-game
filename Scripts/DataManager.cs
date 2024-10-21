using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using AssemblyCSharp;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    private string path;

    public PlayerModel playerModel = new();

    [SerializeField] private InventoryControl inventoryControl;
    [SerializeField] private LocationControl locationControl;
    [SerializeField] private NPCInteractionControl npcInteractionControl;
    [SerializeField] private EvidenceHolders evidenceHolders;

    private void Awake()
    {
        instance = this;
        path = Path.Combine(Application.streamingAssetsPath, "playerData.json");
        if (File.Exists(path))
        {
            Debug.Log("Found saved file");
        }
        else Debug.LogError("Cannot find saved file");
        LoadData();
    }

    public void SetPlayerModel(PlayerModel model)
    {
        this.playerModel = model;
    }

    public void InitialData()
    {
        playerModel = new();
        Debug.Log(PlayerModel.GetJsonFromModel(playerModel, true));   
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
}

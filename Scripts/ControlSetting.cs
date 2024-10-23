using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using AssemblyCSharp;

public class ControlSetting : MonoBehaviour
{
    [Header("Menu buttons")]
    [SerializeField] private Button[] buttons;

    [Header("Default setting page")]
    [SerializeField] private GameObject defaultSetting;

    [Header("Control setting pages")]
    [SerializeField] private GameObject controlSetting;

    [SerializeField] private KeyBindDictionary keys;
    [SerializeField] private DataManager dataManager;

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(buttons[0].gameObject);
    }

    public void RestoreDefault()
    {
        //Reset key binding
        keys.GetDictionary()["WalkLeft"] = KeyCode.A;
        keys.GetDictionary()["WalkRight"] = KeyCode.D;
        keys.GetDictionary()["Interact"] = KeyCode.X;
        keys.GetDictionary()["OpenInventory"] = KeyCode.Z;
        keys.GetDictionary()["OpenSuspects"] = KeyCode.C;
        keys.GetDictionary()["DialogueLog"] = KeyCode.V;


        //Reset text display
        buttons[0].GetComponent<ControlMenuPair>().SetText(keys.GetDictionary()["WalkLeft"].ToString());
        buttons[1].GetComponent<ControlMenuPair>().SetText(keys.GetDictionary()["WalkRight"].ToString());
        buttons[2].GetComponent<ControlMenuPair>().SetText(keys.GetDictionary()["Interact"].ToString());
        buttons[3].GetComponent<ControlMenuPair>().SetText(keys.GetDictionary()["OpenInventory"].ToString());
        buttons[4].GetComponent<ControlMenuPair>().SetText(keys.GetDictionary()["OpenSuspects"].ToString());
        buttons[5].GetComponent<ControlMenuPair>().SetText(keys.GetDictionary()["DialogueLog"].ToString());
    }

    private void OnDisable()
    {
        Apply();
    }

    public void Apply()
    {
        KeyBindModel model = new();
        
        foreach(KeyValuePair<string, KeyCode> key in keys.GetDictionary())
        {
            model.keyDictionary.Add(new KeyBindModel.KeyValue(key.Key, key.Value.ToString()));
        }

        dataManager.SaveKeyBind(model);

        controlSetting.SetActive(false);
        defaultSetting.transform.GetComponentInParent<PauseMenuControl>().SetSelectedButton();
        defaultSetting.SetActive(true);
    }
}

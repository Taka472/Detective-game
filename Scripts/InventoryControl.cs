using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryControl : MonoBehaviour
{
    public static InventoryControl instance;
    public List<Evidence> evidencesID;
    public Button[] buttons;
    public Text descriptionText;
    private int index = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Movement.instance.openInventory)
            HandleInventory();
    }

    public void LoadEvidence()
    {
        index = 0;
        SpriteState sc = new();
        if (evidencesID.Count == 0) return;
        for (int i = 0; i < evidencesID.Count; i++)
        {
            buttons[i].gameObject.SetActive(true);
            sc.selectedSprite = evidencesID[i].spriteSelected;
            buttons[i].transform.GetChild(0).GetComponent<Image>().sprite = evidencesID[i].sprite;
            buttons[i].GetComponent<Button>().spriteState = sc;
        }
        EventSystem.current.SetSelectedGameObject(buttons[index].gameObject);
    }

    public void Close()
    {
        for (int i = 0; i < evidencesID.Count; i++)
        {
            buttons[i].gameObject.SetActive(false);
        }
    }

    public void ExecuteButton(int index)
    {
        if (!DialogueControl.Instance.isPlaying) return;
        DialogueControl.Instance.ShowEvidence(Movement.instance.interacting.GetComponent<NPCControl>().evidenceResponse[index]);
        GetComponent<InventoryControl>().enabled = false;
        Movement.instance.openInventory = false;
        transform.GetChild(0).gameObject.SetActive(false);
        GetComponent<Image>().enabled = false;
        DialogueControl.Instance.dialogueText.gameObject.SetActive(true);
        Close();
    }

    public void HandleInventory()
    {
        if (evidencesID.Count == 0) return;
        int selected = EventSystem.current.currentSelectedGameObject.GetComponent<InventoryButtonIndex>().index;
        descriptionText.text = evidencesID[selected].name + "\n" + evidencesID[selected].description;
    }
}

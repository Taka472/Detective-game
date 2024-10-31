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
    public Image[] buttonsImage;
    public Text descriptionText;
    private int index = 0;
    public GameObject description;
    public Image image;

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
            buttonsImage[i].sprite = evidencesID[i].sprite;
            buttons[i].spriteState = sc;
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
        if (!DialogueControl.instance.isPlaying) return;

        if (!Movement.instance.npcControl)
        {
            DialogueControl.instance.NonSense();
        }
        else DialogueControl.instance.ShowEvidence(Movement.instance.npcControl.evidenceResponse[index]);
        enabled = false;
        Movement.instance.openInventory = false;
        transform.GetChild(0).gameObject.SetActive(false);
        GetComponent<Image>().enabled = false;
        DialogueControl.instance.ToggleText(true);
        Close();
    }

    public void HandleInventory()
    {
        if (evidencesID.Count == 0) return;
        int selected = EventSystem.current.currentSelectedGameObject.GetComponent<InventoryButtonIndex>().index;
        descriptionText.text = evidencesID[selected].name + "\n" + evidencesID[selected].description;
    }

    public void ToggleInvetory(bool boolean)
    {
        enabled = boolean;
        image.enabled = boolean;
        description.SetActive(boolean);
    }
}

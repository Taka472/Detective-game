using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DialogueLogControl : MonoBehaviour
{
    private readonly List<Dialogue> logs = new();

    [SerializeField] private GameObject scrollPane;
    [SerializeField] private GameObject scrollItem;
    [SerializeField] private GameObject mainDialogue;
    [SerializeField] private GameObject mainSpeakerDialogue;
    [SerializeField] private GameObject choicePanel;

    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private KeyBindDictionary keys;

    [SerializeField] private float scrollSpeed = 0.01f;

    [SerializeField] private Text scrollUpText;
    [SerializeField] private Text scrollDownText;

    [SerializeField] private GameObject logButton;
    [SerializeField] private GameObject inventoryButton;

    public void AddLog(Dialogue log)
    {
        logs.Add(log);
    }

    public void ClearLog()
    {
        logs.Clear();
    }

    private void Update()
    {
        if (scrollRect == null) return;

        if (Input.GetKey(keys.GetDictionary()["ScrollUp"]))
        {
            if (scrollRect.verticalNormalizedPosition <= 1)
                scrollRect.verticalNormalizedPosition += scrollSpeed;
        }

        if (Input.GetKey(keys.GetDictionary()["ScrollDown"]))
        {
            if (scrollRect.verticalNormalizedPosition >= 0)
                scrollRect.verticalNormalizedPosition -= scrollSpeed;
        }

        scrollUpText.text = keys.GetDictionary()["ScrollUp"].ToString();
        scrollDownText.text = keys.GetDictionary()["ScrollDown"].ToString();
    }

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(scrollPane);
        mainDialogue.SetActive(false);
        mainSpeakerDialogue.SetActive(false);
        choicePanel.SetActive(false);
        logButton.SetActive(false);
        inventoryButton.SetActive(false);
        if (scrollPane.transform.childCount == 0)
        {
            foreach (Dialogue log in logs)
            {
                GameObject logTemp = Instantiate(scrollItem, scrollPane.transform);
                Text logText = logTemp.GetComponent<Text>();
                logText.color = log.color;
                if (log.color == Color.yellow)
                {
                    logText.alignment = TextAnchor.UpperRight;
                }
                else
                {
                    logText.alignment = TextAnchor.UpperLeft;
                }
                logText.text = log.storyLine;
            }
        }
        else
        {
            for (int i = 0; i < logs.Count; i++)
            {
                if (scrollPane.transform.GetChild(i) != null)
                {
                    GameObject temp = scrollPane.transform.GetChild(i).gameObject;
                    temp.SetActive(true);
                    Text logText = temp.GetComponent<Text>();
                    if (logs[i].color == Color.yellow)
                    {
                        logText.alignment = TextAnchor.UpperRight;
                    }
                    else
                    {
                        logText.alignment = TextAnchor.UpperLeft;
                    }
                    logText.text = logs[i].storyLine;
                } 
                else
                {
                    GameObject logTemp = Instantiate(scrollItem, scrollPane.transform);
                    Text logText = logTemp.GetComponent<Text>();
                    logText.color = logs[i].color;
                    if (logs[i].color == Color.yellow)
                    {
                        logText.alignment = TextAnchor.UpperRight;
                    }
                    else
                    {
                        logText.alignment = TextAnchor.UpperLeft;
                    }
                    logText.text = logs[i].storyLine;
                }
            }

        }
    }

    private void OnDisable()
    {
        mainDialogue.SetActive(true);
        mainSpeakerDialogue.SetActive(true);
        choicePanel.SetActive(true);
        logButton.SetActive(true);
        inventoryButton.SetActive(true);

        for (int i = 0; i < scrollPane.transform.childCount; i++)
        {
            scrollPane.transform.GetChild(i).gameObject.SetActive(false);
        }

        //Reselect the first choice
        EventSystem.current.SetSelectedGameObject(choicePanel.transform.GetChild(0).gameObject);
    }
}

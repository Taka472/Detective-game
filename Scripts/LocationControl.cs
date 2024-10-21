using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class LocationControl : MonoBehaviour
{
    public static LocationControl instance;
    public GameObject locationTitle;
    public List<string> locationNames;
    public Button[] buttons;

    private void Awake()
    {
        instance = this;
        if (!locationNames.Contains("Police station"))
            locationNames.Insert(0, "Police station");
    }

    public void LoadLocation()
    {
        for (int i = 0; i < locationNames.Count; i++)
        {
            buttons[i].gameObject.SetActive(true);
            buttons[i].transform.GetChild(0).GetComponent<Text>().text = locationNames[i];
            buttons[i].transform.GetChild(0).GetComponent<Text>().color = Color.white;
        }
        for (int i = locationNames.Count; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(true);
            buttons[i].transform.GetChild(0).GetComponent<Text>().text = "?";
        }
        buttons[SceneManager.GetActiveScene().buildIndex - 1].transform.GetChild(0).GetComponent<Text>().color = Color.yellow;
        EventSystem.current.SetSelectedGameObject(buttons[0].gameObject);
    }

    public void ExecuteButton(int index)
    {
        if (index + 1 > locationNames.Count) return;
        Cutscene1.instance.DummyTransition(index + 1);
        gameObject.SetActive(false);
        locationTitle.SetActive(false);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}

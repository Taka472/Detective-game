using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class LocationControl : MonoBehaviour
{
    public static LocationControl instance;
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
        }
        buttons[SceneManager.GetActiveScene().buildIndex - 1].transform.GetChild(0).GetComponent<Text>().color = Color.yellow;
        EventSystem.current.SetSelectedGameObject(buttons[0].gameObject);
    }

    public void ExecuteButton(int index)
    {
        if (SceneManager.GetActiveScene().buildIndex == index + 1) return;
        else Cutscene1.instance.DummyTransition(index + 1);
        gameObject.SetActive(false);
    }
}

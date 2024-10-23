using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenuControl : MonoBehaviour
{
    [Header("Menu buttons")]
    [SerializeField] private Button[] buttons;

    [Header("Default setting page")]
    [SerializeField] private GameObject defaultSetting;

    [Header("Control setting pages")]
    [SerializeField] private GameObject controlSetting;

    public void SetSelectedButton()
    {
        EventSystem.current.SetSelectedGameObject(buttons[0].gameObject);
    }

    private void OnEnable()
    {
        defaultSetting.SetActive(true);
        controlSetting.SetActive(false);
        SetSelectedButton();
    }

    public void SoundButton(int index)
    {

    }

    public void ControlSettingButton()
    {
        defaultSetting.SetActive(false);
        controlSetting.SetActive(true);
    }

    public void ClosePauseMenu()
    {
        gameObject.SetActive(false);
    }

    public void BackToMenu()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyBinding : MonoBehaviour
{
    [SerializeField] private KeyBindDictionary keys;

    public Text left, right, interact, openInventory, openSuspects, dialogueLog, scrollUp, scrollDown;

    public ControlMenuPair currentKey;

    private void Start()
    {
        left.text = keys.GetDictionary()["WalkLeft"].ToString();
        right.text = keys.GetDictionary()["WalkRight"].ToString();
        interact.text = keys.GetDictionary()["Interact"].ToString();
        openInventory.text = keys.GetDictionary()["OpenInventory"].ToString();
        openSuspects.text = keys.GetDictionary()["OpenSuspects"].ToString();
        dialogueLog.text = keys.GetDictionary()["DialogueLog"].ToString();
        scrollUp.text = keys.GetDictionary()["ScrollUp"].ToString();
        scrollDown.text = keys.GetDictionary()["ScrollDown"].ToString();
    }

    public Dictionary<string, KeyCode> GetDictionary()
    {
        return keys.GetDictionary();
    }

    private void OnGUI()
    {
        if (currentKey == null) return;

        Event e = Event.current;
        if (e.isKey && e.keyCode.ToString() != "Return")
        {
            if (CheckDuplication(e.keyCode)) return;
            keys.GetDictionary()[currentKey.name] = e.keyCode;
            currentKey.SetText(e.keyCode.ToString());
            currentKey.ChangeTextColor(Color.white);
            currentKey = null;
        }
    }

    public void ChangeKey(ControlMenuPair keys)
    {
        currentKey = keys;
        keys.ChangeTextColor(Color.yellow);
    }

    public bool CheckDuplication(KeyCode key)
    {
        foreach(KeyValuePair<string, KeyCode> e in keys.GetDictionary())
        {
            if (e.Value == key)
            {
                KeyCode temp = keys.GetDictionary()[currentKey.name];

                keys.GetDictionary()[e.Key] = temp;
                keys.GetDictionary()[currentKey.name] = key;
                switch (e.Key)
                {
                    case "Left":
                        {
                            left.text = temp.ToString();
                            break;
                        }
                    case "Right":
                        {
                            right.text = temp.ToString();
                            break;
                        }
                    case "Interact":
                        {
                            interact.text = temp.ToString();
                            break;
                        }
                    case "OpenInventory":
                        {
                            openInventory.text = temp.ToString();
                            break;
                        }
                    case "OpenSuspect":
                        {
                            openSuspects.text = temp.ToString();
                            break;
                        }
                    case "DialogueLog":
                        {
                            dialogueLog.text = temp.ToString();
                            break;
                        }
                    case "ScrollUp":
                        {
                            scrollUp.text = temp.ToString();
                            break;
                        }
                    case "ScrollDown":
                        {
                            scrollDown.text = temp.ToString();
                            break;
                        }
                }

                currentKey.SetText(key.ToString());
                currentKey.ChangeTextColor(Color.white);
                currentKey = null;
                return true;
            }
        }
        return false;
    }
}

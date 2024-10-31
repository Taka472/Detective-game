using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    public static Movement instance;

    public AudioSource source;
    public Animator animator;
    public InventoryControl inventoryControl;
    public float speed = 2;
    public bool isInteracting = false;
    public bool openInventory = false;
    public bool isDialogueLog = false;
    public float minSpace;
    public float maxSpace;
    public Interaction interacting;
    public NPCControl npcControl;
    public GameObject interactKey;
    public Camera cam;
    public Interaction[] interactables;
    public GameObject DialogueLog;
    public GameObject DialogueHistory;
    public GameObject Inventory;

    [SerializeField] private GameObject logButton;
    [SerializeField] private GameObject inventoryButton;

    [SerializeField] private KeyBindDictionary keys;

    private void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.instance.GetActive()) return;
        if (!isInteracting && !openInventory)
        {
            Move();
            CheckInteract();
        }
        else if (interacting != null)
            Interact();
        else Idle();
        if (Input.GetKeyDown(keys.GetDictionary()["OpenInventory"]) && Cutscene1.instance.hasPlayed)
            OpenInventory();

        if (Input.GetKeyDown(keys.GetDictionary()["DialogueLog"]))
        {
            if (isInteracting)
            {
                ToggleLog();
            } else
            {
                DialogueHistory.SetActive(!DialogueHistory.activeSelf);
            }
        }
    }

    private void Move()
    {
        float x;

        if (Input.GetKey(keys.GetDictionary()["WalkLeft"])) x = -1;
        else if (Input.GetKey(keys.GetDictionary()["WalkRight"])) x = 1;
        else x = 0;

        if (x != 0 && transform.position.x <= maxSpace && transform.position.x >= minSpace)
        {
            animator.Play("Walking");
            transform.eulerAngles = new Vector3(0, x > 0 ? 0 : 180, 0);
        }
        else
        {
            animator.Play("Idle");
        }

        if (!((transform.position.x > maxSpace && x > 0) || (transform.position.x < minSpace && x < 0)))
        {
            transform.position += speed * Time.deltaTime * new Vector3(x, 0, 0);
        }
    }

    public void CheckInteract()
    {
        if (Cutscene1.instance.isTransition)
        {
            interactKey.SetActive(false);
            return;
        }

        for (int i = 0; i < interactables.Length; i++)
        {
            if (Vector2.Distance(interactables[i].transform.position, transform.position) <= interactables[i].radius)
            {
                interactKey.SetActive(true);
                return;
            }
        }
        interactKey.SetActive(false);
    }

    public void PlayAudio()
    {
        source.PlayOneShot(source.clip);
    }

    public void Idle()
    {
        animator.Play("Idle");
    }

    void Interact()
    {
        if (npcControl == null)
            animator.Play("LookAt");
    }

    public void OpenInventory()
    {
        if (!openInventory)
        {
            ToggleInventory(true);
            InventoryControl.instance.LoadEvidence();
        }
        else
        {
            ToggleInventory(false);
            InventoryControl.instance.Close();
        }
    }

    public void UpdateSpace(float maxSpace, float minSpace)
    {
        if (minSpace == 0 && maxSpace == 0) return;
        this.maxSpace = maxSpace;
        this.minSpace = minSpace;
    }

    public void ToggleInventory(bool boolean)
    {
        InventoryControl.instance.ToggleInvetory(boolean);
        DialogueControl.instance.ToggleText(!boolean);
        inventoryButton.SetActive(!boolean);
        logButton.SetActive(!boolean);
        openInventory = !openInventory;
    }

    public void ToggleLog()
    {
        logButton.SetActive(!logButton.activeSelf);
        inventoryButton.SetActive(!inventoryButton.activeSelf);
        DialogueLog.SetActive(!DialogueLog.activeSelf);
        isDialogueLog = !isDialogueLog;
    }
}

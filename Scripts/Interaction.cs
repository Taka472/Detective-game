using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public float radius = 2;
    public Transform player;
    public DialogueControl dialogueControl;
    public InventoryControl inventory;
    public NPCInteractionControl npcControl;
    public LocationControl locationControl;
    public bool sceneChange;
    public int playerRotation;

    [Header("Ink JSON")]
    public TextAsset inkJSON;

    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private KeyBindDictionary keys;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void Update()
    {
        if (pauseMenu.activeSelf) return;
        CheckInteract();
    }

    void CheckInteract()
    {
        if (Input.GetKeyDown(keys.GetDictionary()["Interact"]))
        {
            if (Cutscene1.instance.isTransition || Movement.instance.openInventory) return;
            else if (Vector2.Distance(transform.position, player.position) <= radius)
            {
                if (!dialogueControl.isPlaying)
                {
                    if (!dialogueControl.locationControl.gameObject.activeSelf)
                    {
                        if (GetComponent<NPCControl>() == null)
                        {
                            StartCoroutine(dialogueControl.EnterDialogueMode(inkJSON));
                            Movement.instance.interactKey.SetActive(false);
                            Movement.instance.interacting = gameObject;
                        }
                    }
                    if (GetComponent<PositionChange>() != null)
                    {
                        dialogueControl.characterPositionChange = GetComponent<PositionChange>().playerPosition;
                        dialogueControl.cameraPositionChange = GetComponent<PositionChange>().cameraPosition;
                        dialogueControl.minSpace = GetComponent<PositionChange>().minSpace;
                        dialogueControl.maxSpace = GetComponent<PositionChange>().maxSpace;
                    }
                    else if (GetComponent<NPCControl>() != null)
                    {
                        Movement.instance.interacting = gameObject;
                        dialogueControl.normalChoice = true;
                        GetComponent<NPCControl>().PlayerInteraction();
                        if (!npcControl.firstTime.Contains(GetComponent<NPCControl>().NPCID))
                        {
                            StartCoroutine(dialogueControl.EnterDialogueMode(inkJSON));
                        }
                        else StartCoroutine(dialogueControl.EnterDialogueMode(GetComponent<NPCControl>().randomResponse[Random.Range(0, GetComponent<NPCControl>().randomResponse.Length)]));
                        Movement.instance.interactKey.SetActive(false);
                        if (player.transform.rotation.y != playerRotation)
                        {
                            player.transform.eulerAngles = new Vector3(0, playerRotation);
                        }
                    }
                    else if (sceneChange)
                    {
                        dialogueControl.characterPositionChange = Vector3.zero;
                        dialogueControl.cameraPositionChange = Vector3.zero;
                    }
                }
                else if (dialogueControl.isTyping)
                {
                    dialogueControl.isTyping = false;
                    dialogueControl.StopAllCoroutines();
                    dialogueControl.SkipSenetence();
                }
                else
                {
                    dialogueControl.ContinueStory();
                }
            }
        }
    }
}

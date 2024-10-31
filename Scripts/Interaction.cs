using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public float radius = 2;
    public Transform player;
    private DialogueControl dialogueControl;
    public InventoryControl inventory;
    public NPCInteractionControl npcControl;
    public LocationControl locationControl;
    public bool sceneChange;
    public int playerRotation;

    public NPCControl NPC;
    public PositionChange positionChange;

    [Header("Ink JSON")]
    public TextAsset inkJSON;

    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private KeyBindDictionary keys;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void Start()
    {
        NPC = GetComponent<NPCControl>();
        positionChange = GetComponent<PositionChange>();
        dialogueControl = DialogueControl.instance;
    }

    private void Update()
    {
        if (pauseMenu.activeSelf || Movement.instance.isDialogueLog) return;
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
                        if (NPC == null)
                        {
                            StartCoroutine(dialogueControl.EnterDialogueMode(inkJSON));
                            Movement.instance.interacting = this;
                        }
                    }
                    if (positionChange != null)
                    {
                        dialogueControl.characterPositionChange = positionChange.playerPosition;
                        dialogueControl.cameraPositionChange = positionChange.cameraPosition;
                        dialogueControl.minSpace = positionChange.minSpace;
                        dialogueControl.maxSpace = positionChange.maxSpace;
                    }
                    else if (NPC != null)
                    {
                        Movement.instance.npcControl = NPC;
                        dialogueControl.normalChoice = true;
                        NPC.PlayerInteraction();
                        if (!npcControl.firstTime.Contains(NPC.NPCID))
                        {
                            StartCoroutine(dialogueControl.EnterDialogueMode(inkJSON));
                        }
                        else StartCoroutine(dialogueControl.EnterDialogueMode(NPC.randomResponse[Random.Range(0, NPC.randomResponse.Length)]));
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

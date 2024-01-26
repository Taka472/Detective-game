using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public float radius = 2;
    public Transform player;
    public DialogueControl control;
    public InventoryControl inventory;
    public NPCInteractionControl npcControl;
    public bool sceneChange;
    public int playerRotation;

    [Header("Ink JSON")]
    [SerializeField] public TextAsset inkJSON;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void Start()
    {
        control = FindObjectOfType<DialogueControl>();
        inventory = FindObjectOfType<InventoryControl>();
        npcControl = FindObjectOfType<NPCInteractionControl>();
    }

    private void Update()
    {
        CheckInteract();
    }

    void CheckInteract()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (Cutscene1.instance.isTransition || Movement.instance.openInventory) return;
            else if (Vector2.Distance(transform.position, player.position) <= radius)
            {
                if (!control.isPlaying)
                {
                    if (!control.locationControl.gameObject.activeSelf)
                    {
                        if (GetComponent<NPCControl>() == null)
                        {
                            StartCoroutine(control.EnterDialogueMode(inkJSON));
                            Movement.instance.interactKey.SetActive(false);
                            Movement.instance.interacting = gameObject;
                        }
                    }
                    if (GetComponent<PositionChange>() != null)
                    {
                        control.characterPositionChange = GetComponent<PositionChange>().playerPosition;
                        control.cameraPositionChange = GetComponent<PositionChange>().cameraPosition;
                    }
                    else if (GetComponent<NPCControl>() != null)
                    {
                        Movement.instance.interacting = gameObject;
                        control.normalChoice = true;
                        GetComponent<NPCControl>().PlayerInteraction();
                        if (!npcControl.firstTime.Contains(GetComponent<NPCControl>().NPCID))
                        {
                            StartCoroutine(control.EnterDialogueMode(inkJSON));
                        }
                        else StartCoroutine(control.EnterDialogueMode(GetComponent<NPCControl>().randomResponse[Random.Range(0, GetComponent<NPCControl>().randomResponse.Length)]));
                        Movement.instance.interactKey.SetActive(false);
                        if (player.transform.rotation.y != playerRotation)
                        {
                            player.transform.eulerAngles = new Vector3(0, playerRotation);
                        }
                    }
                    else if (sceneChange)
                    {
                        control.characterPositionChange = Vector3.zero;
                        control.cameraPositionChange = Vector3.zero;
                    }
                }
                else if (control.isTyping)
                {
                    control.isTyping = false;
                    control.StopAllCoroutines();
                    control.SkipSenetence();
                }
                else
                {
                    control.ContinueStory();
                }
            }
        }
    }
}

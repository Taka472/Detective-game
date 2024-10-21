using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    public static Movement instance;

    private AudioSource source;
    public Animator animator;
    public InventoryControl inventoryControl;
    public float speed = 2;
    public bool isInteracting = false;
    public bool openInventory = false;
    public float minSpace;
    public float maxSpace;
    public GameObject interacting;
    public Camera cam;
    public GameObject interactKey;
    public GameObject[] interactables;

    private void Start()
    {
        instance = this;
        source = GetComponent<AudioSource>();
        interactKey = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<PauseMenu>().GetActive()) return;
        if (!isInteracting && !openInventory)
        {
            Move();
            CheckInteract();
        }
        else if (interacting != null && interacting.GetComponent<NPCControl>() == null)
            Interact();
        else Idle();
        if (Input.GetKeyDown(KeyCode.Z) && Cutscene1.instance.hasPlayed)
            OpenInventory();
    }

    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");

        /*if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (x != 0 && transform.position.x <= 3 && transform.position.x >= -8.2f)
            {
                animator.Play("Walking");
                transform.eulerAngles = new Vector3(0, x > 0 ? 0 : 180, 0);
            }
            else
            {
                animator.Play("Idle");
            }

            if (!((transform.position.x > 3 && x > 0) || (transform.position.x < -8.2f && x < 0)))
            {
                transform.position += speed * Time.deltaTime * new Vector3(x, 0, 0);
            }
        }
        else
        {
            if (x != 0 && Mathf.Abs(transform.position.x) <= 8.3f)
            {
                animator.Play("Walking");
                transform.eulerAngles = new Vector3(0, x > 0 ? 0 : 180);
            }
            else
            {
                animator.Play("Idle");
            }

            if (!((transform.position.x > 8.3f && x > 0) || (transform.position.x < -8.3f && x < 0)))
            {
                transform.position += speed * Time.deltaTime * new Vector3(x, 0, 0);
            }
        }*/

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

    private void CheckInteract()
    {
        for (int i = 0; i < interactables.Length; i++)
        {
            if (Vector2.Distance(interactables[i].transform.position, transform.position) <= interactables[i].GetComponent<Interaction>().radius)
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
        animator.Play("LookAt");
    }

    public void OpenInventory()
    {
        if (!openInventory)
        {
            //DialogueControl.Instance.inventory.SetActive(true);
            DialogueControl.Instance.inventory.GetComponent<InventoryControl>().enabled = true;
            DialogueControl.Instance.inventory.transform.GetChild(0).gameObject.SetActive(true);
            DialogueControl.Instance.inventory.GetComponent<Image>().enabled = true;
            DialogueControl.Instance.dialogueText.gameObject.SetActive(false);
            DialogueControl.Instance.speakerText.gameObject.SetActive(false);
            InventoryControl.instance.LoadEvidence();
            openInventory = !openInventory;
        }
        else
        {
            //DialogueControl.Instance.inventory.SetActive(false);
            DialogueControl.Instance.inventory.GetComponent<InventoryControl>().enabled = false;
            DialogueControl.Instance.inventory.transform.GetChild(0).gameObject.SetActive(false);
            DialogueControl.Instance.inventory.GetComponent<Image>().enabled = false;
            DialogueControl.Instance.dialogueText.gameObject.SetActive(true);
            DialogueControl.Instance.speakerText.gameObject.SetActive(true);
            InventoryControl.instance.Close();
            openInventory = !openInventory;
        }
    }

    public void UpdateSpace(float maxSpace, float minSpace)
    {
        if (minSpace == 0 && maxSpace == 0) return;
        this.maxSpace = maxSpace;
        this.minSpace = minSpace;
    }
}

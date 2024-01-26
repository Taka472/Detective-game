using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DialogueControl : MonoBehaviour
{
    private static DialogueControl instance;
    public bool isTyping = false;
    [SerializeField] public Text dialogueText;
    [SerializeField] private Animator animator;
    private Story currentStory;
    public bool isPlaying;
    public float dialogueSpeed;
    public AudioSource source;
    public AudioClip[] clips;
    public int audioInterval;
    public float minPitch = 1;
    public float maxPitch = 3;
    public bool normalChoice = false;
    public InventoryControl inventoryControl;
    public Evidence[] evidences;
    public LocationControl locationControl;
    public NPCInteractionControl npcControl;
    public Location locations;

    public GameObject inventory;
    public GameObject choicePanel;

    [Header("Choice UI")]
    [SerializeField] private GameObject[] choices;
    private Text[] choicesText;

    public Vector3 characterPositionChange;
    public Vector3 cameraPositionChange;
    public Canvas mainCanvas;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue in scene");
        }
        instance = this;
        DontDestroyOnLoad(transform.parent.gameObject);
        DontDestroyOnLoad(GameObject.Find("Canvas"));
        DontDestroyOnLoad(GameObject.FindGameObjectWithTag("EventSystem"));
    }

    public static DialogueControl Instance { get { return instance; } }

    private void Start()
    {
        choicesText = new Text[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<Text>();
            index++;
        }
        StartCoroutine(SelectFirstChoice());
    }

    public IEnumerator EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        isPlaying = true;
        Movement.instance.isInteracting = true;
        animator.SetBool("isOpen", true);
        yield return new WaitForSeconds(0.1f);
        ContinueStory();
    }

    public void ShowEvidence(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        //StartCoroutine(TypeText(currentStory.Continue()));
    }

    private void ExitDialogue()
    {
        StopAllCoroutines();
        isPlaying = false;
        Movement.instance.isInteracting = false;
        animator.SetBool("isOpen", false);
        dialogueText.text = "";
        if (Movement.instance.interacting != null)
            if (Movement.instance.interacting.GetComponent<NPCControl>() != null)
                StartCoroutine(Movement.instance.interacting.GetComponent<NPCControl>().StopInteraction());
    }

    public void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            StopAllCoroutines();
            StartCoroutine(TypeText(currentStory.Continue()));
        }
        else
        {
            StopAllCoroutines();
            normalChoice = false;
            ExitDialogue();
        }
    }

    IEnumerator TypeText(string sentences)
    {
        int index = 0;
        dialogueText.color = Color.white;
        dialogueText.alignment = TextAnchor.UpperLeft;
        if (currentStory.currentTags.Count != 0)
        {
            switch (currentStory.currentTags[0])
            {
                case "yellow":
                    {
                        dialogueText.color = Color.yellow;
                        break;
                    }
                case "add":
                    {
                        npcControl.firstTime.Add(Movement.instance.interacting.GetComponent<NPCControl>().NPCID);
                        break;
                    }
                case "center":
                    {
                        dialogueText.alignment = TextAnchor.MiddleCenter;
                        switch(currentStory.currentTags[1])
                        {
                            case "addEvidence":
                                {
                                    AddEvidence(int.Parse(currentStory.currentTags[2]));
                                    break;
                                }
                            case "addLocation":
                                {
                                    AddLocation(int.Parse(currentStory.currentTags[2]));
                                    break;
                                }
                        }
                        break;
                    }
            }
        }
        isTyping = true;
        dialogueText.text = "";
        foreach(char letter in sentences.ToCharArray())
        {
            if (index == 0)
            {
                int random = letter.GetHashCode() % clips.Length;
                float max = maxPitch * 100, min = minPitch * 100, range = max - min;
                if (range == 0)
                {
                    source.pitch = min;
                } else {
                    float pitchIntValue = (letter.GetHashCode() % range) + min;
                    float pitchValue = pitchIntValue / 100f;
                    source.pitch = pitchValue;
                }
                source.Stop();
                source.PlayOneShot(clips[random]);
                index++;
            }
            else if (index == audioInterval)
            {
                index = 0;
            }
            else index++;
            if ((int)letter != 10 && (int)letter != 13)
                dialogueText.text += letter;
            yield return new WaitForSeconds(dialogueSpeed);
        }
        if (currentStory.currentChoices.Count > 0)
            DisplayChoices();
        isTyping = false;
    }

    public void AddEvidence(int index)
    {
        if (!inventoryControl.evidencesID.Contains(evidences[index]))
            inventoryControl.evidencesID.Add(evidences[index]);
    }

    public void AddLocation(int index)
    {
        if (!locationControl.locationNames.Contains(locations.locationName[index]))
            locationControl.locationNames.Add(locations.locationName[index]);
    }

    public void SkipSenetence()
    {
        dialogueText.text = currentStory.currentText.Trim();
        if (currentStory.currentChoices.Count > 0)
            DisplayChoices();
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;
        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices were given than the UI can support. Number of choices given: " + currentChoices.Count);
        }

        int index = 0;
        foreach (Choice choice in currentChoices)
        {
            choices[index].SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }
        StartCoroutine(SelectFirstChoice());
    }

    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return null;
        EventSystem.current.SetSelectedGameObject(choices[0]);
    }

    public void YesButton()
    {
        if (characterPositionChange != Vector3.zero && !normalChoice)
        {
            YesWithinButton();
        }
        else if (characterPositionChange == Vector3.zero && !normalChoice)
        {
            int index = 0;
            List<Choice> currentChoices = currentStory.currentChoices;
            foreach (Choice choice in currentChoices)
            {
                choices[index].SetActive(false);
                choicesText[index].text = choice.text;
                index++;
            }
            ExitDialogue();
            if (SceneManager.GetActiveScene().name.Equals("AtHome"))
                StartCoroutine(Cutscene1.instance.StartTransition(1));
            else
            {
                locationControl.gameObject.SetActive(true);
                locationControl.LoadLocation();
            }
        }
        else
        {
            int index = 0;
            List<Choice> currentChoices = currentStory.currentChoices;
            currentStory.ChooseChoiceIndex(0);
            foreach (Choice choice in currentChoices)
            {
                choices[index].SetActive(false);
                choicesText[index].text = choice.text;
                index++;
            }
        }
    }

    public void YesWithinButton()
    {
        int index = 0;
        List<Choice> currentChoices = currentStory.currentChoices;
        foreach (Choice choice in currentChoices)
        {
            choices[index].SetActive(false);
            choicesText[index].text = choice.text;
            index++;
        }
        ExitDialogue();
        StartCoroutine(Cutscene1.instance.StartTransitionInScene(characterPositionChange, 0, cameraPositionChange));
    }

    public void NoButton()
    {
        int index = 0;
        List<Choice> currentChoices = currentStory.currentChoices;
        currentStory.ChooseChoiceIndex(1);
        if (!normalChoice) currentStory.Continue();
        foreach (Choice choice in currentChoices)
        {
            choices[index].SetActive(false);
            choicesText[index].text = choice.text;
            index++;
        }
    }

    public void Update()
    {
        GameObject.Find("Canvas").GetComponent<Canvas>().worldCamera = Cutscene1.instance.main.GetComponent<Camera>();
    }
}

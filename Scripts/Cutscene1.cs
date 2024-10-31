using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using AssemblyCSharp;

public class Cutscene1 : MonoBehaviour
{
    public static Cutscene1 instance;

    public Animator animator;
    public Interaction interaction;
    public bool hasPlayed = false;
    public AudioSource source;

    public bool isTransition = false;
    public GameObject player;
    public Camera main;

    [SerializeField] private InventoryControl inventoryControl;
    [SerializeField] private LocationControl locationControl;
    [SerializeField] private NPCInteractionControl npcInteractionControl;
    [SerializeField] private DataManager dataManager;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayCutscene());
        // DontDestroyOnLoad(gameObject);
        instance = this;
    }

    IEnumerator PlayCutscene()
    {
        if (SceneManager.GetActiveScene().name.Equals("AtHome"))
        {
            DialogueControl.instance.isPlaying = true;
            yield return new WaitForSeconds(1);
            StartCoroutine(DialogueControl.instance.EnterDialogueMode(interaction.inkJSON));
        }
    }

    public void DummyTransition(int index)
    {
        StartCoroutine(StartTransition(index));
    }

    public IEnumerator StartTransition(int scene)
    {
        Save(scene);
        source.Play();
        Movement.instance.isInteracting = true;
        isTransition = true;
        animator.SetBool("Fade", false);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(scene);
        StartCoroutine(EndTransition());
    }

    public IEnumerator StartTransitionInScene(Vector3 position, int rotation, Vector3 camera)
    {
        source.Play();
        isTransition = true;
        Movement.instance.isInteracting = true;
        Movement.instance.interactKey.SetActive(false);
        animator.SetBool("Fade", false);
        yield return new WaitForSeconds(1);
        player.transform.position = position;
        player.transform.eulerAngles = new Vector2(0, rotation);
        if (camera != Vector3.zero)
            CameraPositionChange(camera);
        StartCoroutine(EndTransition());
    }

    IEnumerator EndTransition()
    {
        yield return new WaitForSeconds(1);
        source.Stop();
        animator.SetBool("Fade", true);
        isTransition = false;
        Movement.instance.isInteracting = false;
    }

    private void Update()
    {
        if (!DialogueControl.instance.isPlaying && !hasPlayed)
        {
            animator.SetBool("Fade", true);
            interaction.enabled = false;
            hasPlayed = true;
        }
    }

    public void CameraPositionChange(Vector3 position)
    {
        main.transform.position = position;
    }

    private void Save(int index)
    {
        PlayerModel model = new();

        List<int> evidenceIDList = new();
        foreach(Evidence e in inventoryControl.evidencesID)
        {
            evidenceIDList.Add(e.ID);
        }

        model.evidenceIDList = evidenceIDList;
        model.locationList = locationControl.locationNames;
        model.firstTimeInteractionList = npcInteractionControl.firstTime;
        model.currentScene = index;
        dataManager.SaveData(model);
    }

    public byte[] SpriteToByteArray(Sprite sprite)
    {
        Texture2D texture = sprite.texture;

        if (!texture.isReadable)
        {
            Debug.LogError("Texture is not readable. Check import settings.");
            return null;
        }

        Texture2D newTexture = new((int)sprite.rect.width, (int)sprite.rect.height);

        Color[] pixels = texture.GetPixels(
            (int)sprite.textureRect.x,
            (int)sprite.textureRect.y,
            (int)sprite.textureRect.width,
            (int)sprite.textureRect.height
        );

        newTexture.SetPixels(pixels);
        newTexture.Apply();

        byte[] bytes = newTexture.EncodeToPNG();

        Destroy(newTexture);
        return bytes;
    }

}
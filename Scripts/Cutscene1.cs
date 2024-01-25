using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cutscene1 : MonoBehaviour
{
    public static Cutscene1 instance;

    public Animator animator;
    Interaction interaction;
    public bool hasPlayed = false;
    public AudioSource source;

    public bool isTransition = false;
    public GameObject player;
    public GameObject main;

    // Start is called before the first frame update
    void Start()
    {
        interaction = GetComponent<Interaction>();
        StartCoroutine(PlayCutscene());
        DontDestroyOnLoad(gameObject);
        FindReference();
        instance = this;
    }

    IEnumerator PlayCutscene()
    {
        DialogueControl.Instance.isPlaying = true;
        yield return new WaitForSeconds(1);
        StartCoroutine(FindObjectOfType<DialogueControl>().EnterDialogueMode(interaction.inkJSON));
    }

    public void DummyTransition(int index)
    {
        StartCoroutine(StartTransition(index));
    }

    public IEnumerator StartTransition(int scene)
    {
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
        animator.SetBool("Fade", false);
        yield return new WaitForSeconds(2);
        player.transform.position = position;
        player.transform.eulerAngles = new Vector2(0, rotation);
        if (camera != Vector3.zero)
            CameraPositionChange(camera);
        StartCoroutine(EndTransition());
    }

    IEnumerator EndTransition()
    {
        FindReference();
        yield return new WaitForSeconds(1);
        source.Stop();
        animator.SetBool("Fade", true);
        isTransition = false;
        Movement.instance.isInteracting = false;
    }

    private void Update()
    {
        if (!DialogueControl.Instance.isPlaying && !hasPlayed)
        {
            animator.SetBool("Fade", true);
            GetComponent<Interaction>().enabled = false;
            hasPlayed = true;
        }
        FindReference();
    }

    public void CameraPositionChange(Vector3 position)
    {
        main.transform.position = position;
    }

    public void FindReference()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        main = GameObject.FindGameObjectWithTag("MainCamera");
    }
}
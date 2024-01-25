using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCControl : MonoBehaviour
{
    public Transform player;
    public bool isLookingLeft;
    public Animator animator;
    //public Animation turningAnimation;

    public int NPCID;
    public TextAsset[] randomResponse;
    public TextAsset[] evidenceResponse;

    public void PlayerInteraction()
    {
        bool standingLeft = false;
        if (player.position.x <= transform.position.x)
        {
            standingLeft = true;
        }
        if (standingLeft && !isLookingLeft || !standingLeft && isLookingLeft)
        {
            animator.Play("LookAt");
        }
    }

    public IEnumerator StopInteraction()
    {
        bool standingLeft = false;
        if (player.position.x <= transform.position.x)
        {
            standingLeft = true;
        }
        if (standingLeft && !isLookingLeft || !standingLeft && isLookingLeft)
        {
            animator.Play("LookBack");
            yield return new WaitForSeconds(2);
        }
        animator.Play("Idle");
    }
}

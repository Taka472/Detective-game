using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private Vector3 off_set = new(0, 0, -10);
    private readonly float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;
    private Vector3 defaultLocation;

    [SerializeField] private Transform player;
    [SerializeField] private float minSpace;

    private void Start()
    {
        defaultLocation = transform.position;
        off_set = new(0, transform.position.y - player.transform.position.y, -10);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.x >= minSpace)
        {
            Vector3 target = player.position + off_set;
            transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime);
        }
    }
}

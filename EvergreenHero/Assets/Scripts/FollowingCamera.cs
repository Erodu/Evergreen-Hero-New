using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    /* We start off by having an offset for the camera so that the player isn't always in the center of the screen. We then define a smoothing time which will allow us to have smooth camera tracking. We then define the camera's target
     * as a transform, and in the editor we can drag the player GameObject into the field.
     * 
     Every update, we have the camera follow its target's position plus the offset defined, and we use Vector3.SmoothDamp() in order to allow for smooth camera movement. */
    private Vector3 offset = new Vector3(0f, 0f, -10f);
    private float smoothingTime = 0.25f;
    private Vector3 velocity = Vector3.zero;

    [SerializeField] private Transform cameraTarget;

    void Update()
    {
        Vector3 targetPos = cameraTarget.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothingTime); //For smooth camera movement
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampCamera : MonoBehaviour
{
    /*
    * The Purpose of this script is to have the camera following the player
    * The camera is to be limited to play area
    */

    // CONFIGURABLE VARIABLES //
    // Assigned in Unity - get the transform of the object the camera is to follow.
    [Header("Target To Follow")]
    [SerializeField] Transform targetToFollow = null;
    
    // Assigned in Unity - these variables are to set the Clamp on the Camera
    [Header("Clamping Dimensions")]
    [SerializeField] float xPosRight = 1.22f;
    [SerializeField] float xPosLeft = -1.22f;
    [SerializeField] float yPosUp = 0.7f;
    [SerializeField] float yPosDown = -0.7f;

    // Update is called once per frame
    void Update()
    {
        // Using Mathf.Clamp to keep the camera view limited to the play area.
        transform.position = new Vector3(
            (Mathf.Clamp(targetToFollow.position.x, xPosLeft, xPosRight)), 
            (Mathf.Clamp(targetToFollow.position.y, yPosDown, yPosUp)), 
            transform.position.z);
    }
}

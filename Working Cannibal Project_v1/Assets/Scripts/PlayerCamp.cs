using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamp : MonoBehaviour
{
    // Assigned in Unity - these variables are to set the Clamp on the Player's Movement.
    // Limit the Player from going "out of bounds".
    [Header("Clamping Dimensions")]
    [SerializeField] float xPosRight = 20.0f;
    [SerializeField] float xPosLeft = -20.0f;
    [SerializeField] float yPosUp = 14.0f;

    // Update is called once per frame
    void Update()
    {
        ClampPlayerPosition();
    }

    private void ClampPlayerPosition()
    {
        // Clamp the player position to keep Player "in-bounds".
        transform.position = new Vector3(
            (Mathf.Clamp(transform.position.x, xPosLeft, xPosRight)), 
            (Mathf.Clamp(transform.position.y, transform.position.y, yPosUp)), 
            0);
    }
}

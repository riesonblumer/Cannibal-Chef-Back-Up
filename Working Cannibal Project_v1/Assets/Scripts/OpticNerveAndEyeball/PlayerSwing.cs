using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwing : MonoBehaviour
{
    //this script determines what happens when player is swinging on the optic nerve

    //Assigned in inspector
    [SerializeField] GameObject opticNerveAndEyeball = null;
    //probably unnecessary, not in use right now
    //[SerializeField] DistanceJoint2D jointToWeight;
    [SerializeField] float exitJumpVelocity = 30f;
    //adding force to the eyeball doesn't seem to do anything while everything is jointed together
    [SerializeField] float weightForce = 30f;
    [SerializeField] PlayerMovement playerMovementScript;

    // cached references
    Rigidbody2D playerRigidbody = null;
    GameObject opticNerveAndEyeballInstance = null;

    // initialized variables
    bool swinging = false;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // input to throw the eyeball out
        if (Input.GetButtonDown("Jump"))
        {
            // checks if airborn
            if (FindObjectOfType<PlayerMovement>().ReturnIsOnGround() == false)
            {
                if (!swinging)
                {
                    //instantiates the opticNervePrefab
                    // prefab should already be at correct angle
                    if (FindObjectOfType<PlayerMovement>().facingRight)
                    {
                        opticNerveAndEyeballInstance = 
                            Instantiate(opticNerveAndEyeball, transform.position, opticNerveAndEyeball.transform.rotation);
                    }
                    else
                    {
                        opticNerveAndEyeballInstance =
                            Instantiate(opticNerveAndEyeball, transform.position, 
                            Quaternion.Euler(-opticNerveAndEyeball.transform.rotation.eulerAngles));
                    }
                    
                    //trying to add more force to the eyeball, doesn't seem to be working
                    FindObjectOfType<EyeballStick>().GetComponent<Rigidbody2D>().AddForce(transform.right * -weightForce);

                    //using a DistanceJoint doesn't seem necessary
                    //jointToWeight.enabled = true;
                    //jointToWeight.connectedBody = FindObjectOfType<EyeballStick>().GetComponent<Rigidbody2D>();

                    //disabling the playerMovementScript prevents you from moving and detaching yourself from the joints
                    playerMovementScript.enabled = false;
                    swinging = true;
                }

                else
                {
                    // while swinging, you can jump once
                    playerMovementScript.enabled = true;
                    Destroy(opticNerveAndEyeballInstance);
                    swinging = false;
                    ExitJump();
                }
            }
        }
    }

    private void ExitJump()
    {
        playerRigidbody.velocity = Vector2.up * exitJumpVelocity;
    }
}

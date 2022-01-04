using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstLink : MonoBehaviour
{
    Rigidbody2D playerRig;
    HingeJoint2D hingeToPlayer;

    void Start()
    {
        //creates a hinge joint on the first link of the optic nerve and attaches it to the player
        hingeToPlayer = gameObject.AddComponent(typeof(HingeJoint2D)) as HingeJoint2D;
        playerRig = FindObjectOfType<PlayerSwing>().GetComponent<Rigidbody2D>();
        hingeToPlayer.connectedBody = playerRig;
    }
}

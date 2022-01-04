using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLinks : MonoBehaviour
{
    private EyeballStick eyeball;
    private Rigidbody2D eyeballRig;

    private void Start()
    {
        eyeball = FindObjectOfType<EyeballStick>();
        eyeballRig = eyeball.GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "peg")
        {
            // keeps weight from moving when it touches the peg
            // this is so that if the end of the optic nerve touches a peg, the ball will still stick
            // this is a bandaid until mechanic properly wraps around peg
            eyeball.transform.position = collision.gameObject.transform.position;
            eyeballRig.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        }
    }
}

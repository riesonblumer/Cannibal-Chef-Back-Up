using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeballStick : MonoBehaviour
{
    // tunable values
    // not in use
    //[SerializeField] float forceAmount = 10f;

    // assigned in inspector
    [SerializeField] GameObject eyeballAndOpticNerve;

    // cached references
    private Rigidbody2D rig;

    // initialized variables
    //not in use
    //bool wrapping = true;

    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        //if this is uncommented, can swing anywhere
        //rig.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
    }
    
    // added force to try to wrap, doesn't seem to do anything
    /*

    private void Update()
    {
        if (wrapping)
        {
            rig.AddForce(transform.up * -forceAmount);
        }
    }
    */

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // if colliding with peg, freeze the eyeball in space
        if (collision.gameObject.name == "peg")
        {
            // an attempt to snap everything to the peg
            /*

            GameObject peg = collision.gameObject;

            float distanceX = eyeballAndOpticNerve.transform.position.x +
                (peg.transform.position.x - eyeballAndOpticNerve.transform.position.x);
            float distanceY = eyeballAndOpticNerve.transform.position.y +
                (peg.transform.position.y - eyeballAndOpticNerve.transform.position.y);

            // locks eyeball to peg
            eyeballAndOpticNerve.transform.position = new Vector3(distanceX, distanceY);

            */

            rig.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
            transform.position = collision.gameObject.transform.position;
            //wrapping = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // configurable variables
    [Header("Player Movement Settings")]
    [SerializeField] float moveSpeed = 10f;

    [Header("Player Jump Settings")]
    [SerializeField] float jumpVelocity = 10f;
    [SerializeField] float groundCheckRadius = .5f; 
    [SerializeField] int extraJumpTotal = 2;
    [SerializeField] Transform groundCheckTransform;
    [SerializeField] LayerMask groundLayerMask;
    [SerializeField] float coyoteTime = .2f;
    [SerializeField] float jumpBufferLength = .2f;

    /* [Header("Player Climb Settings")]
    [SerializeField] float climbSpeed = 5f; 
    [SerializeField] Collider2D hiddenPlatform; // dependent on game level. */

    // stated variables
    Rigidbody2D playerRigidbody;
    Collider2D playerCollider;

    public bool facingRight = true;
    private bool isOnGround;
    private bool isClimbing;
    private float playerGravityScale;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;
    private bool canWallJump = false;

    [SerializeField] private int extraJumps; //Serialized for Debugging

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        playerGravityScale = playerRigidbody.gravityScale;
        extraJumps = extraJumpTotal;
    }

    void Update()
    {
        Jump();
        // Climb();
    }

    void FixedUpdate()
    {
        GroundCheck();
        Move();
    }

    private void GroundCheck()
    {
        // Need to determine if player is on ground for jumping.
        isOnGround = Physics2D.OverlapCircle(groundCheckTransform.position, groundCheckRadius, groundLayerMask);
    }

    private void Move()
    {
        // To move the player left and right along the x axis.
        var moveDeltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var moveNewXPos = transform.position.x + moveDeltaX;
        transform.position = new Vector2(moveNewXPos, transform.position.y);
        SpriteFlip();
    }


    private void SpriteFlip()
    {
        // Need to flip sprite so it doesn't look like it's walking backwards.
        facingRight = !facingRight;

        if(facingRight == false && Input.GetAxis("Horizontal") > 0)
        {
            facingRight = true;
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if(facingRight == true && Input.GetAxis("Horizontal") < 0)
        {
            facingRight = false;
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    private void Jump()
    {
        // To allow for a buffer to press the jump button while jumping
        if(Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferLength;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // To reset coyote time and extra jump counters, and start coyote timer
        if(isOnGround)
        {
            coyoteTimeCounter = coyoteTime;
            extraJumps = extraJumpTotal;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // To manage standard jumping.
        if(jumpBufferCounter >=0 && extraJumps >= 0 && !isClimbing && coyoteTimeCounter > 0f)
        {
            playerRigidbody.velocity = Vector2.up * jumpVelocity;
            jumpBufferCounter = 0f;
        }

        // To create variation in jumping heights determined by button press duration.
        if(Input.GetButtonUp("Jump") && playerRigidbody.velocity.y > 0)
        {
            coyoteTimeCounter = 0f; // Stops weird double jump when spamming jump button.
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, playerRigidbody.velocity.y *.5f);
        }

        // To decrease the number of extra jumps after use.
        else if(Input.GetButtonDown("Jump") && extraJumps > 0 && !isClimbing)
        {
            playerRigidbody.velocity = Vector2.up * jumpVelocity;
            extraJumps--;
        }

        if(canWallJump && Input.GetButtonDown("Jump"))
        {
            playerRigidbody.velocity = new Vector2(moveSpeed * -1, jumpVelocity);
        }
    }

    // just to check whether hand is airborn basically
    // so that we can see if they can instantiate the eyeball and optic nerve
    public bool ReturnIsOnGround()
    {
        return isOnGround;
    }

    /* private void Climb()
    {
        // To reset gravity scale when not On climbing layer.
        if(!playerCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"))) 
        { 
            isClimbing = false;
            playerRigidbody.gravityScale = playerGravityScale;
            hiddenPlatform.isTrigger = false;
        }

        // To climb.
        // Certain platforms' colliders can be turned off to move through floors.
            // Should probably include some sort of fade to the platforms visibility (ghost the floor?) 
        if(playerCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")) && (Input.GetAxis("Vertical") > 0 || Input.GetAxis("Vertical") < 0))
        { 
            isClimbing = true;
            playerRigidbody.gravityScale = 0f;

            float controlThrow = Input.GetAxis("Vertical");
            Vector2 climbVelocity = new Vector2(playerRigidbody.velocity.x, controlThrow * climbSpeed);
            playerRigidbody.velocity = climbVelocity;

            hiddenPlatform.isTrigger = true;
        }
    }  

    // for climbing ooze
    private void OnTriggerStay2D(Collider2D otherCollider)
    {   
        if (otherCollider.tag == "Ooze Right")
        {
            playerRigidbody.gravityScale = 0f;
            transform.eulerAngles = new Vector3(-180, 0, 90);
            transform.position = new Vector2(otherCollider.transform.position.x - .75f, transform.position.y);

            float controlThrow = Input.GetAxis("Vertical");
            Vector2 climbVelocity = new Vector2(playerRigidbody.velocity.x, controlThrow * climbSpeed);
            playerRigidbody.velocity = climbVelocity;
        }

        if (otherCollider.tag == "Ladder" && (Input.GetAxis("Vertical") > 0 || Input.GetAxis("Vertical") < 0))
        {
            transform.position = new Vector2(otherCollider.transform.position.x, transform.position.y);
        }

        if (otherCollider.tag == "Ladder" && isClimbing && !isOnGround)
        {
            if (Input.GetAxis("Horizontal") > 0)
            {
                transform.position = new Vector2(transform.position.x + 1, transform.position.y);
            }

            if (Input.GetAxis("Horizontal") < 0)
            {
                transform.position = new Vector2(transform.position.x - 1, transform.position.y);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.tag == "Ooze Right")
        {
            canWallJump = true;
        }
    }
    private void OnTriggerExit2D(Collider2D otherCollider)
    {
        if (otherCollider.tag == "Ooze Right")
        {
            canWallJump = false;
        }
    } */
}

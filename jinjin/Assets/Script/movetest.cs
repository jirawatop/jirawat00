using UnityEngine;
using UnityEngine.SceneManagement;
public class movetest : MonoBehaviour
{
    public bool drawDebugRaycast = true;
    [Header("Movement Properties")]
    public float speed = 8f;
    public float crouchSpeedDivisor = 3f;
    public float coyoteDuration = 0.5f;
    public float maxFallSpeed = -25f;


    [Header("Jump Properties")]
    public float JumpForce = 6.3f;
    public float crouchJupeBoot = 2.5f;
    public float hangingJumpForce = 15f;
    public float jumpHoldForce = 1.9f;
    public float jumpHoldDuration = .1f;

    [Header("checked")]
    public float footOffset = .4f;
    public float eyeHeight = 1.5f;
    public float reachOffset = .7f;
    public float headClearance = .5f;
    public float groundDistance = .2f;
    public float grabDistance = .4f;
    public LayerMask groundLayer;


    [Header("Status ")]
    public bool isOnGround;
    public bool isJumping;
    public bool isHanging;
    public bool isCrouching;
    public bool isHeadBlocked;
    inputtest input;
    Rigidbody2D rigidbody2d;
    BoxCollider2D collider2d;
   

   

    private float originalXScale;
    private int direction = 1;

    private float jumpTime;
    private float coyoteTime;
    private float playerHeight;


    Vector2 colliderStandSize;
    Vector2 colliderStandOffset;
    Vector2 colliderCrouchSize;
    Vector2 colliderCrouchOffset;


    

    private const float smallAmount = .5f;
    private void Awake()
    {
        

    }
    private void Start()
    {
      


        
        input = GetComponent<inputtest>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<BoxCollider2D>();
        originalXScale = transform.localScale.x;
        playerHeight = collider2d.size.y;
        colliderStandSize = collider2d.size;
        colliderStandOffset = collider2d.offset;
        colliderCrouchSize = new Vector2(collider2d.size.x, collider2d.size.y / 2f);
        colliderCrouchOffset = new Vector2(collider2d.offset.x, collider2d.offset.y / 2f);
    }



    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        PhysicsCheck();
        GroundMovement();
        MidAirMovement();
       
    }
    
    private void PhysicsCheck()
    {
        isOnGround = false;
        isHeadBlocked = false;

        RaycastHit2D leftCheck = Raycast(new Vector2(-footOffset, 0f), Vector2.down, groundDistance);
        RaycastHit2D rightCheck = Raycast(new Vector2(footOffset, 0f), Vector2.down, groundDistance);
        //Debug.Log(leftCheck);
        //Debug.Log(rightCheck);
        if (leftCheck || rightCheck)
            isOnGround = true;
        RaycastHit2D headCheck = Raycast(new Vector2(0f, collider2d.size.y), Vector2.up, headClearance);
        if (headCheck)
            isHeadBlocked = true;
        Vector2 grabDir = new Vector2(direction, 0f);
        RaycastHit2D blockedCheck = Raycast(new Vector2(footOffset * direction, playerHeight), grabDir, grabDistance);
        RaycastHit2D ledgeCheck = Raycast(new Vector2(reachOffset * direction, playerHeight), Vector2.down, grabDistance);
        RaycastHit2D wallCheck = Raycast(new Vector2(footOffset * direction, eyeHeight), grabDir, grabDistance);

        if (!isOnGround && !isHanging && rigidbody2d.velocity.y < 0f &&
            ledgeCheck && wallCheck && !blockedCheck)
        {
            Vector3 pos = transform.position;
            pos.x += (wallCheck.distance - smallAmount) * direction;
            pos.y -= ledgeCheck.distance;
            transform.position = pos;
            rigidbody2d.bodyType = RigidbodyType2D.Static;
            isHanging = true;
        }
    }
    void GroundMovement()
    {
        if (isHanging)
            return;

        if (input.crouchHeld && isOnGround)
         
            Crouch();
         
        else if(!input.crouchHeld && isCrouching)
            StandUp();
        
       
        
        float xVelocity = speed * input.horizontal;

        if (xVelocity * direction < 0f)
            FlipCharacterDirection();
        if (isCrouching)
            xVelocity /= crouchSpeedDivisor;

        rigidbody2d.velocity = new Vector2(xVelocity, rigidbody2d.velocity.y);

        if (isOnGround)
            coyoteTime = Time.time + coyoteDuration;
    }

    private void MidAirMovement()
    {
        if (isHanging)
        {
            if (input.crouchPressed)
            {
                isHanging = false;
                rigidbody2d.bodyType = RigidbodyType2D.Dynamic;
                return;
            }

            if (input.jumpPressed)
            {
                isHanging = false;
                rigidbody2d.bodyType = RigidbodyType2D.Dynamic;
                rigidbody2d.AddForce(new Vector2(0f, hangingJumpForce), ForceMode2D.Impulse);
                return;
            }
        }

        if (input.jumpPressed && !isJumping && isOnGround)
        {
            if (isCrouching && !isHeadBlocked)
            {
               

                StandUp();
                //rigidbody2d.AddForce(new Vector2(0f, crouchJupeBoot), ForceMode2D.Impulse);
            }

            isOnGround = false;
            isJumping = true;

            jumpTime = Time.time + jumpHoldDuration;

            rigidbody2d.AddForce(new Vector2(0f, JumpForce), ForceMode2D.Impulse);

           // AudioManager.PlayJumpAudio();
        }
        else if (isJumping)
        {
            if (input.jumpHeld)
                rigidbody2d.AddForce(new Vector2(0f, jumpHoldForce), ForceMode2D.Impulse);

            if (jumpTime <= Time.time)
                isJumping = false;
        }

        if (rigidbody2d.velocity.y < maxFallSpeed)
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, maxFallSpeed);

    }

    private void FlipCharacterDirection()
    {
        direction *= -1;
        Vector3 scale = transform.localScale;
        scale.x = originalXScale * direction;
        transform.localScale = scale;

    }

    private void Crouch()
    {
        isCrouching = true;
        collider2d.size = colliderCrouchSize;
        collider2d.offset = colliderCrouchOffset;
    }
    private void StandUp()
    {
        if (isHeadBlocked)
            return;

        isCrouching = false;
        collider2d.size = colliderStandSize;
        collider2d.offset = colliderStandOffset;
       
    }

    private RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length)
    {
        return Raycast(offset, rayDirection, length, groundLayer);
    }


    private RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length, LayerMask mask)
    {
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDirection, length, mask);
        if (drawDebugRaycast)
        {
            Color color = hit ? Color.red : Color.green;
            Debug.DrawRay(pos + offset, rayDirection * length, color);

        }

        return hit;
    }


}

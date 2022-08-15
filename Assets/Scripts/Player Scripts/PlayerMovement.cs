using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D myBody;

    [SerializeField]
    private float moveSpeed = 5f;

    private float horizontalMovement;

    private PlayerAnimation playerAnim;

    [SerializeField]
    private float normalJumpForce = 5f, doubleJumpForce = 5f;

    private float jumpForce = 5f;

    private RaycastHit2D groundCast;
    private BoxCollider2D boxCol2D;

    [SerializeField]
    private LayerMask groundMask;

    private bool canDoublejump;
    private bool jumped;


    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<PlayerAnimation>();
        boxCol2D = GetComponent<BoxCollider2D>();

        canDoublejump = true;

    }

    private void Update()
    {
        horizontalMovement = Input.GetAxisRaw(TagManager.HORIZONTAL_MOVEMENT_AXIS);
        HandleAnimation();
        HandleJumping();
        CheckToDoubleJump();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        if (horizontalMovement > 0)
        {
            myBody.velocity = new Vector2(moveSpeed, myBody.velocity.y);
        }
        else if (horizontalMovement < 0)
        {
            myBody.velocity = new Vector2(-moveSpeed, myBody.velocity.y);
        }
        else
        {
            myBody.velocity = new Vector2(0f, myBody.velocity.y);
        }
    }

    void HandleAnimation()
    {
        //if we are on the ground
        if (myBody.velocity.y == 0f)
            playerAnim.PlayWalk(Mathf.Abs((int)myBody.velocity.x));

        playerAnim.ChangeFacingDirection((int)myBody.velocity.x);

        playerAnim.PlayJumpAndFall((int)myBody.velocity.y);
    }

    void HandleJumping()
    {
        if (Input.GetButtonDown(TagManager.JUMP_BUTTON))
        {
            if (IsGrounded())
            {
                jumpForce = normalJumpForce;
                Jump();
            }
            else
            {
                if (canDoublejump)
                {
                    canDoublejump = false;
                    jumpForce = doubleJumpForce;
                    Jump();
                }
            }
                
        }
    }

    bool IsGrounded()
    {
        //groundCast = Physics2D.Raycast(boxCol2D.bounds.center, Vector2.down,
        //    boxCol2D.bounds.extents.y + 0.02f, groundMask);

        //Debug.DrawRay(boxCol2D.bounds.center,
        //    Vector2.down * (boxCol2D.bounds.extents.y + 0.02f), Color.red);

        groundCast = Physics2D.BoxCast(boxCol2D.bounds.center,
            boxCol2D.bounds.size, 0f, Vector2.down, 0.02f, groundMask);

        return groundCast.collider != null;
    }

    void Jump()
    {
        myBody.velocity = Vector2.up * jumpForce;
        jumped = true;
    }

    void CheckToDoubleJump()
    {
        if (!canDoublejump && myBody.velocity.y == 0f)
            canDoublejump = true;
    }

} // class 

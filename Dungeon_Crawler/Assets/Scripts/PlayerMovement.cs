using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Variables
    [SerializeField] private float moveSpeed; // serialize to allow to access these private values in 
                                                // inspector, but not allow full access like public
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;

    private Vector3 moveDirection;
    private Vector3 velocity;

    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity;

    [SerializeField] private float jumpHeight;

    // References

    private CharacterController controller;

    private Animator anim;

    // Attack variables & references
    private GameObject sword;
    [SerializeField] private bool canAttack = true;
    [SerializeField] private float attackCoolDown = 0.4f;

    public bool isAttacking; // public to be used in other scripts

    private AudioSource swordSwing;
    [SerializeField] private AudioClip swordSwingClip;



    private void Start()
    {
        controller = GetComponent<CharacterController>(); // goes through COMPONENTS in gameobject lookng for character controller

        anim = GetComponentInChildren<Animator>(); // goes through components in children of object

        // sword sound
        swordSwing = GetComponent<AudioSource>();
    }

    private void Update()
    {
        Move();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (canAttack)
            {
                Attack();
            }
        }
    }

    private void Move()
    {
        // is grounded = (player position, distance from ground, layer mask of ground)
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2.0f;
        }

        // move forward / backward on Z axis using Vertical input from Input Manager
        float moveZ = Input.GetAxis("Vertical");

        moveDirection = new Vector3(0, 0, moveZ); // move forward/back based on key input
        moveDirection = transform.TransformDirection(moveDirection); // change forward to be direction player is facing (vs global). Facing direciton transformed in camera controller.


        // set moveDirection value to be equal to itself * walk/runSpeed bc w/o too slow
        //moveDirection *= walkSpeed; test code

        if (isGrounded) // character on ground?
        {
            // check moveDirection & left shift press to determine moveSpeed level
            if (moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift)) // speed not 0 AND not holding left shift key
            {
                //Walk
                Walk();

            }
            else if (moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
            {
                //Run
                Run();
            }
            else if (moveDirection == Vector3.zero)
            {
                //Idle
                Idle();
            }

            moveDirection *= moveSpeed;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }

        // check moveDirection & left shift press to determine moveSpeed level
        // move to inside isGrounded and only apply if character on ground
        ////if (moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift)) // speed not 0 AND not holding left shift key
        ////{
        ////    //Walk
        ////    Walk();

        ////}
        ////else if (moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
        ////{
        ////    //Run
        ////    Run();
        ////}
        ////else if (moveDirection == Vector3.zero)
        ////{
        ////    //Idle
        ////    Idle();
        ////}

        //moveDirection *= moveSpeed; move to inside isGrounded IF statement above

        controller.Move(moveDirection * Time.deltaTime); // action independednt of frame rate, happens at constant rate     

        velocity.y += gravity * Time.deltaTime; // calculate gravity
        controller.Move(velocity * Time.deltaTime); // apply gravity to character
    
    }

    private void Idle()
    {
        anim.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
    }

    private void Walk()
    {
        moveSpeed = walkSpeed;

        anim.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
    }

    private void Run()
    {
        moveSpeed = runSpeed;

        anim.SetFloat("Speed", 1f, 0.1f, Time.deltaTime);
    }
    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);

        anim.SetTrigger("JumpTrigger");
    }

    private void Attack()
    {
        isAttacking = true;
        canAttack = false; // cannot attack again on mouse click until made true - prevent spamming
        anim.SetTrigger("Attack"); // animation attack

        swordSwing.PlayOneShot(swordSwingClip);

        StartCoroutine(ResetAttackCooldown());

    }

    IEnumerator ResetAttackCooldown()
    {
        StartCoroutine(ResetAttackBool());

        yield return new WaitForSeconds(attackCoolDown); // variable set above - wait time
        canAttack = true; // switch back to true to be able to attack again
    }

    IEnumerator ResetAttackBool()
    {
        yield return new WaitForSeconds(0.4f);
        isAttacking = false;
    }
}

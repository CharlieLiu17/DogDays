using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    private Transform cam;
    public Animator anim;
    public Rigidbody rb;

    public float colliderY;
    public bool jumpBool = false; // For animation
    public bool jumping = false;
    public float jumpFloat = 4f;
    public float jumpLag = 0.5f;
    public float speed = 6f;
    public float runSpeed = 12f;
    public int moveInt = 0;  // For animation
    public float smoothTurnTime = 0.1f; //
    public float gravity = 8f;
    public Vector3 moveDirection = new Vector3(0f, 0f, 0f);
    float turnSmoothVelocity;
    float finalSpeed = 1;
    private int buildIndex;


    private void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
        buildIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnEnable()
    {
        controller.center = new Vector3(controller.center.x, colliderY, controller.center.z);
    }
    // Update is called once per frame
    void Update()
    {
        float targetAngle = 0f;
        if (controller.isGrounded)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
            moveInt = 0;
            if (direction.magnitude >= 0.1f)
            {
                if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
                {
                    finalSpeed = runSpeed;
                    moveInt = 2;
                }
                else
                {
                    finalSpeed = speed;
                    moveInt = 1;
                }
                //basically this code determines the direction you are 
                // inputting as well accounting for the camera angle so for example, you always go forward from the perspective of the camera when you press W
                targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, smoothTurnTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                anim.SetInteger("Movement Int", moveInt);
            }
            else
            {
                anim.SetInteger("Movement Int", moveInt); //
                if (!(anim.GetCurrentAnimatorStateInfo(0).IsName("Jump") || anim.GetCurrentAnimatorStateInfo(0).IsName("JumpRun")))
                {
                    moveDirection = new Vector3(0f, 0f, 0f);
                }
            }
        }

        //jump
        if (Input.GetButtonDown("Jump") && controller.isGrounded && !(anim.GetCurrentAnimatorStateInfo(0).IsName("Jump") || anim.GetCurrentAnimatorStateInfo(0).IsName("JumpRun")))
        {
            anim.SetBool("Jump Bool", true);
            jumpBool = true;
        }
        
        //jumpAnimation
        else
        {
            if (jumpBool) // This means that the animation of jumping should be in effect
            {
                if (!anim.IsInTransition(0)) //
                {
                    controller.center = new Vector3(controller.center.x, anim.GetFloat("ColliderCurve"), controller.center.z);
                }
                // colliderY is the original y position of the character controller's collider, and if the player is under it, then terminate the jump
                // this is the mechanism that always turns off the jumping and the collider following the animation curve
                // Doesn't reset player collider, this may be the problem
                if (anim.GetFloat("ColliderCurve") <= colliderY && jumping == true)
                {
                    jumpBool = false;
                    jumping = false;
                    anim.SetBool("Jump Bool", false);
                }
                //it's true if the dog is still above the colliderY
                // This could definitely be the problem! If the curve is not synced up with the jump, the collider may for some reason always be too high?
                else if (anim.GetFloat("ColliderCurve") > colliderY)
                {
                    jumping = true;
                }
                //gravity is still affecting it
                moveDirection.y -= anim.GetFloat("GravityCurve") * Time.deltaTime;
                //this accounts for the direction and magnitued of the dog while its jumping
                controller.Move(moveDirection.normalized * finalSpeed * Time.deltaTime);
            }
            else //if the dog isn't jumping, the dog moves as normal and is always affected by gravity
            {
                moveDirection.y -= gravity * Time.deltaTime;
                controller.Move(moveDirection.normalized * finalSpeed * Time.deltaTime);
            }
        }
        
    }

    private void OnDisable()
    {
        jumpBool = false;
        anim.SetBool("Jump Bool", false);
        anim.SetInteger("Movement Int", 0); //change this to ai script later
        controller.center = new Vector3(controller.center.x, colliderY, controller.center.z);

    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
        Debug.Log("HELLJFOIJDASOIFJ");
    }
}

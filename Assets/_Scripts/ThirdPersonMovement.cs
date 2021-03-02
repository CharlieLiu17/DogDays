using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public Animator anim;

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
    


    private void OnEnable()
    {
        controller.center = new Vector3(controller.center.x, colliderY, controller.center.z);
    }
    // Update is called once per frame
    void Update()
    {
        float targetAngle = 0f;
        Vector3 temp = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);
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
                } else
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
            } else
            {
                anim.SetInteger("Movement Int", moveInt); //idle
            }
        }

        float deltaRotation = (new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z) - temp).magnitude; // detecting change of rotation from last fram I think I don't know if this works at all
        //jump
        if (Input.GetButtonDown("Jump") && controller.isGrounded && !(anim.GetCurrentAnimatorStateInfo(0).IsName("Jump") || anim.GetCurrentAnimatorStateInfo(0).IsName("JumpRun")) && deltaRotation <= 0.020)
        {
            moveDirection.y = jumpFloat;
            anim.SetBool("Jump Bool", true);
            moveDirection.y -= gravity * Time.deltaTime;
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
                }
                //it's true if the dog is still above the colliderY
                // This could definitely be the problem! If the curve is not synced up with the jump, the collider may for some reason always be too high?
                else if (anim.GetFloat("ColliderCurve") > colliderY)
                {
                    jumping = true;
                }
                //sets the animation to off
                // the jumping animation will always go, because if you go to the transition from jump to onGround state, "Has Exit Time" is checked
                // this code can be gotten rid of, because we don't always want to immediately go back to the original state now (which the following line does), we should also go 
                // to fall state if the situation arises
                anim.SetBool("Jump Bool", false);
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
        anim.SetInteger("Movement Int", 0); //change this to ai script later
        controller.center = new Vector3(controller.center.x, colliderY, controller.center.z);
        jumpBool = false;
    }
}

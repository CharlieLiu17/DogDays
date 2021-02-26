using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public Animator anim;

    public float colliderY;
    public bool jumpBool = false;
    public bool jumping = false;
    public float jumpFloat = 4f;
    public float jumpLag = 0.5f;
    public float speed = 6f;
    public float runSpeed = 12f;
    public int moveInt = 0;
    public float smoothTurnTime = 0.1f;
    public float gravity = 8f;
    public Vector3 moveDirection = new Vector3(0f, 0f, 0f);
    float turnSmoothVelocity;
    float finalSpeed = 1;

    public int count = 0;


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
                targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, smoothTurnTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                anim.SetInteger("Movement Int", moveInt);
            } else
            {
                anim.SetInteger("Movement Int", moveInt);
            }
        }

        float deltaRotation = (new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z) - temp).magnitude;
        //jump
        if (Input.GetButtonDown("Jump") && controller.isGrounded && !(anim.GetCurrentAnimatorStateInfo(0).IsName("Jump") || anim.GetCurrentAnimatorStateInfo(0).IsName("JumpRun")) && deltaRotation <= 0.020)
        {
            moveDirection.y = jumpFloat;
            anim.SetBool("Jump Bool", true);
            moveDirection.y -= gravity * Time.deltaTime;
            jumpBool = true;
        }
        
        else
        {
            if (jumpBool)
            {
                count++;
                if (!anim.IsInTransition(0))
                {
                    controller.center = new Vector3(controller.center.x, anim.GetFloat("ColliderCurve"), controller.center.z);
                }
                if (anim.GetFloat("ColliderCurve") <= colliderY && jumping == true)
                {
                    jumpBool = false;
                    jumping = false;
                }
                else if (anim.GetFloat("ColliderCurve") > colliderY)
                {
                    jumping = true;
                }

                anim.SetBool("Jump Bool", false);
                moveDirection.y -= anim.GetFloat("GravityCurve") * Time.deltaTime;
                controller.Move(moveDirection.normalized * finalSpeed * Time.deltaTime);
            }
            else
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

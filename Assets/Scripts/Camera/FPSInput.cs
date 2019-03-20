using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PrinceAction
{
    IDLE,
    WALKING,
    RUNNING,
    IDLE_JUMPING,
    JUMPING,
    LONG_JUMPING,
    CLIMBING,
    SITTING,
    FALLING,
    TURN,
    TURNING_180
}

[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Control Script/FPS Input")]

public class FPSInput : MonoBehaviour
{
    public PrinceAction princeAction;

    public float speed = 6.0f;
    private float jumpForceY = 55.0f;
    private float jumpForceX = 1.0f;
    private float gravityJump = 6.0f;
    private float yAxis = -1.0f;

    private bool isFacingRight = true;

    private CharacterController characterController;
    private Animator _animator;
    private Transform _transform;

    //timer
    private bool isMoving;
    private float m_Time_Running = 0f;
    private float m_Time_Idle = 0f;


    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _transform = GetComponent<Transform>();
        princeAction = PrinceAction.IDLE;
    }

    private void FixedUpdate()
    {
        if (characterController.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space) && princeAction == PrinceAction.IDLE)
            {
                yAxis = jumpForceY * Time.deltaTime;
                _animator.SetTrigger("Idle_Jumping");
            }
            else if (Input.GetKeyDown(KeyCode.Space) && princeAction == PrinceAction.RUNNING)
            {
                jumpForceX = 0.5f;
                yAxis = (jumpForceY + 2) * Time.deltaTime;
                _animator.SetTrigger("Jumping");
            }
            else
            {
                jumpForceX = 1f;
                yAxis = -1f;
            }
        }
        else
        {
            yAxis -= gravityJump * Time.deltaTime;
        }

        var dir = new Vector3(-Input.GetAxis("Horizontal") * jumpForceX, yAxis, 0f);
        characterController.Move(dir * speed * Time.deltaTime);

    }

    // Update is called once per frame
    void Update()
    {

        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0)
        {
            princeAction = PrinceAction.RUNNING;
        }
        else
        {
            princeAction = PrinceAction.IDLE;
        }

        switch (princeAction)
        {
            case PrinceAction.IDLE:
                Idle();
                break;

            case PrinceAction.RUNNING:
                Run();
                _animator.SetFloat("InputX", Input.GetAxis("Horizontal"));
                break;

            case PrinceAction.IDLE_JUMPING:

                break;

            case PrinceAction.JUMPING:
                break;

            case PrinceAction.LONG_JUMPING:

                break;

            case PrinceAction.CLIMBING:

                break;

            case PrinceAction.SITTING:

                break;

            case PrinceAction.FALLING:

                break;

            case PrinceAction.TURN:

                break;

            case PrinceAction.TURNING_180:

                break;

        }

        //timer
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0)
        {
            m_Time_Running += Time.deltaTime;
        }

        //timer
        if (Mathf.Abs(Input.GetAxis("Horizontal")) == 0)
        {
            m_Time_Idle += Time.deltaTime;
        }
    }

    private void Idle()
    {
        Vector3 direction = new Vector3(-Input.GetAxis("Horizontal"), yAxis, 0f);
        characterController.Move(direction * speed * Time.deltaTime);

        if (Input.GetAxis("Horizontal") > 0 && !isFacingRight)
        {
            Turn();
        }
        else if (Input.GetAxis("Horizontal") < 0 && isFacingRight)
        {
            Turn();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpIdle();
        }
    }

    private void JumpIdle()
    {
        princeAction = PrinceAction.IDLE_JUMPING;
    }

    private void Run()
    {
        Vector3 direction = new Vector3(-Input.GetAxis("Horizontal"), yAxis, 0f);
        characterController.Move(direction * speed * Time.deltaTime);

        if (Input.GetAxis("Horizontal") > 0 && !isFacingRight)
        {
            Turn();
        }
        else if (Input.GetAxis("Horizontal") < 0 && isFacingRight)
        {
            Turn();
        }

        princeAction = PrinceAction.RUNNING;
    }

    private void Turn()
    {

        if (m_Time_Idle > 3f)
        {
            m_Time_Running = 0;
        }

        if (m_Time_Running > 0.3f)
        {
            _animator.SetTrigger("Turn");
            m_Time_Running = 0;
        }

        m_Time_Idle = 0;

        isFacingRight = !isFacingRight;
        _animator.transform.Rotate(0, 180, 0);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Floor")
        {
            Debug.Log("DABUDI DABUDAI");
            //_animator.SetTrigger("Climb");
            //Vector3 climbPos = transform.position;
            //GetComponent<Rigidbody>().useGravity = false;
            var dir = new Vector3(0.2f, 5.44f, 0f);
            characterController.Move(dir);
        }
    }

}


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
    private float jumpForce = 45.0f;
    private float gravityJump = 6.0f;
    private float yAxis = -1.0f;

    private bool isFacingRight = true;

    private CharacterController characterController;
    private Animator _animator;
    private Transform _transform;

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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                yAxis = jumpForce * Time.deltaTime;
                _animator.SetTrigger("Jumping");
            }
            else
            {
                yAxis = -1f;
            }
        }
        else
        {
            yAxis -= gravityJump * Time.deltaTime;
        }

        var dir = new Vector3(-Input.GetAxis("Horizontal"), yAxis, 0f);
        characterController.Move(dir * speed * Time.deltaTime);

    }

    // Update is called once per frame
    void Update()
    {
        //var dir = new Vector3(-Input.GetAxis("Horizontal"), yAxis, 0f);
        //characterController.Move(dir * speed * Time.deltaTime);
        Debug.Log(yAxis + "LOH");

        //Vector3 direction = new Vector3(-Input.GetAxis("Horizontal"), yAxis, 0f);
        //characterController.Move(direction * speed * Time.deltaTime);

        //Debug.Log("LOH" + direction);

        ////if (Input.GetAxis("Horizontal") > 0 && !isFacingRight)
        ////{
        ////    Turn();
        ////}
        ////else if (Input.GetAxis("Horizontal") < 0 && isFacingRight)
        ////{
        ////    Turn();
        ////}

        //_animator.SetFloat("InputX", Input.GetAxis("Horizontal"));
        //_animator.SetFloat("InputY", Input.GetAxis("Vertical"));

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
        //if (princeAction == PrinceAction.IDLE)
        //{
        //    if (characterController.isGrounded)
        //    {
        //        if (Input.GetKeyDown(KeyCode.Space))
        //        {
        //            yAxis = jumpForce * Time.deltaTime;
        //            princeAction = PrinceAction.IDLE_JUMPING;
        //        }
        //        else
        //        {
        //            yAxis = -1f;
        //        }
        //    }
        //    else
        //    {
        //        yAxis -= gravityJump * Time.deltaTime;
        //    }

        //    var dir = new Vector3(-Input.GetAxis("Horizontal"), yAxis, 0f);
        //    characterController.Move(dir * speed * Time.deltaTime);

        //}

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
        isFacingRight = !isFacingRight;
        _animator.transform.Rotate(0, 180, 0);

        if (princeAction == PrinceAction.RUNNING)
        {
            _animator.SetTrigger("Turn");
        }

    }

    private void Turn180()
    {

    }
}


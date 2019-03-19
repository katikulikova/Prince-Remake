using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Control Script/FPS Input")]

public class FPSInput : MonoBehaviour
{
    public float speed = 6.0f;
    private float jumpForce = 80.0f;
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
    }

    private void FixedUpdate()
    {
        //if (characterController.isGrounded)
        //{
        //    if (Input.GetAxis("Vertical") > 0)
        //    {
        //        yAxis = jumpForce * Time.deltaTime;
        //    }
        //    else
        //    {
        //        yAxis = -1f;
        //    }
        //}
        //else
        //{
        //    yAxis -= gravityJump * Time.deltaTime;
        //}

        //var dir = new Vector3(-Input.GetAxis("Horizontal"), yAxis, 0f);
        //characterController.Move(dir * speed * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 direction = new Vector3(-Input.GetAxis("Horizontal"), yAxis, 0f);
        characterController.Move(direction * speed * Time.deltaTime);

        if (Input.GetAxis("Horizontal") > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (Input.GetAxis("Horizontal") < 0 && isFacingRight)
        {
            Flip();
        }

        _animator.SetFloat("InputX", Input.GetAxis("Horizontal"));
        _animator.SetFloat("InputY", Input.GetAxis("Vertical"));

        //Animator State
        //if (Input.GetAxis("Horizontal") != 0)
        //{
        //    _animator.SetInteger("State", 1);
        //}
        //else if (Input.GetAxis("Horizontal") == 0 && !characterController.isGrounded)
        //{
        //    _animator.SetInteger("State", 2);
        //    Debug.Log(_animator.GetInteger("State") + "LOH");
        //}
        //else
        //{
        //    _animator.SetInteger("State", 0);
        //}

    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        _animator.transform.Rotate(0, 180, 0);
        //_animator.SetTrigger("Turn");
        //_animator.SetInteger("State", 0);

        //Quaternion theRotation = _transform.localRotation;
        //theRotation.y *= -1;
        //_transform.localRotation = theRotation;
    }
}


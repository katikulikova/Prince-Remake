using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PrinceAction
{
    NONE,
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
    private PrinceAction m_princeAction = PrinceAction.NONE; // for logging of 1st IDLE, need .NONE
    // these public vars are for watching state of prince in Inspector during game debug
    public string dbg_princeAction = "";
    public bool dbg_IsGrounded = false;
    public float m_yAxis_VerticalSpeed = -1.0f;

    private PrinceAction princeAction
    {
        get
        {
            return m_princeAction;
        }
        set
        {
            if (m_princeAction != value)
            {
                dbg_princeAction = value.ToString();
                m_princeAction = value;
                // only log changes in Prince state
                Debug.Log("m_princeAction = " + value.ToString());
            }
        }
    }

    public float m_SecondsBeforeTurn180 = 0.3f;
    public float m_Turn180_AnimStart = 0.20f;
    public float m_SpeedDuringTurn180 = 1f;
    /// <summary>
    /// general multiplier for Input.GetAxis(..) for both Hor & Vertical speeds
    /// </summary>
    public float m_Speed_Multiplier = 6.0f;

    private float c_JumpSpeedY = 2f; //55.0f;
    private float c_gravityJump = 6.0f;


    private bool isFacingRight = true;

    private CharacterController characterController;
    private Animator _animator;
    private Transform _transform;

    private float m_Time_Running = 0f;
    private float m_Time_Idle = 0f;

    public bool ableToClimb;
    public bool isClimbing;

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
    private Int64 m_InputTick = 1000;

    // these variables are set in FixedUpdate. Discrete is for strict state control. Analog is for precise Speed and for Blend Trees in animator.
    private float m_DiscreteInput_X = 0;
    private float m_AnalogInput_X = 0;

    private void FixedUpdate()
    {
        bool thisFrame = false;

        float loc_jumpSpeedMultiplier = 1.0f;
        float loc_DeltaTime = Time.fixedDeltaTime;

        // NEVER USE Input.GetAxis, this is smooth-filtered value from GetAxisRaw
        // Don't use Input.GetAxis in outside code, use m_DiscreteInput_*/m_AnalogInput_*
        m_AnalogInput_X = Input.GetAxisRaw("Horizontal");



        m_InputTick++;
        #region BEGIN RECALCULATE princeAction based on Input/Time elapsed
        if (princeAction == PrinceAction.TURNING_180)
        {
            var loc_Animator_State_Info = _animator.GetCurrentAnimatorStateInfo(0);
            bool loc_Is_180_Turn_Being_Done = loc_Animator_State_Info.IsName("180");
            // Debug.Log("Current State is TURNING_180. Is Animator State 180? " + loc_Is_180_Turn_Being_Done.ToString());
            if (!loc_Is_180_Turn_Being_Done)
            {
                _animator.transform.Rotate(0, 180, 0);
                // finished => go to default state
                princeAction = PrinceAction.IDLE;
            }
            else
            {
                // OVERRIDE Input
                // give a bit slower speed in same direction, TODO: ideally it should be descreasing speed
                m_AnalogInput_X = (isFacingRight ? -1 : 1) * m_SpeedDuringTurn180;
            }
        }
        m_DiscreteInput_X = Math.Abs(m_AnalogInput_X) > 0 ? Math.Sign(m_AnalogInput_X) : 0;

        if (princeAction == PrinceAction.RUNNING || princeAction == PrinceAction.IDLE)
        {
            if (Mathf.Abs(m_DiscreteInput_X) > 0)
            {
                princeAction = PrinceAction.RUNNING;
            }
            else
            {
                princeAction = PrinceAction.IDLE;
            }
        }

        #endregion
        //Debug.Log(m_InputTick.ToString() + " Input.Horizontal A,D = " + m_AnalogInput_X.ToString() + ", " + m_DiscreteInput_X.ToString());

        // to get correct value of .isGrounded in FixedUpdate, we must issue .Move, see .isGrounded documentation

        if (isClimbing)
        {
            _animator.applyRootMotion = true;
            characterController.enabled = false;
        }
        else
        {
            characterController.SimpleMove(new Vector3(0, 0, 0));
            dbg_IsGrounded = characterController.isGrounded;
        }

        if (characterController.isGrounded)
        {
            if (Input.GetKey(KeyCode.Space) && princeAction == PrinceAction.IDLE)
            {
                if (ableToClimb && !isClimbing)
                {
                    isClimbing = true;
                    thisFrame = true;
                    Logged_SetTrigger("Climb", " Check if repeats twice");
                    //_animator.SetTrigger("Climb");
                }
                else
                {
                    m_yAxis_VerticalSpeed = c_JumpSpeedY;
                    Logged_SetTrigger("Idle_Jumping", "Space in .IDLE");
                }
            }
            else if (Input.GetKey(KeyCode.Space) && princeAction == PrinceAction.RUNNING)
            {
                loc_jumpSpeedMultiplier = 1.5f;
                m_yAxis_VerticalSpeed = c_JumpSpeedY;
                Logged_SetTrigger("Jumping", "Space in .RUNNING");
            }
            else
            {
                m_yAxis_VerticalSpeed = -1f;
            }
        }
        else
        {
            m_yAxis_VerticalSpeed -= c_gravityJump * loc_DeltaTime;
        }
        float loc_xAxis_HorizontalSpeed = -m_AnalogInput_X;

        m_Speeds_For_Move_in_Update = new Vector3(loc_xAxis_HorizontalSpeed * loc_jumpSpeedMultiplier, m_yAxis_VerticalSpeed, 0f);

        if (isClimbing == true)
        {
            if (thisFrame == false && _animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                isClimbing = false;
            }
        }

    }
    public Vector3 m_Speeds_For_Move_in_Update = new Vector3(0, 0, 0);

    /// <summary>
    /// called from Idle/Running, moves char according to m_speed
    /// </summary>
    private void UpdateOneTick_Turn_If_Needed()
    {
        if (m_DiscreteInput_X > 0 && !isFacingRight)
        {
            Turn();
        }
        else if (m_DiscreteInput_X < 0 && isFacingRight)
        {
            Turn();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // MAIN RULE: FixedUpdate() changes state variables. Update() only reflects them in UI,Animator,etc for smooth movement
        // FixedUpdate is "smart" place. Update is "smooth game" place.

        // do .Move in Update() for smooth visuals
        if (isClimbing)
        {
            _animator.applyRootMotion = true;
            characterController.enabled = false;
        }
        else
        {
            characterController.enabled = true;
            _animator.applyRootMotion = false;
            var loc_DeltaTime = Time.deltaTime;
            characterController.Move(m_Speeds_For_Move_in_Update * m_Speed_Multiplier * loc_DeltaTime);
        }

        // our .IDLE and .RUNNING states correspond to Blend-tree "Idle" state of Animator
        // so keep Animator informed
        _animator.SetFloat("InputX", m_AnalogInput_X);

        switch (princeAction)
        {
            case PrinceAction.IDLE:
                UpdateOneTick_Turn_If_Needed();
                break;

            case PrinceAction.RUNNING:
                UpdateOneTick_Turn_If_Needed();
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

        if (princeAction == PrinceAction.RUNNING)
        {
            m_Time_Idle = 0;
            Logged_SetTimeRunning(m_Time_Running + Time.deltaTime, "Update");
        }

        if (princeAction == PrinceAction.IDLE)
        {
            m_Time_Idle += Time.deltaTime;
            // Standing X seconds => prince was stopped, clear running-time to avoid 180-degree turn animation
            if (m_Time_Idle > 0.2f)
            {
                Logged_SetTimeRunning(0, "Idle_X_sec");
            }
        }
    }

    private void Logged_SetTrigger(string triggerName, string _Reason)
    {
        Debug.Log("Set Trigger " + triggerName + " by " + _Reason);
        _animator.SetTrigger(triggerName);
    }
    private void Logged_SetTimeRunning(float newTimeRunning, string _Reason)
    {
        if (m_Time_Running != newTimeRunning)
        {
            m_Time_Running = newTimeRunning;
            // Debug.Log("m_Time_Running = " + newTimeRunning.ToString() + " by " + _Reason);
        }
    }


    private void Turn()
    {
        m_Time_Idle = 0;

        // Running for X seconds => run 180-degree animation. Why Trigger called not Turn180???
        if (m_Time_Running > m_SecondsBeforeTurn180)
        {
            //Logged_SetTrigger("Turn180", "Turn after running some time");
            _animator.Play("180", 0, m_Turn180_AnimStart);
            princeAction = PrinceAction.TURNING_180;
            isFacingRight = !isFacingRight;
            return;
        }
        Logged_SetTimeRunning(0, "Any Turn");


        isFacingRight = !isFacingRight;
        _animator.transform.Rotate(0, 180, 0);

    }

    private void Climb()
    {
        _animator.applyRootMotion = true;
        characterController.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Floor")
        {
            Debug.Log("OnTriggerEnter other.Tag = Floor");

            //Logged_SetTrigger("Climb", "Floor Touched");
            //Vector3 climbPos = transform.position;
            //GetComponent<Rigidbody>().useGravity = false;
            //var dir = new Vector3(0.2f, 5.44f, 0f);
            //characterController.Move(dir);
        }
    }

}
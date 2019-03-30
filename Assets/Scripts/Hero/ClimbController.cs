using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbController : MonoBehaviour
{
    public GameObject prince;
    private FPSInput fpsInput;
    private Transform _transform;

    // Start is called before the first frame update
    void Start()
    {
        fpsInput = prince.GetComponent<FPSInput>();
        _transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {

        if (other.GetComponent<ClimbCheckController>() != null)
        {
            if (other.GetComponent<ClimbCheckController>().doorState == DoorState.CLOSED || other.GetComponent<ClimbCheckController>().checkerState == false)
            {
                fpsInput.ableToClimb = false;
            }
            else
            {
                fpsInput.ableToClimb = true;
            }
        }
        //else if (_transform.parent.tag == "Door")
        //{
        //    if (_transform.parent.gameObject.GetComponent<Door>().doorState == DoorState.CLOSED)
        //    {
        //        fpsInput.ableToClimb = false;
        //    }
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<ClimbCheckController>() != null)
        {
            fpsInput.ableToClimb = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbController : MonoBehaviour
{
    public GameObject prince;
    private FPSInput fpsInput;

    // Start is called before the first frame update
    void Start()
    {
        fpsInput = prince.GetComponent<FPSInput>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "ClimbCheck")
        {
            fpsInput.ableToClimb = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "ClimbCheck")
        {
            fpsInput.ableToClimb = false;
        }
    }
}

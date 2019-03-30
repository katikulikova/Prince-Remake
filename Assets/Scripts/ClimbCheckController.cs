using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbCheckController : MonoBehaviour
{
    public DoorState doorState;
    public bool checkerState;
    // Update is called once per frame

    private void Start()
    {
        checkerState = true;
    }

    void Update()
    {
        if (transform.GetComponentInChildren<DoorStateGetter>() != null)
        {
            doorState = transform.GetComponentInChildren<DoorStateGetter>().currentDoorState;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorClimbController : MonoBehaviour
{
    private Transform climbChecker;
    private Door _door;

    // Start is called before the first frame update
    void Start()
    {
        if (transform.parent.parent != null)
        {
            if (transform.parent.parent.tag == "ClimbCheker")
            {
                climbChecker = transform.parent.parent;
            }
        }

        _door = GetComponent<Door>();

    }

    // Update is called once per frame
    void Update()
    {
        if (climbChecker != null)
        {
            Debug.Log(climbChecker.childCount);
        
            if (_door.doorState == DoorState.CLOSED)
            {
                climbChecker.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                climbChecker.GetChild(0).gameObject.SetActive(true);
            }
        }
    }
}

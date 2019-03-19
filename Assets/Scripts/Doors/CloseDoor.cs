using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDoor : MonoBehaviour
{
    public GameObject doorObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            doorObject.GetComponent<Door>().Close(5);
        }
    }
}

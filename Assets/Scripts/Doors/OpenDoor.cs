using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public GameObject doorObject;

    private void OnTriggerEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            doorObject.GetComponent<Door>().Open();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            doorObject.GetComponent<Door>().Open();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(Close());
            doorObject.GetComponent<Door>().Open();
        }
    }

    IEnumerator Close()
    {
        yield return new WaitForSeconds(5);
        doorObject.GetComponent<Door>().Close();
    }
}

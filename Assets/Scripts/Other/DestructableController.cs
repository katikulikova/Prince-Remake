using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableController : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("KEK");
        if (other.tag == "Player")
        {
            Debug.Log("LOH");
            Destroy(gameObject, 1.0f);
        }
    }
}

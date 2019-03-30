using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableController : MonoBehaviour
{
    public ClimbCheckController[] climbCheckers;

    private void Start()
    {
        if (climbCheckers != null)
        {
            foreach (var c in climbCheckers)
            {
                c.GetComponent<ClimbCheckController>().checkerState = false;
            }
        }
    }

    private void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("KEK");
        if (other.tag == "Player")
        {
            Debug.Log("LOH");
            Destroy(gameObject, 1.0f);
        }
    }

    private void OnDestroy()
    {
        if (climbCheckers != null)
        {
            foreach (var c in climbCheckers)
            {
                c.GetComponent<ClimbCheckController>().checkerState = true;
            }
        }
    }
}

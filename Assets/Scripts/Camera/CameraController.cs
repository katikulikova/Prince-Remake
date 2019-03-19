using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform target;
    private Transform bounds;

    private float minX;
    private float maxX;

    private float minY;
    private float maxY;

    private Camera camera;

    public float speed = 5.0f;


    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        bounds = GameObject.FindGameObjectWithTag("Bounds").transform;
        
        minX = bounds.position.x - (bounds.transform.localScale.x / 2) + 7;
        maxX = bounds.position.x + (bounds.transform.localScale.x / 2) - 7;

        minY = bounds.position.y - (bounds.transform.localScale.y / 2) + 3.7f;
        maxY = bounds.position.y + (bounds.transform.localScale.y / 2) - 3.7f;
    }

    // Update is called once per frame
    void Update()
    {


    }

    private void LateUpdate()
    {
        Vector3 targetPos = target.position;

        if (target.position.x < minX)
        {
            Move(minX, targetPos.y);
        }
        else if (targetPos.y > maxX)
        {
            Move(maxX, targetPos.y);
        }
        else if (targetPos.y < minY)
        {
            Move(targetPos.x, minY);
        }
        else if (targetPos.y > maxY)
        {
            Move(targetPos.x, maxY);
        }
        else
        {
            Move(targetPos.x, targetPos.y);
        }
    }

    private void Move(float newX, float newY)
    {
        Vector3 newPos = new Vector3(newX, newY, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, newPos, speed * Time.deltaTime);
    }
}

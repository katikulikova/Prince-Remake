using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public enum RotationAxes
    {
        MouseXandY = 0,
        MouseX= 1,
        MouseY = 2
    }

    public RotationAxes axes = RotationAxes.MouseXandY;

    public float sensitivityHor = 6.0f;
    public float sensitivityVert = 6.0f;

    public float minimumVert = -45.0f;
    public float maximumVert = 45.0f;

    public float _rotationZ = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        if (rigidbody != null)
            rigidbody.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (axes == RotationAxes.MouseX)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityHor, 0);
        }
        else if (axes == RotationAxes.MouseY)
        {
            _rotationZ -= Input.GetAxis("Mouse Y") * sensitivityVert;
            _rotationZ = Mathf.Clamp(_rotationZ, minimumVert, maximumVert);

            float rotationY = transform.localEulerAngles.y;
            transform.localEulerAngles = new Vector3(_rotationZ, rotationY, 0);
        } else 
        {
            _rotationZ -= Input.GetAxis("Mouse Y") * sensitivityVert;
            _rotationZ = Mathf.Clamp(_rotationZ, minimumVert, maximumVert);

            float delta = Input.GetAxis("Mouse X") * sensitivityHor;
            float rotationY = transform.localEulerAngles.y + delta;

            transform.localEulerAngles = new Vector3(0, rotationY, _rotationZ);

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouseLook : MonoBehaviour
{

    public float cameraSensitivityVert = 6.0f;

    public float cameraMinimumVert = -45.0f;
    public float cameraMaximumVert = 45.0f;
     
    public float _cameraRotationZ = 0f;


    // Update is called once per frame
    void Update()
    {

            _cameraRotationZ -= Input.GetAxis("Mouse Y") * cameraSensitivityVert;
            _cameraRotationZ = Mathf.Clamp(_cameraRotationZ, cameraMinimumVert, cameraMaximumVert);

            float cameraRotationY = transform.localEulerAngles.y;
            transform.localEulerAngles = new Vector3(_cameraRotationZ, cameraRotationY, 0);
       
        }
    }


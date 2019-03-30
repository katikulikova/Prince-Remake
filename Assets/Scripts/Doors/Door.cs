using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorState
{
    OPENED,
    CLOSED,
    OPENNING,
    CLOSING
}

public class Door : MonoBehaviour
{
    public DoorState doorState;
    public FPSInput fPSInput;
    public float doorHeight;
    private float speed = 2;
    float visibleDoorPart;

    public void Open(float _speed = 2)
    {
        if (doorState == DoorState.OPENED || doorState == DoorState.OPENNING)
            return;

        if (doorState == DoorState.CLOSED)
            visibleDoorPart = doorHeight;

        doorState = DoorState.OPENNING;
        speed = _speed;
    }

    public void Close(float _speed = 2)
    {
        if (doorState == DoorState.CLOSED || doorState == DoorState.CLOSING)
            return;

        if (doorState == DoorState.OPENED)
            visibleDoorPart = 0;

        doorState = DoorState.CLOSING;
        speed = _speed;
    }

    void Update()
    {
        if (Input.GetKeyDown("o"))
            Open();

        if (Input.GetKeyDown("c"))
            Close();

        Vector3 newPos;
        float moveBy;

        switch (doorState)
        {
            case DoorState.OPENNING:

                moveBy = Time.deltaTime * speed;

                if (visibleDoorPart - moveBy <= 0)
                {
                    doorState = DoorState.OPENED;
                    moveBy = visibleDoorPart;
                }
                else
                {
                    visibleDoorPart -= moveBy;
                }

                newPos = transform.localPosition;
                newPos.y += moveBy;

                transform.localPosition = newPos;

                break;

            case DoorState.CLOSING:

                moveBy = Time.deltaTime * speed;

                if (visibleDoorPart + moveBy >= doorHeight)
                {
                    doorState = DoorState.CLOSED;
                    moveBy = doorHeight - visibleDoorPart;
                }
                else
                {
                    visibleDoorPart += moveBy;
                }

                newPos = transform.localPosition;
                newPos.y -= moveBy;

                transform.localPosition = newPos;

                break;

            case DoorState.CLOSED:

                break;
        }
    }
}

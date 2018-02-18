using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private const float Y_ANGLE_MIN = 2.0f;
    private const float Y_ANGLE_MAX = 80.0f;

    private const float X_ANGLE_MIN = 2.0f;
    private const float X_ANGLE_MAX = 80.0f;


    public Transform lookAt;
    public Transform camTransform;

    private Camera cam;

    private float distance = 10.0f;
    private float currentX = 0.0f;
    private float currentY = -60.0f;
    private float sensivityX = 4.0f;
    private float sensivityY = 4.0f;

    Vector3 gestureStartPosition;
    Vector3 gestureDelta;

    private void Start()
    {
        camTransform = transform;
        cam = Camera.main;
    }

    private void Update()
    {
        //Debug.Log(Application.platform);
        //if (Application.platform == RuntimePlatform.Android)
        {
            TouchScreen();
        }
        //else
        {
            //Mouse();
        }          

        currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);

    }

    private void LateUpdate()
    {
        Vector3 dir = new Vector3(0, 0 + distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX*sensivityX, 0 );
        camTransform.position = lookAt.position + rotation * dir;
        camTransform.LookAt(lookAt.position);
    }

    private void TouchScreen()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gestureStartPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            gestureDelta = Input.mousePosition - gestureStartPosition;
            gestureDelta /= Screen.dpi;
        }
        else
        {
            gestureDelta = Vector3.zero;
        }

        currentX += gestureDelta.x;
        currentY += gestureDelta.y;

        //Debug.Log("GestureDelta: "+gestureDelta+" X: "+currentX+"Y: "+currentY);
    }

    private void Mouse()
    {
        currentX += Input.GetAxis("Mouse X");
        currentY += Input.GetAxis("Mouse Y");
    }
}

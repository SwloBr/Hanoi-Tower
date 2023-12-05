using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Camera camera;
    public float cameraAngleY = 0f;
    public float cameraAngleX = 0f;
    public float cameraRotSpeed = Mathf.PI;
    public float cameraX;
    public float cameraZ;
    public float cameraRadius = -11f;
    public Transform cameraTransform;
    public Vector3 cameraEulerAngles;
    public Vector2 cameraOrigin = new Vector2(5, 0);


    void Start()
    {
        {
            camera = Camera.main;
            cameraTransform = camera.transform;
            cameraEulerAngles = cameraTransform.eulerAngles;
        }
    }

    void Update()
    {
        // When mouse button wheel is pressed, the camera will rotate in .
        if (Input.GetAxis("Mouse X") != 0f && Input.GetMouseButton(2))
        {
            float YAxisSpeed = Input.GetAxis("Mouse X");
            cameraAngleY -= YAxisSpeed * cameraRotSpeed;
        }

        if (Input.GetAxis("Mouse Y") != 0f && Input.GetMouseButton(2))
        {
            float XAxisSpeed = Input.GetAxis("Mouse Y");
            cameraAngleX -= XAxisSpeed * cameraRotSpeed;
        }


        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            cameraRadius += 2 * Input.GetAxis("Mouse ScrollWheel");
            cameraRadius = Mathf.Clamp(cameraRadius, -15f, -5f);
        }

        cameraEulerAngles = new Vector3(cameraAngleX, cameraAngleY, cameraEulerAngles.z);
        cameraTransform.eulerAngles = cameraEulerAngles;

        // When angle y rotate more than 360 or less than -360, it will reset to 0. it prevent the int limit.
        if (Mathf.Abs(cameraAngleY) > 360)
        {
            cameraAngleY = 0;
        }

        /// <summary>
        /// this is the camera movement
        /// </summary>
        /// <param name="cameraX">Position X from cam</param>
        /// <param name="cameraZ">Position Z from cam</param>
        /// <param name="cameraZ">Position Z from cam</param>
        cameraX = cameraOrigin.x + cameraRadius * Mathf.Sin(cameraAngleY * Mathf.Deg2Rad);
        cameraZ = cameraOrigin.y + cameraRadius * Mathf.Cos(cameraAngleY * Mathf.Deg2Rad);
        cameraTransform.position = new Vector3(cameraX, cameraTransform.position.y, cameraZ);
    }
        
    
}
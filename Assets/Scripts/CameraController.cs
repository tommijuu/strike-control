using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 pos;
    private Vector3 rot;
    private Vector3 zoom;
    public Vector3 zoomAmount;
    private float shiftModifier = 1;
    private float moveSpeed = 30f;
    private float edgeOffSet = 30f;
    private bool toggleEdgeScrolling = true;

    private Vector3 startingCameraPosition;
    private Vector3 startingZoom;

    public LayerMask layerMask;
    public GameObject audioListener;

    private bool cameraGameOver;

    public float EdgeOffSet { get { return edgeOffSet; } set { edgeOffSet = value; } }
    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }

    // Start is called before the first frame update
    void Start()
    {
        startingCameraPosition = transform.position;
        startingZoom = Camera.main.transform.localPosition;
        pos = transform.position;
        zoom = Camera.main.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            audioListener.transform.position = hit.point;
        }
        

        if (Input.GetMouseButtonDown(2)) //middle mouse button
        {
            if (toggleEdgeScrolling)
            {
                toggleEdgeScrolling = false;
            } else
            {
                toggleEdgeScrolling = true;
            }
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            shiftModifier = 3f;
        }
        else
        {
            shiftModifier = 1f;
        }

        if (!GameManager.instance.paused && !cameraGameOver)
        {
            if (Input.GetKeyUp(KeyCode.H))
            {
                pos.x = startingCameraPosition.x;
                pos.z = startingCameraPosition.z;

            }
            else if (Input.GetKeyUp(KeyCode.J))
            {
                zoom.z = startingZoom.z;
            }
            else
            {

                if (Input.GetAxisRaw("Vertical") != 0)
                {
                    pos.x += (Input.GetAxisRaw("Vertical")) * moveSpeed * shiftModifier * Time.deltaTime;
                }

                if (Input.GetAxisRaw("Horizontal") != 0)
                {
                    pos.z += (Input.GetAxisRaw("Horizontal")) * -moveSpeed * shiftModifier * Time.deltaTime;
                }

                if (Input.mouseScrollDelta.y > 0 && Camera.main.transform.localPosition.z < 20f)
                {
                    zoom += zoomAmount * shiftModifier;
                }

                if (Input.mouseScrollDelta.y < 0 && Camera.main.transform.localPosition.z > -40f)
                {
                    zoom -= zoomAmount * shiftModifier;
                }


                if (toggleEdgeScrolling)
                {
                    if (Input.mousePosition.x > Screen.width - edgeOffSet)
                    {
                        pos.z += -moveSpeed * shiftModifier * Time.deltaTime;
                    }
                    else if (Input.mousePosition.x < edgeOffSet)
                    {
                        pos.z += moveSpeed * shiftModifier * Time.deltaTime;
                    }

                    if (Input.mousePosition.y > Screen.height - edgeOffSet)
                    {
                        pos.x += moveSpeed * shiftModifier * Time.deltaTime;
                    }
                    else if (Input.mousePosition.y < edgeOffSet)
                    {
                        pos.x += -moveSpeed * shiftModifier * Time.deltaTime;
                    }
                }

                
            }
        }

        transform.position = pos;
        Camera.main.transform.localPosition = zoom;


    }

    public void CameraGameOver()
    {
        pos.x = startingCameraPosition.x;
        pos.z = startingCameraPosition.z;
        zoom.z = startingZoom.z;
        cameraGameOver = true;
    }


    
}


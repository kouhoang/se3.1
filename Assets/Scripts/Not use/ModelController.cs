//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class ModelController : MonoBehaviour
{
    [SerializeField]
    public float rotationSpeed = 2.0f;
    [SerializeField]
    public float panSpeed = 2.0f;
    [SerializeField]
    public float zoomSpeed = 2.0f;

    private Vector2 lastPanPosition;
    private int panFingerId; // Touch ID for panning

    void Update()
    {
        HandleTouchInput();
    }

    void HandleTouchInput()
    {
        int touchCount = Input.touchCount;

        // Handle single touch to rotate
        if (touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Moved:
                    // Rotate the model based on touch delta position
                    float rotationX = touch.deltaPosition.y * rotationSpeed * Time.deltaTime;
                    float rotationY = -touch.deltaPosition.x * rotationSpeed * Time.deltaTime;
                    transform.Rotate(rotationX, rotationY, 0);
                    break;
            }
        }

        // Handle two touches to pan and zoom
        if (touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            // Check for pinch zoom
            if (touch1.phase == TouchPhase.Moved && touch2.phase == TouchPhase.Moved)
            {
                float pinchDelta = Vector2.Distance(touch1.position, touch2.position) -
                                   Vector2.Distance(touch1.position - touch1.deltaPosition,
                                                   touch2.position - touch2.deltaPosition);
                ZoomModel(pinchDelta * zoomSpeed * Time.deltaTime);
            }

            // Check for pan
            if (touch1.phase == TouchPhase.Moved && touch2.phase == TouchPhase.Stationary)
            {
                PanModel(touch1.deltaPosition);
            }

            // Store last pan position for delta calculation
            if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
            {
                lastPanPosition = 0.5f * (touch1.position + touch2.position);
                panFingerId = touch1.fingerId;
            }
        }
    }

    void PanModel(Vector2 deltaPan)
    {
        // Pan the model based on touch delta position
        Vector3 pan = new Vector3(-deltaPan.x, -deltaPan.y, 0) * panSpeed * Time.deltaTime;
        transform.Translate(pan, Space.Self);
    }

    void ZoomModel(float deltaZoom)
    {
        // Zoom the model based on pinch delta
        Vector3 zoom = new Vector3(0, 0, deltaZoom);
        transform.Translate(zoom, Space.Self);
    }
}
using UnityEngine;

public class Player : MonoBehaviour
{
    public int Wood { get; private set; }
    public int Clay { get; private set; }
    public int Stone { get; private set; }
    public float moveSpeed = 10f;
    public float edgeScrollSpeed = 10f;
    public float edgeTreshold = 10f;
    public float rotationSpeed = 100f;
    public float zoomSpeed = 10f;
    public float minZoom = 5f;
    public float maxZoom = 25f;
    public float minHeight = 4f; 
    public float maxHeight = 30f; 


    private Vector3 dragOrigin;

    private void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleZoom();
    }

    private void HandleMovement()
    {
        Vector3 moveDirection = Vector3.zero;

        //WASD movement
        if (Input.GetKey(KeyCode.W)) { moveDirection += transform.forward; }
        if (Input.GetKey(KeyCode.S)) { moveDirection -= transform.forward; }
        if (Input.GetKey(KeyCode.D)) { moveDirection += transform.right; }
        if (Input.GetKey(KeyCode.A)) { moveDirection -= transform.right; }

        float detectedFloorHeight = GetFloorHeight();
        float currentHeight = transform.position.y;

        // Up and Down
        if (Input.GetKey(KeyCode.Space) && currentHeight < maxHeight)
        { 
            moveDirection += transform.up;
        }
        if (Input.GetKey(KeyCode.LeftControl) && currentHeight > detectedFloorHeight + minHeight) // Adding a small buffer
        {
            moveDirection -= transform.up;
        }

        transform.position += moveDirection.normalized * moveSpeed * Time.deltaTime;

        //Panning with middle mouse button
        if (Input.GetMouseButtonDown(2)) { dragOrigin = Input.mousePosition; }
        if (Input.GetMouseButtonDown(2))
        {
            Vector3 difference = Camera.main.ScreenToViewportPoint(dragOrigin - Input.mousePosition);
            transform.position += new Vector3(difference.x * moveSpeed, 0, difference.y * moveSpeed);
            dragOrigin = Input.mousePosition;
        }
    }

    private void HandleRotation()
    {
        if (Input.GetMouseButtonDown(1))
        { 
            float rotation = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            transform.Rotate(Vector3.up, rotation, Space.World);
        }
        if (Input.GetKey(KeyCode.Q)) { transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World); }
        if (Input.GetKey(KeyCode.E)) { transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime, Space.World); }
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - scroll, minZoom, maxZoom);
    }

    private float GetFloorHeight()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity))
        {
            return hit.point.y; // Returns the height of the detected floor
        }
        return 0f; // Default floor height if nothing is detected
    }
}

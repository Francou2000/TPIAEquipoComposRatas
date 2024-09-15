using UnityEngine;

public class Door : MonoBehaviour
{
    public float openRotationAngle = 90f;   // The angle to rotate the door when opened
    public float rotationSpeed = 2f;        // How fast the door rotates
    public Vector3 rotationAxis = Vector3.up; // The axis on which the door rotates (default is Y axis)

    private bool isOpen = false;            // Keeps track of whether the door is open or closed
    private Quaternion closedRotation;      // The original rotation of the door
    private Quaternion openRotation;        // The target rotation when the door is open
    private bool isMoving = false;          // Flag to check if the door is currently rotating

    void Start()
    {
        // Store the door's original rotation
        closedRotation = transform.rotation;

        // Calculate the target open rotation based on the open angle and rotation axis
        openRotation = Quaternion.Euler(rotationAxis * openRotationAngle) * closedRotation;
    }

    // Call this method via the Interactable Unity Event to open/close the door
    public void Interact()
    {
        if (!isMoving)
        {
            isOpen = !isOpen;  // Toggle the door state
            isMoving = true;   // Start rotating the door
        }
    }

    void Update()
    {
        if (isMoving)
        {
            // Rotate towards the target rotation (either open or closed)
            Quaternion targetRotation = isOpen ? openRotation : closedRotation;

            // Smoothly rotate the door
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            // Check if the door has reached the target rotation
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                transform.rotation = targetRotation;  // Snap to the target rotation
                isMoving = false;                    // Stop rotating
            }
        }
    }
}

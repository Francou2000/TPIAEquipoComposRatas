using UnityEngine;

public class Door : MonoBehaviour
{
    public float openRotationAngle = 90f;   
    public float rotationSpeed = 2f;       
    public Vector3 rotationAxis = Vector3.up; 

    private bool isOpen = false;           
    private Quaternion closedRotation;      
    private Quaternion openRotation;        
    private bool isMoving = false;          

    void Start()
    {
         closedRotation = transform.rotation;

         openRotation = Quaternion.Euler(rotationAxis * openRotationAngle) * closedRotation;
    }

    public void Interact()
    {
        if (!isMoving)
        {
            isOpen = !isOpen;  
            isMoving = true;   
        }
    }

    void Update()
    {
        if (isMoving)
        {
            Quaternion targetRotation = isOpen ? openRotation : closedRotation;

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                transform.rotation = targetRotation;  
                isMoving = false;                  
            }
        }
    }
}

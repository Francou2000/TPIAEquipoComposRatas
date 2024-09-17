using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Door : MonoBehaviour
{
    public float _openRotationAngle = 90f;   
    public float _rotationSpeed = 2f;       
    public Vector3 _rotationAxis = Vector3.up;

    public AudioSource _audioSource;        
    public AudioClip _openSound;            
    public AudioClip _closeSound;

    private bool _isOpen = false;           
    private Quaternion _closedRotation;      
    private Quaternion _openRotation;        
    private bool _isMoving = false;          

    void Start()
    {
         _closedRotation = transform.rotation;

         _openRotation = Quaternion.Euler(_rotationAxis * _openRotationAngle) * _closedRotation;
    }

    public void Interact()
    {
        if (!_isMoving)
        {
            _isOpen = !_isOpen;
            _isMoving = true;

            if (_isOpen && _openSound != null)
            {
                _audioSource.PlayOneShot(_openSound);
            }
            else if (!_isOpen && _closeSound != null)
            {
                _audioSource.PlayOneShot(_closeSound);
            }
        }
    }

    void Update()
    {
        if (_isMoving)
        {
            Quaternion targetRotation = _isOpen ? _openRotation : _closedRotation;

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);

            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                transform.rotation = targetRotation;  
                _isMoving = false;                  
            }
        }
    }
}

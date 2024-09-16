using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootableItem : MonoBehaviour
{
    private Transform _player; 
    public float _rotationSpeed = 30f;  
    public Vector3 _offset = new Vector3(0, 1, 0);  

    private void Start()
    {
        GameObject playerReference = GameObject.FindWithTag("Player");
        if (playerReference != null)
        {
            _player = playerReference.transform;
        }

        transform.rotation = Quaternion.Euler(-90, 0, 0);
    }

    private void Update()
    {
        transform.Rotate(0, 0, _rotationSpeed * Time.deltaTime);

        if (_player != null)
        {
            transform.position = _player.position + _offset;
        }
    }
}

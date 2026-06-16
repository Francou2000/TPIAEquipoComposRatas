using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultySetter : MonoBehaviour
{
    void Start()
    {
        if (DifficultyManager.Instance)
        {
            DifficultyManager.Instance.ApplyDifficulty();
        }
        //Destroy(gameObject);
        Debug.Log("Dificultad seteada");
    }
}

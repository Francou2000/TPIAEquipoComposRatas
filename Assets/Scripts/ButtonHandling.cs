using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))] 
public class ButtonHandling : MonoBehaviour
{
    private Button myButton;
    public DifficultyManager.Difficulty _difficultyToSet;

    private void Awake()
    {
        myButton = GetComponent<Button>();
        myButton.onClick.AddListener(HandleClick);
    }

    private void HandleClick()
    {
        DifficultyManager.Instance.SetDifficulty(_difficultyToSet);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManagement : MonoBehaviour
{
  public DropZone dropZone;
  
  public void Play()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
  }
  

  public void Victory()
  {
    if (dropZone != null && dropZone._playerPoints >= 100) 
    {
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }  
  }

  public void Defeat()
  {
    GameObject playerRef = GameObject.FindWithTag("Player");
    if (playerRef == null)
    {
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);
    }
  }


  public void Quit()
  {
    Application.Quit();
  }
}

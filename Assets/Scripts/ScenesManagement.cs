using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManagement : MonoBehaviour
{
  
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        AudioManager._instance.RefreshAudioSources();
    }

    public void Quit()
    {
        Application.Quit();
    }
}

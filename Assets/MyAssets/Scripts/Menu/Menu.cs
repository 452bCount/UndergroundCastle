using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Method to load a new scene by name
    public void LoadSceneName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Method to load a new scene by index
    public void LoadSceneInt(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    // Method to quit the application
    public void QuitGame()
    {
        Application.Quit();
    }

}

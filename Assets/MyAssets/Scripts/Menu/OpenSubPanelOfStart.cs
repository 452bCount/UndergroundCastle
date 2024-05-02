using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenSubPanelOfStart : MonoBehaviour
{
    public GameObject SubPanelOfStart;
    public void OpenSubPanel()
    {
        SubPanelOfStart.SetActive(true);
    }

    public void LoadSceneInt(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}

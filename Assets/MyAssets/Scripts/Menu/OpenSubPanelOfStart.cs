using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenSubPanelOfStart : MonoBehaviour
{
    public GameObject SubPanelOfStart;
    public void OpenSubPanel()
    {
        SubPanelOfStart.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance { get; private set; }

    private void Awake()
    {
        // Set Parent to Root
        transform.parent = null;

        //If there is more than one instance, destroy the extra else Set the static instance to this instance
        if (Instance != null && Instance != this) { Destroy(this.gameObject); } else { Instance = this; DontDestroyOnLoad(gameObject); }
    }
}

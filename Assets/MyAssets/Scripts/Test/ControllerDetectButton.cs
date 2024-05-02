using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.Events;

public class ControllerDetectButton : MonoBehaviour
{
    public int playerId = 0; // The ID for the player
    private Player player; // The Rewired Player

    public string gamepadButton;
    public UnityEvent actionPerform;
    bool _Active;


    void Start()
    {
        player = ReInput.players.GetPlayer(playerId); // Get the Rewired Player object for this player.
    }

    void Update()
    {
        if (player.GetButtonDown(gamepadButton) && _Active == true) // Check if the Interact button was pressed
        {
            PerformAction();
        }
    }

    void PerformAction()
    {
        actionPerform.Invoke();
    }


    private void OnEnable()
    {
        _Active = true;
    }
    private void OnDisable()
    {
        _Active = false;
    }
}

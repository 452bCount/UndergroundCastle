using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class PlayerHud : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI playerText;
    [SerializeField] public TextMeshProUGUI lifeText;
    [SerializeField] public TextMeshProUGUI placeText;
    [SerializeField] public Transform reloadUI;
    [SerializeField] public Transform aliveUI;
    [SerializeField] public Transform deathUI;
    [SerializeField] public Transform health;
    [SerializeField] public List<Image> brightHubColor;
    [SerializeField] public List<Image> darkHubColor;

    public void SetPlayerColor(Color pColor)
    {
        // Brighten Color
        foreach (var imageBright in brightHubColor)
        {
            imageBright.color = pColor;
        }

        // Darken Color
        Color darkerColor = Color.Lerp(pColor, Color.black, 0.5f);
        foreach (var imageDark in darkHubColor)
        {
            imageDark.color = darkerColor;
        }
    }

    public void UpdateHP(float currentHealth, float maxHealth)
    {
        health.localScale = new Vector3((currentHealth/maxHealth), 1f);
    }

    public void UpdateLife(int life)
    {
        lifeText.text = $"LIFE: X{life}";
    }

    public void respawnOffUI() { reloadUI.gameObject.SetActive(true); }
    public void respawnOnUI() { reloadUI.gameObject.SetActive(false); }

    public void deathOnUI() { aliveUI.gameObject.SetActive(false); deathUI.gameObject.SetActive(true); }
}

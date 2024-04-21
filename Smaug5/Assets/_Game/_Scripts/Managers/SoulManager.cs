using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SoulManager : MonoBehaviour
{
    public TextMeshProUGUI soulDisplay;

    void Update()
    {
        if (soulDisplay == null)
        {
            soulDisplay = FindObjectOfType<TextMeshProUGUI>();
            soulDisplay.SetText("Almas: " + _playerStats.Souls);
        }
    }
}

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EngraneUI : MonoBehaviour
{
    [SerializeField] private string label; // Texto que aparece como "Current Health"
    [SerializeField] private TextMeshProUGUI textComponent;

    public void UpdateCoins(int coins, int maxCoins)
    {
        textComponent.text = $"{label}: {coins}/{maxCoins}";
    }
}

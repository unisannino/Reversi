using UnityEngine;
using System.Collections;
using TMPro;

public class PlayerView
{
    TextMeshProUGUI _score;

    public PlayerView(TextMeshProUGUI score)
    {
        _score = score;
    }

    public void UpdateScore(int score)
    {
        _score.text = $"{score:00}";
    }
}
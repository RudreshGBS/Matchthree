using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardScorePanel : MonoBehaviour
{
    public TextMeshProUGUI Rank;
    public TextMeshProUGUI Player;
    public TextMeshProUGUI Score;
    public Image higilightImage;

    public void SetPlayerScore(string rank,string player, string id,bool isHighlighted)
    {
        Rank.text = rank;
        Player.text = player;
        Score.text = id;
        higilightImage.enabled = isHighlighted;
    }
}

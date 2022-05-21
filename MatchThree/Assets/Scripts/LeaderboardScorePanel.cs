using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardScorePanel : MonoBehaviour
{
    public TextMeshProUGUI Player;
    public TextMeshProUGUI Score;

    public void SetPlayerScore(string player, string id)
    {
        Player.text = player;
        Score.text = id;
    }
}

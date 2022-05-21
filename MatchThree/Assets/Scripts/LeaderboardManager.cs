using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField]
    private GameObject leaderboardScorePanel;
    [SerializeField]
    private GameObject content;
    [SerializeField]
    private FirebaseManager firebaseManager;
    public async void PopulateLeaderBoard()
    {
        var UserList = await firebaseManager.OnShowLeaderboardButtonClicked();
        if (UserList.Count > 0)
        {
            foreach (var user in UserList)
            {
                var O = Instantiate(leaderboardScorePanel, content.transform);
                var panel = O.GetComponent<LeaderboardScorePanel>();
                panel.SetPlayerScore(user.id, user.score);
            }
        }
    }

    public void ClearLeaderboardContent()
    {
        var _content = content.GetComponentsInChildren<LeaderboardScorePanel>();
        foreach(var c in _content)
        {
            GameObject.Destroy(c.gameObject);
        }
    }

    public string GetLast(string source, int numberOfChars)
    {
        if (numberOfChars >= source.Length)
            return source;
        return source.Substring(source.Length - numberOfChars);
    }
}

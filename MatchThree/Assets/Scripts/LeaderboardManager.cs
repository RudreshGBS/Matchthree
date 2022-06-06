using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField]
    private GameObject leaderboardScorePanel;
    [SerializeField]
    private GameObject content;
    [SerializeField]
    private FirebaseManager firebaseManager;
    [SerializeField]
    private ScrollRect scrollRect;
    protected RectTransform contentPanel;

    public async void PopulateLeaderBoard()
    {
        var UserList = await firebaseManager.OnShowLeaderboardButtonClicked();
        if(UserList == null)
        {
            Debug.LogError("Leaderboard List is Empty!!");
            return;
        }
        if (UserList.Count > 0)
        {
            int rank = 1;
            foreach (var user in UserList)
            {
                var O = Instantiate(leaderboardScorePanel, content.transform);
                var panel = O.GetComponent<LeaderboardScorePanel>();
                Debug.Log($"wallet id= {GameDataStore.WalletId} ,  user.id : {user.id}");
                Debug.Log($"wallet id== user.id : {user.id.Contains(GameDataStore.WalletId)}");
                panel.SetPlayerScore(rank.ToString(),user.id, user.score, user.id.Contains(GameDataStore.WalletId));
                rank++;
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
   

    public void SnapTo(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();

        contentPanel.anchoredPosition =
                (Vector2)scrollRect.transform.InverseTransformPoint(contentPanel.position)
                - (Vector2)scrollRect.transform.InverseTransformPoint(target.position);
    }
}

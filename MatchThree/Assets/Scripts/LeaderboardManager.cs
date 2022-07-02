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
    [SerializeField]
    private RectTransform contentPanel;
    List<LeaderboardScorePanel> leaderboardElementList;
    public async void PopulateLeaderBoard()
    {
        var UserList = await firebaseManager.OnShowLeaderboardButtonClicked();
        leaderboardElementList = new List<LeaderboardScorePanel>();
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
                leaderboardElementList.Add(panel);
                rank++;
            }
        }
        var CurrentUser =  leaderboardElementList.Find(currentUser => currentUser.Player.text.Contains(GameDataStore.WalletId));
        SnapTo(CurrentUser.gameObject.GetComponent<RectTransform>());
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

        Vector2 calculatedPos =
                (Vector2)scrollRect.transform.InverseTransformPoint(contentPanel.position)
                - (Vector2)scrollRect.transform.InverseTransformPoint(target.position);
        contentPanel.anchoredPosition = new Vector2(contentPanel.anchoredPosition.x, calculatedPos.y);
        Debug.Log($"x:{contentPanel.anchoredPosition.x}, y:{contentPanel.anchoredPosition.y}");
    }
}

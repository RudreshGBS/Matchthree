using Firebase;
using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using System.Linq;
using System.Threading.Tasks;

public class FirebaseManager : MonoBehaviour
{
    public UserList LeaderboardList = new UserList();
    UserList userlist = new UserList();
    DatabaseReference databaseReference;
    void Start()
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        //PopulateLeaderBoard(0);

        //for (int i = 1; i <= 10; i++)
        //{
        //    saveData(i + "00000000000000000xsdadhasdhads00000" + i, i * Random.Range(1000, 10000));
        //}
        //StartCoroutine(ShowLeaderboard());
    }

    public void saveData(string id, int score)
    {
        User user1 = new User();
        user1.id = id;
        user1.score = score.ToString();
        userlist.Users.Add(user1);
        var jsondata = JsonUtility.ToJson(userlist);
        databaseReference.SetRawJsonValueAsync(jsondata);
        Debug.Log("Saved Data : " + jsondata);

        /// Last Working Structure
        //databaseReference.Child("Users").Child(id).Child("Score").SetValueAsync(score);

    }


    async Task PopulateLeaderBoard(int LoadLimit = 0)
    {
        LeaderboardList.Users.Clear();
        var top5 = (LoadLimit == 0) ? FirebaseDatabase.DefaultInstance.GetReference("Users").OrderByChild("score") : FirebaseDatabase.DefaultInstance.GetReference("Users").OrderByChild("score").LimitToFirst(LoadLimit);
        // Keep this query synced.
        top5.KeepSynced(true);

        await top5.GetValueAsync().ContinueWith(task =>
        {
            if (task.Exception != null)
            {
                throw task.Exception;
            }
            if (!task.IsCompleted)
            {
                return;
            }

            // Iterate children in order based on "score"
            if (task.Result != null)
            {
                foreach (var record in task.Result.Children)
                {
                    User user1 = new User();
                    user1.id = record.Child("id").GetRawJsonValue();
                    user1.score = record.Child("score").GetRawJsonValue();
                    //Debug.Log(user1.id + " " + user1.score);
                    LeaderboardList.Users.Add(user1);
                    //Debug.Log("Loaded Data: ID: " + record.Child("id").GetRawJsonValue() + " Score: " + record.Child("score").GetRawJsonValue());
                }
            }
        });
    }

    public async Task<List<User>> OnShowLeaderboardButtonClicked()
    {
        await PopulateLeaderBoard(5);
        if (LeaderboardList.Users.Count > 0)
        {
            return LeaderboardList.Users;
        }
        else
        {
            Debug.LogError("User List is Empty");
            return LeaderboardList.Users;
        }
        //StartCoroutine(ShowLeaderboard());
    }
}
[System.Serializable]
public class UserList
{
    public List<User> Users = new List<User>();
}

[System.Serializable]
public class User
{
    public string id;
    public string score;
}


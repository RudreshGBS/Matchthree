using Firebase;
using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using System.Linq;
using System.Threading.Tasks;
using System;

public class FirebaseManager : MonoBehaviour
{
    public UserList LeaderboardList = new UserList();
    public UserList UsersList = new UserList();
    UserList userlist = new UserList();
    DatabaseReference databaseReference;
    public static string userData = null;
    public bool IsUserValid = false;


    void Start()
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        //--------------- To Add Dummy Data to Database -------------------
        //for (int i = 1; i <= 1000; i++)
        //{
        //    SaveUsernameScore(i + "00000000000000000xsdadhasdhads00000" + i, i * Random.Range(1000, 10000));
        //}

        //---------------To Check if the User's Local Key exisits in Database or not ----------------
        //GameDataStore.Key = "-N2fH6nk9ZbrvoR1vesL";
        //isUserKeyValid(GameDataStore.Key, "400000000000000000xsdadhasdhads000002", ()=>
        //{
        //    if (IsUserValid)
        //    {
        //        Debug.Log("User Key Valid");
        //    }
        //});

        //---------------------To Check if the User exisits in Database or not ----------------
        //CheckExisitngUserInDatabaseWithoutKey("400000000000000000xsdadhasdhads000002", (value) =>
        //{
        //    if (value)
        //    {
        //        Debug.Log("Username in Database found");
        //    }
        //    else
        //    {
        //        Debug.Log("No User Found in database, Creating a new User");
        //    }
        //});
    }


    #region Public Methods

    public async void CheckExisitngUserInDatabaseWithoutKey(string id, Action<bool> onSucess)
    {
        var isUserFound = false;
        await FirebaseDatabase.DefaultInstance.GetReference("Users").OrderByChild("score").GetValueAsync().ContinueWith(task =>
        {
            if (task.Result != null)
            {
                foreach (var record in task.Result.Children)
                {
                    if (string.Compare($"\"{id}\"", record.Child("id").GetRawJsonValue()) == 0)
                    {
                        onSucess?.Invoke(true);
                        isUserFound = true;
                    }
                }
                if(!isUserFound)
                {
                    onSucess?.Invoke(false);
                }
            }

        });
    }

    public async void CheckExisitngUserInDatabaseWitKey(string key, string id, Action onSucess)
    {
        var task1 = await VerifyUsernameQuery(key, id);
        IsUserValid = task1;
        onSucess?.Invoke();
    }
    public async void SaveUsernameScore(string id, int score)
    {
        User user1 = new User();
        user1.id = id;
        user1.score = score.ToString();
        var jsondata = JsonUtility.ToJson(user1);
        var key = FirebaseDatabase.DefaultInstance.GetReference("Users").Push().Key;
        GameDataStore.Key = key;
        await FirebaseDatabase.DefaultInstance.GetReference("Users").Child(key).SetRawJsonValueAsync(jsondata);
    }

    public async Task<List<User>> OnShowLeaderboardButtonClicked()
    {
        SaveUsernameScore(UnityEngine.Random.Range(1, 10) + "00000000000000000xsdadhasdhads00000" + UnityEngine.Random.Range(1, 10), UnityEngine.Random.Range(1, 10) * UnityEngine.Random.Range(1000, 10000));
        await PopulateLeaderBoard(5);
        if (LeaderboardList.Users.Count > 0)
        {
            return LeaderboardList.Users;
        }
        else
        {
            Debug.LogError("User List is Empty");
            return null;
        }
        //StartCoroutine(ShowLeaderboard());
    }


    #endregion

    #region Private Methods


    private async Task<bool> VerifyUsernameQuery(string key, string id)
    {
        await FirebaseDatabase.DefaultInstance.GetReference("Users")
            .Child(key)
            //.OrderByValue()
            .GetValueAsync()
            .ContinueWith(task =>
            {
                DataSnapshot result = task.Result;
                userData = result.Child("id").Value.ToString() ?? null;
            });
        if (string.Compare(id, userData) == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
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


    #endregion

    #region Old Implementations


    //------------------------ Old Method of Saving the Data ----------------------------
    //public void saveData(string id, int score)
    //{
    //    User user1 = new User();
    //    user1.id = id;
    //    user1.score = score.ToString();
    //    userlist.Users.Add(user1);
    //    var jsondata = JsonUtility.ToJson(userlist);
    //    databaseReference.SetRawJsonValueAsync(jsondata);
    //    Debug.Log("Saved Data : " + jsondata);

    //    /// Last Working Structure
    //    //databaseReference.Child("Users").Child(id).Child("Score").SetValueAsync(score);

    //}

    #endregion

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


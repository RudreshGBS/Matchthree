using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Need to Affiliate this class with the RealtimeDatabase
/// </summary>
public static class GameDataStore 
{
    public static int CurrentLevel { get; set; }

    private static int val;

    public static int LastUnloackedLevel 
    {
        get 
        {
            return val;
        }
        set {
             val = value;
            PlayerPrefs.SetInt("LastUnloackedLevel", value);
        } 
    }
    public static int Score 
    {   get
        {
            return _score;
        }
        set
        {
            _score = value;
        }
    }

    private static int _score;
    
    public static bool isFirsttime = true;

    public static bool CanMoveRocket = true;

    private static string key;

    public static string Key;
    public static string WalletId;
    public static int WalletBal;
   


    public static async void LoadData()
    {
        if (PlayerPrefs.HasKey("LastUnloackedLevel"))
        {
            var _score = await FirebaseManager.LoadScore();
            if (int.TryParse(_score, out GameDataStore._score))
            {
                Debug.Log("Data Loaded Successfully");
            }
        }
        else {
            Debug.Log("Anti Cheat activate");
            SaveData(0);
        }
    }
    public static void SaveData(int score)
    {
        _score += score;
        FirebaseManager.SaveScore(_score, () => 
        {
            Debug.Log("Data Saved Successfully");
        });
    }
    
}

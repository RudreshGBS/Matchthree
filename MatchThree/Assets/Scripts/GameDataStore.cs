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
    public static int Score { get; set; }
    
    public static bool isFirsttime = true;

    public static bool CanMoveRocket = true;

    private static string key;
    
    public static string Key
    {
        get
        {
            key = PlayerPrefs.GetString("DatabaseKey");
            return key ?? null;
        }
        set
        {
            key = value;
            PlayerPrefs.SetString("DatabaseKey", value);
        }
    }


    public static void LoadData()
    {
        //Load from Realtime Database
    }
    public static void SaveData()
    {

    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Need to Affiliate this class with the RealtimeDatabase
/// </summary>
public static class GameDataStore 
{
    public static int CurrentLevel { get; set; }
    public static int Score { get; set; }
    public static void LoadData()
    {
        //Load from Realtime Database
    }
}

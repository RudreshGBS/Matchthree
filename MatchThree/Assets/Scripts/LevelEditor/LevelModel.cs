using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "LevelEditor/Level", order = 1)]
public class LevelModel : ScriptableObject
{
    public string levelNo;
    public List<Sprite> symbols = new List<Sprite>();
    public Sprite background;
    public Sprite EGTItem;
    public Sprite blockFrame;
    public int rows;
    public int cols;
    public int maxMoves;
    public int time;
    public int tagetScore;
}

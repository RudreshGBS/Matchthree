using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    public int Level = 0;
    public bool isActiveLevel = false;
    public Color Enabled;
    public Color Disabled;
    public Transform RocketEndPosition;
    private SpriteRenderer planet;
    private void Start()
    {
        planet = GetComponent<SpriteRenderer>();
        SetupLevel();
    }
    public void SetupLevel()
    {
        if(!isActiveLevel)
        {
            planet.color = Disabled;
        }
        else
        {
            planet.color = Enabled;
        }
    }

    public void LoadLevel()
    {
        //Load the Main Scene from Here
    }
}

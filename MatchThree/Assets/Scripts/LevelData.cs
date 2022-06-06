using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelData : MonoBehaviour
{
    /// <summary>
    /// Level Number 
    /// </summary>
    public int Level = 0;


    /// <summary>
    /// Is this Level Active or not
    /// </summary>
    public bool isActiveLevel = false;


    /// <summary>
    /// If Level is active or not, What should be it's color
    /// </summary>
    public Color Enabled;
    public Color Disabled;

    public Color ButtonEnabled;
    public Color ButtonDisabled;

    public SpriteRenderer Platform;

    /// <summary>
    /// Reference of the Rocekt's Waypoints from current Planet to Next Planet
    /// </summary>
    public Transform[] RocketWaypoints;


    //Reference of the Clickable Level Button
    public Button levelButton;

    [SerializeField]
    private TMP_Text planetButtonText;

    private void Start()
    {
        Level = Convert.ToInt32(gameObject.name);
        planetButtonText.text = Level.ToString();
        SetupLevel();
    }


    /// <summary>
    /// Enable Disable the planet features according to the planet's activeness
    /// </summary>
    public void SetupLevel()
    {
        if(!isActiveLevel)
        {
            Platform.color = Disabled;
            levelButton.image.color = ButtonDisabled;
            levelButton.interactable = false;
            planetButtonText.color = Disabled;
        }
        else
        {
            Platform.color = Enabled;
            levelButton.image.color = ButtonEnabled;
            levelButton.interactable = true;
            planetButtonText.color = Enabled;
        }
    }


    /// <summary>
    /// Use this Method to change to any OLD LEVEL Scene and use the integer Level to send it across
    /// </summary>
    public void LoadLevelOnButtonClick()
    {
        GameDataStore.CurrentLevel = Level;
        // HINT use Level
        Debug.Log($"User Clicked an Active Level: { this.Level} Now load the Main Scene with this level");
        SceneManager.LoadScene("Main");


    }
}

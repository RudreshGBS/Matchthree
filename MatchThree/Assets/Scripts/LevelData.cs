using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelData : MonoBehaviour
{
    public int Level = 0;
    public bool isActiveLevel = false;
    public Color Enabled;
    public Color Disabled;
    public Transform RocketEndPosition;
    private Image planet;
    [SerializeField]
    private Button planetButton;
    [SerializeField]
    private TMP_Text planetButtonText;
    private void Start()
    {
        planet = GetComponent<Image>();
        planetButtonText.text = Level.ToString();
        SetupLevel();
    }
    public void SetupLevel()
    {
        if(!isActiveLevel)
        {
            planet.color = Disabled;
            planetButton.image.color = Disabled;
            planetButton.interactable = false;
            planetButtonText.color = Disabled;
        }
        else
        {
            planet.color = Enabled;
            planetButton.image.color = Enabled;
            planetButton.interactable = true;
            planetButtonText.color = Enabled;
        }
    }

    public void LoadLevel()
    {
        //Load the Main Scene from Here
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManger : MonoBehaviour
{
    public GameObject mainMenu; 
    public GameObject Logo; 
    public void OnPlayButtonCLick() 
    {
        SceneManager.LoadScene("Main");
    }
    public void ShowMainMenu() { 
        mainMenu.SetActive(true);
        Logo.SetActive(false);
    }
}

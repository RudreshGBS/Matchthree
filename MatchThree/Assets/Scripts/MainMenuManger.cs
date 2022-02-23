using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManger : MonoBehaviour
{
    public GameObject mainMenu; 
    public GameObject ConnectWalletObject; 
    public GameObject ConnectButton; 
    public GameObject Logo; 
    public void OnPlayButtonCLick() 
    {
        SceneManager.LoadScene("Main");
    }
    public void ConnectWallet() {
        Logo.SetActive(false);
        ConnectWalletObject.SetActive(true);
        ShowMainMenu();
        ConnectButton.SetActive(true);
    }
    public void ShowMainMenu() { 
        mainMenu.SetActive(true);
    }

}

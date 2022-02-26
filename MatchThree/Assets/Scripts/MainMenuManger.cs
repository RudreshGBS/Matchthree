using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManger : MonoBehaviour
{
    public GameObject mainMenu; 
    public GameObject connectWalletObject; 
    public GameObject connectButton; 
    public GameObject logo; 
    public GameObject playButton;
    public GameObject settingsButton;
    public GameObject minBalanceError;
    public GetBalance getBalance;
    public int minNFT;

    private void OnEnable()
    {
        getBalance.onGettingBalance += CheckMinimumBalance;
    }
    public void OnPlayButtonCLick() 
    {
        SceneManager.LoadScene("Main");
    }
    public void ConnectWallet() {
        logo.SetActive(false);
        connectWalletObject.SetActive(true);
        ShowMainMenu();
        connectButton.SetActive(true);
    }
    public void ShowMainMenu() { 
        mainMenu.SetActive(true);
        playButton.SetActive(false);
        settingsButton.SetActive(false);
    }
    public void ShowGameMenu() {
        playButton.SetActive(true);
        settingsButton.SetActive(true); 
    }
    public void CheckMinimumBalance() 
    {
        if (getBalance.bal >= minNFT)
        {
            ShowGameMenu();
        }
        else 
        {
            minBalanceError.SetActive(true);
        }
        getBalance.onGettingBalance -= CheckMinimumBalance;
    }
    public void QuitGame() 
    { 
        Application.Quit();
    }
}

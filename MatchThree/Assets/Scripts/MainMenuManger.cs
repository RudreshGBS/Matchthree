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
    public GameObject leaderboardButton;
    public GameObject leaderboardMenu;
    public GameObject minBalanceError;
    public GetBalance getBalance;
    public int minNFT;
    [SerializeField]
    private LeaderboardManager leaderBoardManager;
    private void OnEnable()
    {
        getBalance.onGettingBalance += CheckMinimumBalance;
    }
    public void OnPlayButtonCLick()
    {
        SceneManager.LoadScene("LevelSelection");
    }
    public void OnLeaderboardButtonClick()
    {
        leaderboardButton.SetActive(false);
        playButton.SetActive(false);
        leaderboardMenu.SetActive(true);
        leaderBoardManager.PopulateLeaderBoard();
    }
    public void OnLeaderboardMainMenuButtonClick()
    {
        playButton.SetActive(true);
        leaderboardButton.SetActive(true);
        leaderboardMenu.SetActive(false);
        leaderBoardManager.ClearLeaderboardContent();
    }
    public void ConnectWallet() {
        logo.SetActive(false);
#if !UNITY_EDITOR
        if (GameDataStore.isFirsttime)
        {

        connectWalletObject.SetActive(true);
        ShowMainMenu();
        connectButton.SetActive(true);

        }
        else 
        { 
            ShowMainMenu();
            ShowGameMenu();
        }
#endif
#if UNITY_EDITOR
        ShowMainMenu();
        ShowGameMenu();
#endif

    }
    public void ShowMainMenu() { 
        mainMenu.SetActive(true);
        playButton.SetActive(false);
        leaderboardButton.SetActive(false);
    }
    public void ShowGameMenu() {
        playButton.SetActive(true);
        leaderboardButton.SetActive(true); 
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

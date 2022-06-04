using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public GameObject soundButton;
    public GameObject leaderboardMenu;
    public GameObject minBalanceError;
    public GameObject ALDError;
    public TMP_Text ALDErrorMessage;
    public GetBalance getBalance;
    public ALD Ald;
    public LoadALD loadALD;
    public int minNFT;
    public TMP_Text SoundText;
    public AudioSource audioSource;
    [SerializeField]
    private LeaderboardManager leaderBoardManager;
    

    private void Awake()
    {
        loadALD.LoadData();
        toggleSound();

    }
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
        if (Ald.ALDModel.isvalid)
        {

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
        else
        {
            ShowMainMenu();
            ALDErrorMessage.text = Ald.ALDModel.message;
            ALDError.SetActive(true);
        }
    }
    
    public void ShowMainMenu() { 
        mainMenu.SetActive(true);
        playButton.SetActive(false);
        leaderboardButton.SetActive(false);
        soundButton.SetActive(false);
    }
    public void ShowGameMenu() {
        playButton.SetActive(true);
        leaderboardButton.SetActive(true);
        soundButton.SetActive(true);
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
    public void toggleSound() 
    {
        if (audioSource.mute)
        {
            //UNMUTE
            audioSource.mute = false;
            SoundText.text = "Sound : OFF";
        }
        else {
            audioSource.mute = true;
            SoundText.text = "Sound : ON";
        }
    }
}

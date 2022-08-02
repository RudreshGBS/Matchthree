using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
    public GameObject quitButton;
    public GameObject leaderboardMenu;
    public GameObject minBalanceError;
    public GetBalance getBalance;
    public FirebaseManager firebaseManager;

    public int minNFT;
    public TMP_Text SoundText;
    public SoundManager soundManager;
    public AudioSource audioSource;
    public GameObject quitBox;
    [SerializeField]
    private LeaderboardManager leaderBoardManager;
    [SerializeField]
    private GameObject UserNamePopup;
    [SerializeField]
    public TMP_InputField usernameText;

    private void Awake()
    {
        toggleSound();
    }
    private void Start()
    {
        soundManager = SoundManager.Instance;
        audioSource = soundManager.GetComponent<AudioSource>();
    }

    private void ShowWalletIdAndBal()
    {
        if (!String.IsNullOrEmpty(GameDataStore.WalletId) && GameDataStore.WalletBal != null)
        {
            getBalance.accountText.text = $"{GameDataStore.WalletId}";
            getBalance.resultText.text = $"your account balance is  {GameDataStore.WalletBal}";
            getBalance.accountText.gameObject.SetActive(true);
            getBalance.resultText.gameObject.SetActive(true);
        }
    }

    private void OnEnable()
    {

        getBalance.onGettingBalance += CheckMinimumBalance;
    }

    public void OpenQuitBox() { 
    
        quitBox.SetActive(true);
    }
    public void closeQuitBox()
    {
        quitBox.SetActive(false);
    }
    public void OnPlayButtonCLick()
    {
        SceneManager.LoadScene("LevelSelection_New");
    }
    public void OnLeaderboardButtonClick()
    {
        leaderboardButton.SetActive(false);
        playButton.SetActive(false);
        soundButton.SetActive(false);
        quitButton.SetActive(false);
        leaderboardMenu.SetActive(true);
        leaderBoardManager.PopulateLeaderBoard();
    }
    public void OnLeaderboardMainMenuButtonClick()
    {
        playButton.SetActive(true);
        leaderboardButton.SetActive(true);
        soundButton.SetActive(true);
        quitButton.SetActive(true);
        leaderboardMenu.SetActive(false);
        leaderBoardManager.ClearLeaderboardContent();
    }
    public void ConnectWallet() {
        logo.SetActive(false);
        //#if !UNITY_EDITOR
        //        if (GameDataStore.isFirsttime)
        //        {

        //        connectWalletObject.SetActive(true);
        //        ShowMainMenu();
        //        connectButton.SetActive(true);

        //        }
        //        else 
        //        { 
        //            ShowMainMenu();
        //            ShowGameMenu();
        //        }
        //#endif
        //#if UNITY_EDITOR
        if (GameDataStore.isFirsttime)
        {
            if (PlayerPrefs.HasKey("username"))
            {
                SetIDandDB(PlayerPrefs.GetString("username"));
                ShowMainMenu();
                ShowGameMenu();
            }
            else 
            {
                ShowMainMenu();
                UserNamePopup.SetActive(true);
            }
        }
        else 
        { 
            ShowMainMenu();
            ShowGameMenu();
        }
//#endif  
        
    }

    private void SetIDandDB(string username)
    {
        GameDataStore.WalletId = username;
        SetDB(GameDataStore.WalletId);
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
        ShowWalletIdAndBal();

    }
    public void SubmitUserName() {
        if (string.IsNullOrWhiteSpace(usernameText.text)) {
            usernameText.placeholder.GetComponent<TMP_Text>().enabled = true;
            usernameText.placeholder.GetComponent<TMP_Text>().text = "Invalid ID";
            return;
        }
        PlayerPrefs.SetString("username", usernameText.text);
        SetIDandDB(usernameText.text);
        UserNamePopup.SetActive(false);
        ShowGameMenu();
    }
    public void CheckMinimumBalance(string id) 
    {
        if (getBalance.bal >= minNFT)
        {
            ShowGameMenu();
        }
        else 
        {
            minBalanceError.SetActive(true);
        }
        SetDB(id);
        getBalance.onGettingBalance -= CheckMinimumBalance;
    }
    public void QuitGame() 
    { 
        Application.Quit();
    }
    public void toggleSound() 
    {
        if (audioSource == null) {
            return;
        }
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

   
    
    private void SetDB(string id)
    {
        Debug.Log($"Key : {GameDataStore.Key}");

        if (string.IsNullOrEmpty(GameDataStore.Key))
        {
            Debug.Log("Local Database Key found, Searching for the Username in the Database");
            firebaseManager.CheckExisitngUserInDatabaseWitKeyAsync(GameDataStore.Key, id, () =>
            {
                if (firebaseManager.IsUserValid)
                {
                    Debug.Log("Existing User found, Loading the Data Now!");
                    GameDataStore.LoadData();
                }
                else
                {
                    Debug.Log("No User Found in database, Creating a new User");
                    firebaseManager.SaveUsernameScore(id, 0);
                }
            });
        }
        else
        {
            Debug.Log("Local Database Key not found, Searching for the Username in the Database");
            firebaseManager.CheckExisitngUserInDatabaseWithoutKey(id, (value) =>
            {
                if (value)
                {
                    Debug.Log("Username in Database found");
                    GameDataStore.LoadData();

                }
                else
                {
                    Debug.Log("No User Found in database, Creating a new User");
                    firebaseManager.SaveUsernameScore(id, 0);

                }
            });
        }
    }
}

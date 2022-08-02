using System.Collections;
using System.Collections.Generic;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject levelselctionButton;
    public GameObject achievementPanel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI resultText;


    private void OnEnable()
    {
        //int score = PlayerPrefs.GetInt("score");
        //int highScore;
        //if (PlayerPrefs.HasKey("highScore"))
        //{
        //    highScore = PlayerPrefs.GetInt("highScore");
        //}
        //else 
        //{
        //    highScore = 0; 
        //}
        //if (score > highScore)
        //{
            //PlayerPrefs.SetInt("highScore", score);
            //highScoreText.text = score.ToString();
            //HighScoreAlert.SetActive(true);
        //}
        //else
        //{
            //HighScoreAlert.SetActive(false);
            //highScoreText.text = highScore.ToString();
        //}
        //scoreText.text = score.ToString();
    }


    public void OnResult(bool isPass,int score) 
    {
        scoreText.text = score.ToString();
        levelselctionButton.SetActive(isPass);
        achievementPanel.SetActive(isPass);
        resultText.text = isPass?"Pass":"Fail";
        resultText.color = isPass?Color.green:Color.red;
        if(isPass)
        {
            Debug.Log($"CL: {GameDataStore.CurrentLevel}");
            if (PlayerPrefs.HasKey($"Level{GameDataStore.CurrentLevel}"))
            {
                var lastscore = PlayerPrefs.GetInt($"Level{GameDataStore.CurrentLevel}");
                Debug.Log($"lastscore = {lastscore} , Score = {score}");
                if (score > lastscore) 
                {
                    PlayerPrefs.SetInt($"Level{GameDataStore.CurrentLevel}", score);
                    GameDataStore.SaveData(score-lastscore);
                }
            }
            else 
            {
                PlayerPrefs.SetInt($"Level{GameDataStore.CurrentLevel}", score);
                GameDataStore.SaveData(score);
            }
        }
    }
  
    public void RestartLevel()
    {
        Destroy(GridManager.Instance.gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void GotoLevelSelection()
    {
        Destroy(GridManager.Instance.gameObject);
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("LevelSelection_New");
    }
    public void GotoMainMenu()
    {
        Destroy(GridManager.Instance.gameObject);
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("MainMenu");
    }
}

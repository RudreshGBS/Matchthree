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
        resultText.text = isPass?"Pass":"Fail";
        resultText.color = isPass?Color.green:Color.red;
    }
  
    public void RestartLevel()
    {
        Destroy(GridManager.Instance.gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void GotoLevelSelection()
    {
        Destroy(GridManager.Instance.gameObject);
        SceneManager.LoadScene("LevelSelection");
    }
    public void GotoMainMenu()
    {
        Destroy(GridManager.Instance.gameObject);
        SceneManager.LoadScene("MainMenu");
    }
}

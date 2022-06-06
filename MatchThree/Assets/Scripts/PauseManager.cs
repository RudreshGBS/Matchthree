using rudscreation.Utils;
using System.Collections;
using System.Collections.Generic;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public void Pause() 
    {   
        gameObject.SetActive(true);
        TimerHandler.Instance.PauseTimer();
        Time.timeScale = 0.0f;
    }
    public void Resume() {
        gameObject.SetActive(false);
        TimerHandler.Instance.ResumeTimer();
        Time.timeScale = 1.0f;
    }
  
    public void RestartLevel()
    {
        gameObject.SetActive(false);
        Destroy(GridManager.Instance.gameObject);
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
   
    public void GotoMainMenu()
    {
        gameObject.SetActive(false);
        Destroy(GridManager.Instance.gameObject);
        Time.timeScale = 1.0f;
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("MainMenu");
    }
}

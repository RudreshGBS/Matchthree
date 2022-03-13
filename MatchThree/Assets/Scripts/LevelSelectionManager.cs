using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionManager : MonoBehaviour
{
    /// <summary>
    /// TODO: currentLevel = GameDataStore.CurrentLevel; should be used
    /// this is the Current Level of the player at the moment
    /// </summary>
    public int CurrentLevel;
    
    
    /// <summary>
    /// Total Number of Levels should be updated here
    /// Logical Calculations like Panning the Camera depends on total number of Levels
    /// </summary>
    public float TotalLevels;


    /// <summary>
    /// Gameobject Reference of visible Rocket Path
    /// </summary>
    public GameObject Path;
    
    /// <summary>
    /// Reference of the Scrollbar to pan the Camera as needed
    /// </summary>
    [SerializeField]
    private Scrollbar scrollBar;


    /// <summary>
    /// Reference of Rocket Controller
    /// </summary>
    [SerializeField]
    private RocketController rocketController;


    /// <summary>
    /// All the levels with Level Data Component on it should be referenced here 
    /// </summary>
    [SerializeField]
    private LevelData[] levels;


    /// <summary>
    /// Returns the current leveldata
    /// </summary>
    public LevelData CurrentLevelData 
    { 
        get
        {
            return levels[CurrentLevel - 1];
        }
     }


    /// <summary>
    /// Event for Loading Next Level
    /// </summary>
    public Action LoadNextLevelNow;


    void Start()
    {
        if (scrollBar != null)
        {
            double slidingValue;
            if(CurrentLevel == 1)
            {
                slidingValue = 0;
            }
            else if(CurrentLevel == TotalLevels)
            {
                slidingValue = 1;
            }
            else
            {
                slidingValue = CurrentLevel / TotalLevels;
            }
            scrollBar.value = (float)slidingValue < 0.75 ? (float)slidingValue - 0.1f : (float)slidingValue;
        }

        ActivateLevels();
        
        //TODO: Launch Rocket Only after a level pass Event has been received
        rocketController.LaunchRocket();
    }

    /// <summary>
    /// Use this method when Switching to the next level Scene
    /// </summary>
    void LoadNextLevel()
    {
        Debug.Log("LoadNextLevelNow");
        if (CurrentLevel < TotalLevels)
        {
            CurrentLevel += 1;
        }
        ActivateLevels(CurrentLevel);
        // After Some Time Load the Scene here after



    }

    /// <summary>
    /// withtout param will Setup all the levels according to the current active level
    /// Setup Next Level using the param 
    /// </summary>
    /// <param name="level"></param>
    public void ActivateLevels(int level = -1)
    {
        if (level == -1)
        {
            for (int i = 0; i < CurrentLevel; i++)
            {
                levels[i].isActiveLevel = true;
                levels[i].SetupLevel();
            }
        }
        else
        {
            levels[level-1].isActiveLevel = true;
            levels[level-1].SetupLevel();
        }
    }
    /// <summary>
    /// When the Rocket is launched, disable/enable the Level Selection Buttons
    /// </summary>
    /// <param name="enabled"></param>
    public void SetActiveLevelsButtonOnRocketLaunch(bool enabled)
    {
        for (int i = 0; i < CurrentLevel; i++)
        {
            levels[i].levelButton.interactable = enabled;
        }
    }

    private void OnEnable()
    {
        LoadNextLevelNow += LoadNextLevel;
    }
    private void OnDisable()
    {
        LoadNextLevelNow -= LoadNextLevel;
    }
}

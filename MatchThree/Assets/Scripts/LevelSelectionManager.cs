using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionManager : MonoBehaviour
{
    /// <summary>
    /// TODO: currentLevel = GameDataStore.CurrentLevel; should be used
    /// this is the Current Level of the player at the moment
    /// </summary>
    private int LastUnloackedLevel;
    
    
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

    [SerializeField]
    public ScrollRect scroll;

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
            return levels[LastUnloackedLevel];
        }
     }

    bool startFirsRocketLaunch;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("LastUnloackedLevel"))
        {
            if (GameDataStore.isFirsttime||!GameDataStore.CanMoveRocket)
            {
                LastUnloackedLevel = PlayerPrefs.GetInt("LastUnloackedLevel");
            }
            else
            {
                 LastUnloackedLevel = GameDataStore.CurrentLevel;
                 GameDataStore.CanMoveRocket = false;
            }
            if (GameDataStore.LastUnloackedLevel == 0) 
            { 
                GameDataStore.LastUnloackedLevel = LastUnloackedLevel;
            }
            startFirsRocketLaunch = false;
        }
        else
        {
            GameDataStore.LastUnloackedLevel = 1;
            startFirsRocketLaunch = true;
        }
    }

    void Start()
    {

       
        if (scrollBar != null)
        {
            double slidingValue;
            if(LastUnloackedLevel == 1)
            {
                slidingValue = 0;
            }
            else if(LastUnloackedLevel == TotalLevels)
            {
                slidingValue = 1;
            }
            else
            {
                slidingValue = LastUnloackedLevel / TotalLevels;
            }
            scrollBar.value = (float)slidingValue < 0.75 ? (float)slidingValue - 0.1f : (float)slidingValue;
        }
        ActivateLevels();
        if (startFirsRocketLaunch) 
        {
            LastUnloackedLevel = GameDataStore.LastUnloackedLevel;
            rocketController.LaunchRocket();
            startFirsRocketLaunch= false;
        }

        if (LastUnloackedLevel != GameDataStore.LastUnloackedLevel && !startFirsRocketLaunch)
        {
            LastUnloackedLevel = GameDataStore.LastUnloackedLevel;
            rocketController.LaunchRocket();
        }

        
    }

    /// <summary>
    /// Use this method when Switching to the next level Scene
    /// </summary>
    //public void LoadNextLevel()
    //{
    //    Debug.Log("LoadNextLevelNow");
    //    if (LastUnloackedLevel < TotalLevels)
    //    {
    //        LastUnloackedLevel += 1;
    //    }
    //    ActivateLevels(LastUnloackedLevel);
    //    // After Some Time Load the Scene here after



    //}

    /// <summary>
    /// withtout param will Setup all the levels according to the current active level
    /// Setup Next Level using the param 
    /// </summary>
    /// <param name="level"></param>
    public void ActivateLevels(int level = -1)
    {
        if (level == -1)
        {
            for (int i = 0; i <= LastUnloackedLevel; i++)
            {
                levels[i].isActiveLevel = true;
                levels[i].SetupLevel();
            }
        }
        else
        {
            levels[level].isActiveLevel = true;
            levels[level].SetupLevel();
        }
    }
    /// <summary>
    /// When the Rocket is launched, disable/enable the Level Selection Buttons
    /// </summary>
    /// <param name="enabled"></param>
    public void SetActiveLevelsButtonOnRocketLaunch(bool enabled)
    {
        for (int i = 0; i < LastUnloackedLevel; i++)
        {
            levels[i].levelButton.interactable = enabled;
        }
    }

    private void OnEnable()
    {
        //GridManager.Instance.OnLevelResult += LevelPassed;

    }
    private void OnDisable()
    {
        //GridManager.Instance.OnLevelResult -= LevelPassed;

    }
    private void LevelPassed(int levelNo)
    {
       
    }
#if UNITY_EDITOR
    [MenuItem("Match three/Reset PlayerPrefs ")]
    static void ResetPP()
    {
        PlayerPrefs.DeleteAll();
        //PlayerPrefs.SetInt("LastUnloackedLevel", 0);
    }
#endif

}


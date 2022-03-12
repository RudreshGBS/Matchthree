using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionManager : MonoBehaviour
{
    public int CurrentLevel;
    public float TotalLevels;
    [SerializeField]
    private Scrollbar scrollBar;
    [SerializeField]
    private LevelData[] levels;
    // Start is called before the first frame update
    void Start()
    {
        //currentLevel = GameDataStore.CurrentLevel;

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
            scrollBar.value = (float)slidingValue - 0.15f;
        }

        ActivateLevels();
        
    }
    private void Update()
    {
        //if (scrollBar != null)
        //{
        //    if (scrollBar.value > 0.8f)
        //    {
        //        scrollBar.value = 0.8f;
        //    }
        //}
    }
    public void ActivateLevels()
    {
        for (int i = 0; i < CurrentLevel; i++)
        {
            levels[i].isActiveLevel = true;
            levels[i].SetupLevel();
        }
    }
}

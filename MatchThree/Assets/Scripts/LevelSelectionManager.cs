using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionManager : MonoBehaviour
{
    int currentLevel, nextLevel;
    [SerializeField]
    private Scrollbar scrollBar;
    // Start is called before the first frame update
    void Start()
    {
        currentLevel = GameDataStore.CurrentLevel;
        nextLevel = currentLevel++;
        ActivateLevels();
        if (scrollBar != null)
        {
            scrollBar.value = 0;
        }
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
        for (int i = 1; i <= GameDataStore.CurrentLevel; i++)
        {

        }
    }
}

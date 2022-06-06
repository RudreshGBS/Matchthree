using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SatueManager : MonoBehaviour
{
    [SerializeField]
    public List<StatueBlock> statueBlocks = new List<StatueBlock>();
    public int LastActivatedId;
    // Start is called before the first frame update
    void Start()
    {
       LastActivatedId = (GameDataStore.LastUnloackedLevel != 0) ? GameDataStore.LastUnloackedLevel - 1 : 0;
       
        FillUpProgress();
    }

    public void FillUpProgress() 
    {
        for (int i = 0; i < LastActivatedId; i++)
        {
            statueBlocks[i].EnableBlock(false);
        }

    }
    private void OnDisable()
    {
        statueBlocks.ForEach(x => x.DisableBlocks());
    }
    public void BackButtonClick() 
    {
        SceneManager.UnloadSceneAsync("Statue");
        SceneManager.LoadScene("LevelSelection_New");
    }
}

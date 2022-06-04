using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class LoadALD : MonoBehaviour
{
    public ALD ALD;

    public void Save()
    {
        string json = JsonUtility.ToJson(ALD);
        Debug.Log(json);
        Debug.Log(Application.persistentDataPath);
        File.WriteAllText(Application.persistentDataPath + "/ALD.json", json);
    }
    public void LoadData() {
        StartCoroutine("Load");
    }
    public IEnumerator Load()
    {
        string url = "https://rudreshgbs.github.io/MatchThree/ALD.json";
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();
        if (www.error == null)
        {
            Debug.Log(www.downloadHandler.text);
            string json = www.downloadHandler.text;
            JsonUtility.FromJsonOverwrite(json, this.ALD);
            Debug.Log(this.ALD.ALDModel.isvalid);
            Debug.Log(this.ALD.ALDModel.message);
        }
        else
        {
            Debug.Log("ERROR: " + www.error);
        }       
    }

}
